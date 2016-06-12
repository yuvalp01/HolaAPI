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
                            && types.Contains(b.type)
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
            try
            {

                var sale_to_add = (from a in db.Sales
                                   where a.product_fk == sale.product_fk && a.PNR == sale.PNR
                                   orderby a.date_update descending
                                   select a).FirstOrDefault();

                //It is possible to add more than one sale to each reservation (client) ONLY if it is 'other'
                if (sale_to_add != null && sale_to_add.canceled == false && sale_to_add.product_fk != 100)
                {
                    return Content(HttpStatusCode.Conflict, "Sale already exists");
                }
                else
                {
                    sale.canceled = false;
                    sale.date_update = DateTime.Now;
                    sale.date_sale = DateTime.Today;
                    db.Sales.Add(sale);

                }

                db.SaveChanges();
                return Ok(sale);
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
        //public IHttpActionResult CancelSale(  string PNR, int product_fk)
        //public IHttpActionResult CancelSale([FromBody] SaleDTO sale)
        public IHttpActionResult CancelSale(int ID)
        {
            try
            {
                //var sale_to_update = db.Sales.SingleOrDefault(a => a.PNR == sale.PNR && a.product_fk == sale.product_fk && a.canceled==false);
                var sale_to_update = db.Sales.SingleOrDefault(a => a.ID == ID && a.canceled == false);
                sale_to_update.canceled = true;
                sale_to_update.date_update = DateTime.Now;
                db.SaveChanges();
                return Ok(sale_to_update);
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
        public IHttpActionResult UpdateSale([FromBody] SaleDTO sale, int ID)
        {
            try
            {
                var sale_to_update = db.Sales.SingleOrDefault(a => a.ID == ID  && a.canceled == false);

                //TODO: Block possibility for updaing price directly. enable only through Payments table

                sale_to_update.remained_pay = sale.remained_pay;
                sale_to_update.comments = sale.comments;
                sale_to_update.persons = sale.persons;
                sale_to_update.date_update = DateTime.Now;
                db.SaveChanges();
                return Ok(sale_to_update);
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
        public IHttpActionResult UpdateTransport([FromBody] SaleDTO sale, int ID)
        {
            try
            {
                var sale_to_update = db.Sales.SingleOrDefault(a => a.ID == ID && a.canceled == false);

                //TODO: Block possibility for updaing price directly. enable only through Payments table

                sale_to_update.remained_pay = sale.remained_pay;
                sale_to_update.comments = sale.comments;
                sale_to_update.product_fk = sale.product_fk;
                sale_to_update.date_update = DateTime.Now;
                db.SaveChanges();
                return Ok(sale_to_update);
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                Logger.Write(ex.Message);
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
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

namespace HolaAPI.Models
{
    public class SaleDTO
    {

        public int ID { get; set; }
        public string PNR { get; set; }
        public int product_fk { get; set; }
        public string product_name { get; set; }
        public int persons { get; set; }
        public decimal remained_pay { get; set; }
        public string sale_type { get; set; }
        public DateTime date_sale { get; set; }
        public DateTime date_update { get; set; }
        public string comments { get; set; }

    }

}




//[ResponseType(typeof(Sale))]
//public IHttpActionResult PutSale([FromBody]Sale sale)
//{
//    try
//    {

//        var sale_to_update = db.Sales.SingleOrDefault(a => a.PNR == sale.PNR && a.product_fk == sale.product_fk);
//        sale_to_update.persons = sale.persons;
//        sale_to_update.comments = sale.comments;
//        sale_to_update.price = sale.price;
//        sale_to_update.date_update = DateTime.Now;

//        db.SaveChanges();
//        return Ok(sale_to_update);
//    }

//    catch (Exception ex)
//    {
//        Exception rootEx = ex.GetBaseException();
//        return Content(HttpStatusCode.BadRequest, rootEx.Message);
//    }
//}


//SqlException innerException = rootEx as SqlException;
//if (innerException != null && innerException.Number == 2627)
//{
//    return Content(HttpStatusCode.Conflict, innerException.Message);

//}


//var update_sale = db.Sales.SingleOrDefault(a => a.product_fk == sale.product_fk && a.PNR == sale.PNR && a.canceled==false);

//if (update_sale != null && update_sale.canceled == true)
//{

//    update_sale.price = sale.price;
//    update_sale.persons = sale.persons;
//    update_sale.sale_type = sale.sale_type;
//    update_sale.paid = sale.paid;
//    update_sale.comments = sale.comments;
//    update_sale.date_sale = sale.date_sale;
//    update_sale.date_update = DateTime.Now;
//    update_sale.canceled = false;
//    update_sale.deleted = false;
//}


//[ResponseType(typeof(Sale))]
//[HttpPut]
//[ActionName("UpdatePrice")]

//public IHttpActionResult UpdatePrice([FromBody] SaleDTO sale)
//{
//    try
//    {
//        var sale_to_update = db.Sales.SingleOrDefault(a => a.PNR == sale.PNR && a.product_fk == sale.product_fk&& a.canceled == false);
//        sale_to_update.price = sale.price;
//        sale_to_update.date_update = DateTime.Now;
//        db.SaveChanges();
//        return Ok(sale_to_update);
//    }

//    catch (Exception ex)
//    {
//        Exception rootEx = ex.GetBaseException();
//        return Content(HttpStatusCode.BadRequest, rootEx.Message);
//    }
//}


//[ActionName("UpdatePlan")]
//[HttpPut]
//public DepartPlanDTO UpdatePlan([FromBody] DepartPlanDTO line)
//{

//    DepartPlan line_to_update = db.DepartPlans.Find(line.depart_list, line.hotel_fk);
//    line_to_update.time = line.time;
//    db.SaveChanges();
//    return line;
//}






//// DELETE: api/Clients/5
//[ResponseType(typeof(Client))]
//public IHttpActionResult Delete(int PNR)
//{
//    try
//    {
//        Client client = db.Clients.Find(PNR);
//        if (client == null)
//        {
//            return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", PNR));
//        }

//        db.Clients.Remove(client);
//        db.SaveChanges();
//        return Ok(client);
//    }
//    catch (Exception ex)
//    {
//        return Content(HttpStatusCode.BadRequest, ex.Message);

//    }
//}
