using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HolaAPI.Models;
using System.Data.Entity.Core;
using System.Data.SqlClient;

namespace HolaAPI.Controllers
{
    public class SalesController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        [Route("api/sales/GetSales/{agency_fk}/{PNR}/{type}")]
        [ResponseType(typeof(List<SaleDTO>))]
        [HttpGet]
        public IHttpActionResult GetSales(int agency_fk, string PNR, string type)

        {
            try
            {
                string[] types = type.Split(',');
                var sales = from a in db.Sales
                            orderby a.date_update
                            join b in db.Products on a.product_fk equals b.ID
                            where a.agency_fk == agency_fk && a.PNR == PNR && a.canceled == false
                            && types.Contains(b.category)
                            select new SaleDTO
                            {
                                ID = a.ID,
                                PNR = a.PNR,
                                agency_fk = a.agency_fk,
                                product_fk = a.product_fk,
                                product_name = b.name,
                                persons = a.persons,
                                remained_pay = a.remained_pay,
                                sale_type = a.sale_type,
                                date_sale = a.date_sale,
                                date_update = a.date_update,
                                comments = a.comments

                            };
                var sales_list = sales.ToList();
                return Ok(sales_list);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

        }




        [ResponseType(typeof(Sale))]
        public IHttpActionResult PostSale([FromBody]Sale sale)
        {
            if (sale.persons == 0) throw new Exception("persons cannot be zero");

            try
            {

                var existing_sale = (from a in db.Sales
                                     where a.product_fk == sale.product_fk && a.PNR == sale.PNR
                                     orderby a.date_update descending
                                     select a).FirstOrDefault();

                //It is possible to add more than one sale to each reservation (client) Unless it's 'other tour'
                var product = db.Products.SingleOrDefault(a => a.ID == sale.product_fk);
                if (existing_sale != null && existing_sale.canceled == false && (!(product.category == "tour" && product.subcat == "other")))
                {
                    return Content(HttpStatusCode.Conflict, "Sale already exists");
                }
                else
                {
                    sale.canceled = false;
                    sale.date_update = DateTime.Now;
                    sale.date_sale = DateTime.Today;
                    db.Sales.Add(sale);
                    AddSaleEvents(sale);

                }

                db.SaveChanges();
                return Ok("{}");
            }

            catch (Exception ex)
            {

                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }


        }



        [ResponseType(typeof(Sale))]
        [HttpPut]
        [Route("api/sales/CancelSale/{ID}")]
        public IHttpActionResult CancelSale(int ID)
        {
            try
            {
                var sale_to_update = db.Sales.SingleOrDefault(a => a.ID == ID && a.canceled == false);
                sale_to_update.canceled = true;
                sale_to_update.date_update = DateTime.Now;

                var soldActivities = db.SoldActivities.Where(a => a.sale_fk == ID);
                foreach (SoldActivity sa in soldActivities)
                {
                    sa.canceled = true;
                }

                db.SaveChanges();
                return Ok("{}");
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }



        [ResponseType(typeof(Sale))]
        [HttpPut]
        [Route("api/sales/UpdateSale/{ID}")]
        public IHttpActionResult UpdateSale([FromBody] Sale sale, int ID)
        {
            if (sale.persons == 0) throw new Exception("persons cannot be zero");
            try
            {
                var sale_to_update = db.Sales.SingleOrDefault(a => a.ID == ID && a.canceled == false);
                //Patch for null comment bug
                sale.comments = sale.comments ?? "";
                //TODO: next version: Block possibility for updaing price directly. enable only through Payments table

                sale_to_update.remained_pay = sale.remained_pay;
                sale_to_update.comments = sale.comments;
                sale_to_update.persons = sale.persons;
                sale_to_update.date_update = DateTime.Now;

                db.SaveChanges();
                return Ok("{}");
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }


        [ResponseType(typeof(Sale))]
        [HttpPut]
        [Route("api/sales/UpdateTransport/{ID}")]
        public IHttpActionResult UpdateTransport([FromBody] Sale sale, int ID)
        {
            try
            {
                var sale_to_update = db.Sales.SingleOrDefault(a => a.ID == ID && a.canceled == false);

                //TODO: next version Block possibility for updaing price directly. enable only through Payments table

                sale_to_update.remained_pay = sale.remained_pay;
                sale_to_update.comments = sale.comments;
                sale_to_update.product_fk = sale.product_fk;
                sale_to_update.date_update = DateTime.Now;

                //Cancel old sale events:
                var saleEvent_old = db.SoldActivities.Where(a => a.sale_fk == ID).ToList();
                saleEvent_old.ForEach(a => a.canceled = true);
                //Add the new ones:
                AddSaleEvents(sale_to_update);

                db.SaveChanges();
                return Ok("{}");
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                Logger.Write(ex.Message);
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }


        private void AddSaleEvents(Sale sale)
        {
            var activities = from a in db.Activities
                             join b in db.Rel_product_activity on a.ID equals b.activity_fk
                             where b.product_fk == sale.product_fk
                             select a;

            foreach (var activity in activities)
            {
                SoldActivity saleEvent = new SoldActivity();
                saleEvent.Sale = sale;
                saleEvent.PNR = sale.PNR;
                saleEvent.agency_fk = sale.agency_fk;
                saleEvent.date_update = sale.date_update;
                saleEvent.canceled = sale.canceled;
                saleEvent.activity_fk = activity.ID;
                saleEvent.event_fk = 0;
                db.SoldActivities.Add(saleEvent);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }




























        //not in use yet
        [ResponseType(typeof(List<SaleDTO>))]
        [HttpGet]
        public IHttpActionResult GetSales()
        {
            try
            {
                var sales = (from a in db.Sales
                             join b in db.Products on a.product_fk equals b.ID
                             where a.canceled == false
                             select new SaleDTO
                             {
                                 ID = a.ID,
                                 PNR = a.PNR,
                                 product_fk = a.product_fk,
                                 product_name = b.name,
                                 persons = a.persons,
                                 remained_pay = a.remained_pay,
                                 sale_type = a.sale_type,
                                 date_sale = a.date_sale,
                                 date_update = a.date_update,


                             }).ToList<SaleDTO>();
                return Ok(sales);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

        }


        //not in use yet
        [ResponseType(typeof(IQueryable<SaleDTO>))]
        [HttpGet]
        public IHttpActionResult GetSales(string PNR)
        {
            try
            {
                var sales = from a in db.Sales
                            where a.PNR == PNR &&
                             a.canceled == false
                            orderby a.date_update
                            join b in db.Products on a.product_fk equals b.ID
                            select new SaleDTO
                            {
                                ID = a.ID,
                                PNR = a.PNR,
                                product_fk = a.product_fk,
                                product_name = b.name,
                                persons = a.persons,
                                remained_pay = a.remained_pay,
                                sale_type = a.sale_type,
                                date_sale = a.date_sale,
                                date_update = a.date_update,
                                comments = a.comments

                            };
                return Ok(sales);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

        }


        ///not in use yet
        [ResponseType(typeof(Sale))]
        [HttpPut]
        [Route("api/sales/_UpdateSale/{field}")]
        public IHttpActionResult _UpdateSale([FromBody] SaleDTO sale, string field)
        {
            try
            {
                var sale_to_update = db.Sales.SingleOrDefault(a => a.PNR == sale.PNR && a.product_fk == sale.product_fk && a.canceled == false);
                sale_to_update.date_update = DateTime.Now;

                switch (field)
                {
                    case "price":
                        sale_to_update.remained_pay = sale.remained_pay;
                        break;
                    case "persons":
                        sale_to_update.persons = sale.persons;
                        break;
                    case "comments":
                        sale_to_update.comments = sale.comments;
                        break;
                    default:
                        break;
                }
                db.SaveChanges();
                return Ok(sale_to_update);
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }

    }
}

