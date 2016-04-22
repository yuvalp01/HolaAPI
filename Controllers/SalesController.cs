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

namespace HolaAPI.Controllers
{
    public class SalesController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        public IQueryable<SaleDTO> GetSales()
        {

            return from a in db.Sales
                   join b in db.Products on a.product_fk equals b.ID
                   where a.deleted == false && a.canceled == false
                    select new SaleDTO
                    {
                        PNR = a.PNR,
                        product_fk = a.product_fk,
                        product_name = b.name,
                        persons = a.persons,
                        price = a.price,
                        sale_type = a.sale_type,
                        date_sale = a.date_sale,
                        date_update = a.date_update,
                        paid = a.paid

                    };
        }

        public IQueryable<SaleDTO> GetSales(string PNR)
        {

            return from a in db.Sales
                   where a.PNR == PNR &&
                   a.deleted == false && a.canceled == false
                   join b in db.Products on a.product_fk equals b.ID
                   select new SaleDTO
                   {
                       PNR = a.PNR,
                       product_fk = a.product_fk,
                       product_name = b.name,
                       persons = a.persons,
                       price = a.price,
                       sale_type = a.sale_type,
                       date_sale = a.date_sale,
                       date_update = a.date_update,
                       paid = a.paid

                   };
        }


        [ResponseType(typeof(Sale))]
        public IHttpActionResult PostSale([FromBody]Sale sale)
        {
            try
            {
                var rate = db.Products.FirstOrDefault(a => a.ID == sale.product_fk).rate;
                sale.price = rate;
                sale.date_sale = DateTime.Today;
                sale.date_update = DateTime.Now;
                sale.deleted = false;
                sale.canceled = false;
                db.Sales.Add(sale);
                db.SaveChanges();
                return Ok(sale);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }


        }

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



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}

namespace HolaAPI.Models
{
    public class SaleDTO
    {

        public string PNR { get; set; }
        public int product_fk { get; set; }
        public string product_name { get; set; }
        public int persons { get; set; }
        public decimal price { get; set; }
        public string sale_type { get; set; }
        public DateTime date_sale { get; set; }
        public DateTime date_update { get; set; }
        public decimal paid { get; set; }

    }

}

