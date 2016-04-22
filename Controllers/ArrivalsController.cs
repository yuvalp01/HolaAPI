using HolaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace HolaAPI.Controllers
{
    public class ArrivalsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();
        // GET: api/Arrival
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        public List<Arrival> GetArrivals([FromUri] string date_start, [FromUri] string flights)
        {
            ArrivalHelper helper = new ArrivalHelper();
            return helper.getListArrival(date_start, flights);

        }

        [ActionName("GetPlan")]
        [HttpGet]
        public List<TourPlanDTO> GetPlan([FromUri] string date_start, [FromUri] string date_end)
        {
            ArrivalHelper helper = new ArrivalHelper();
            return helper.getTourPlan(date_start, date_end);

        }


        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE: api/Arrival/5
        public void Delete(int id)
        {
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private List<SaleRow> getSalesTable()
        {
            using (HolaShalomDBEntities db = new HolaShalomDBEntities())
            {

                List<SaleRow> salesTable = (from a in db.Sales
                                            group a by a.PNR into g
                                            select new SaleRow { PNR = g.Key }).ToList();

                foreach (SaleRow row in salesTable)
                {
                    var products = from a in db.Sales
                                   where a.PNR == row.PNR
                                   group a by new { a.product_fk } into g
                                   select new { g.Key.product_fk, people = g.Sum(s => s.persons) };
                    foreach (var item in products)
                    {
                        switch (item.product_fk)
                        {

                            case 11:
                                row.MKF = item.people;
                                break;
                            case 12:
                                row.VDN = item.people;
                                break;
                            case 13:
                                row.FIG = item.people;
                                break;
                            case 14:
                                row.MON = item.people;
                                break;

                            default:
                                break;
                        }
                    }
                }
                return salesTable;

            }

        }
    }

}

namespace HolaAPI.Models
{
    public class Arrival_Info
    {

        public string date_start { get; set; }
        public string date_end { get; set; }
        public string flights { get; set; }

    }



}



//// POST: api/Arrival

//public List<Arrival> Post([FromBody] Arrival_Info arrival_info)

//{
//    DateTime date_start = Convert.ToDateTime(arrival_info.date_start);
//    string[] flights_array = arrival_info.flights.Split('~');
//    List<FlightDetails> flights = DataHelper.boxFlightInfo(arrival_info.flights);
//    string list_fk = arrival_info.date_start + "_" + flights[0].num;
//    list_fk = flights.Count > 1 ? list_fk + "_" + flights[1].num : list_fk;

//    List<SaleRow> salesTable = getSalesTable();
//    DateTime date1 = flights[0].date;
//    string num1 = flights[0].num;

//    var _clients = db.Clients.Select(a => a);
//    DateTime earliest_date_arrival = _clients.OrderBy(a => a.date_arr).FirstOrDefault().date_arr;
//    DateTime latest_date_departure = _clients.OrderByDescending(a => a.date_dep).FirstOrDefault().date_dep.Value;

//    //if (flights.Count == 1)
//    //{
//    //    _clients = _clients.Where(a => a.date_arr == date1 && a.num_arr == num1);
//    //}
//    /// MISSING FLIGHT LY102!!!
//    var clients_flight = _clients.Where(a => a.date_arr == date1 && a.num_arr == num1);

//    if (flights.Count == 2)
//    {
//        DateTime date2 = flights[1].date;
//        string num2 = flights[1].num;
//        var clients_flight_2 = _clients.Where(a => (a.date_arr == date1 && a.num_arr == num1) || (a.date_arr == date2 && a.num_arr == num2));
//        clients_flight.Union(clients_flight_2);

//    }

//    var query = from a in salesTable
//                join b in _clients on a.PNR equals b.PNR
//                join c in db.Hotels on b.hotel_fk equals c.ID
//                where b.date_arr == date_start
//                orderby c.name, b.names
//                select new Arrival
//                {
//                    PNR = b.PNR,
//                    names = b.names,
//                    hotel = c.name,
//                    phone = b.phone,
//                    PAX = b.PAX,
//                    MKF = a.MKF,
//                    VDN = a.VDN,
//                    FIG = a.FIG,
//                    MON = a.MON
//                };


//    return query.ToList<Arrival>();
//}


// PUT: api/Arrival/5