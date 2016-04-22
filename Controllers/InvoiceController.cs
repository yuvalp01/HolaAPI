using HolaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace HolaAPI.Controllers
{
    public class InvoiceController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        public IQueryable<HotelDTO> GetHotels()
        {

            return db.Hotels.Select(a => new HotelDTO { ID = a.ID, name = a.name, address = a.address });
        }


        // 
        [ActionName("GetInvoiceSummary")]
        [HttpGet]
        public IQueryable<InvoiceSummary> GetInvoiceSummary([FromUri]int month, [FromUri]int year, [FromUri]int agency_fk)
        {

            return from a in db.Sales
                   join b in db.Clients on a.PNR equals b.PNR
                   join c in db.Products on a.product_fk equals c.ID
                   where b.date_arr.Month == month && b.date_arr.Year == year && b.agency_fk == agency_fk
                   group new { a, b, c } by new { b.date_arr, b.num_arr, rate= c.rate, c.name } into g
                   orderby g.Key.date_arr

                   select new InvoiceSummary
                   {
                       date_arr = g.Key.date_arr,
                       num_arr = g.Key.num_arr,
                       product = g.Key.name,
                       people = g.Sum(s => s.a.persons),
                       rate = g.Key.rate,
                       sum = g.Sum(s => s.a.persons) * g.Key.rate

                   };

        }



        //[ActionName("GetPlan")] 
        //[HttpGet]
        //public IQueryable<DepartPlanDTO> GetPlan([FromUri]string depart_list)
        //{

        //    return from a in db.DepartPlans
        //           where a.depart_list == depart_list
        //           join b in db.Hotels on a.hotel_fk equals b.ID
        //           orderby a.time
        //           select new DepartPlanDTO
        //           {
        //               depart_list = a.depart_list,
        //               hotel_fk = a.hotel_fk,
        //               hotel = b.name,
        //               time = a.time,
        //               PAX = a.PAX
        //           };

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
    public class InvoiceSummary
    {
        public DateTime date_arr { get; set; }
        public string num_arr { get; set; }
        public string product { get; set; }
        public int people { get; set; }
        public decimal rate { get; set; }
        public decimal sum { get; set; }

    }

}