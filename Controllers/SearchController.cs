using HolaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HolaAPI.Controllers
{

    public class SearchView
    {

        public string PNR { get; set; }
        public string names { get; set; }
        public int PAX { get; set; }
        public string num_arr { get; set; }
        public DateTime date_arr { get; set; }
        public string num_dep { get; set; }
        public DateTime? date_dep { get; set; }
        public string hotel { get; set; }
        public string agency { get; set; }
        public string phone { get; set; }



    }


    public class SearchController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        // GET: api/FITs
        public IQueryable<SearchView> GetSearch()
        {
            var search = from a in db.Clients
                         join  b in db.Hotels  on a.hotel_fk equals b.ID
                         join c in db.Agencies on a.agency_fk equals c.ID
                         select new SearchView {
                             PNR = a.PNR,
                             names = a.names,
                             PAX = a.PAX,
                             num_arr = a.num_arr,
                             date_arr =a.date_arr,
                             num_dep = a.num_dep,
                             date_dep = a.date_dep,
                             hotel = b.name,
                             agency = c.name,
                             phone = a.phone
                             

                             };
            return search;
        }



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
