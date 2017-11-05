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
using System.Linq.Expressions;

namespace HolaAPI.Controllers
{
    public class FlightsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        [ResponseType(typeof(List<FlightDTO>))]
        [HttpGet]
        public IHttpActionResult GetFlights()
        {
            try
            {
                var flights = db.Flights.Select(a => new FlightDTO
                {
                    num = a.num,
                    date = a.date,
                    time = a.time,
                    destination = a.destination,
                    direction = a.direction,
                    date_update = a.date_update
                })
                //.Where(a=>a.date>=DateTime.Today)
                .OrderBy(a=>new { a.date,a.time}).ToList();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }


        [ResponseType(typeof(List<FlightDTO>))]
        public IHttpActionResult GetFlights([FromUri] string num, string date)
        {
            try
            {
                DateTime _date = Convert.ToDateTime(date);
                var flights = db.Flights.Where(a => a.num == num && a.date == _date).Select(a => new FlightDTO
                {
                    num = a.num,
                    date = a.date,
                    time = a.time,
                    destination = a.destination,
                    direction = a.direction,
                    //time_approved = a.time_approved,
                    date_update = a.date_update


                }).ToList();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }
        [HttpGet]
        [ResponseType(typeof(List<FlightDTO>))]
        public IHttpActionResult GetFlights([FromUri] string direction)
        {
            try
            {
                var flights = db.Flights.Where(a => a.direction == direction).Select(a => new FlightDTO
                {
                    num = a.num,
                    date = a.date,
                    time = a.time,
                    destination = a.destination,
                    direction = a.direction,
                    //time_approved = a.time_approved,
                    date_update = a.date_update,

                }).ToList();
                return Ok(flights);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }









        //TODO: replace
        [ResponseType(typeof(FlightsStats))]
        //[ActionName("GetFlightsStats")]
        [HttpGet]
        [Route("api/flights/GetFlightsStats/{date}")]
        public IHttpActionResult GetFlightsStats(string date)
        {
            try
            {
                DateTime _date = Convert.ToDateTime(date);
                FlightsStats stats = new FlightsStats();
                var flights = db.Flights.Where(a => a.date == _date);
                stats.IN = flights.Count(a => a.direction == "IN");
                stats.OUT = flights.Count(a => a.direction == "IN");

                stats.PAX_IN = db.Clients.Where(a => a.Flight.direction == "IN" && a.Flight.date == _date).Count();
                stats.PAX_OUT = db.Clients.Where(a => a.Flight1.direction == "OUT" && a.Flight1.date == _date).Count();


					 var clients = from a in db.Clients
										where a.Flight.direction == "IN" && a.Flight.date == _date
										select a;
					 var sum = clients.Sum(a => a.PAX);
					 IQueryable query = db.Clients.Where(a => a.Flight.direction == "IN" && a.Flight.date == _date);

					 var xxx = query.ToListAsync();

		


					  var sql = ((System.Data.Entity.Core.Objects.ObjectQuery)query).ToTraceString();


					 return Ok(stats);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }


        }


        [ResponseType(typeof(Flight))]
        public IHttpActionResult Post([FromBody]Flight flight)
        {
            try
            {
                flight.date_update = DateTime.Now;
                db.Flights.Add(flight);
                db.SaveChanges();
                return Ok(flight);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }


        }







        [ResponseType(typeof(Flight))]
        [HttpPut]
        [ActionName("UpdateFlight")]
        //[Route("api/flights/UpdateEvent/{num}/{date}/{time}")]
        public IHttpActionResult UpdateFlight([FromBody] Flight client_flight )
        {
            try
            {
                Flight db_flight = db.Flights.Find(client_flight.num, client_flight.date);
                db_flight.time = client_flight.time;
                db_flight.date_update = DateTime.Now;

                db.SaveChanges();
                return Ok("{}");
            }


            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }





        [HttpDelete]
        [Route("api/flights/DeleteFlight/{num}/{date}")]
        public IHttpActionResult DeleteFlight(string num, DateTime date)
        {
            try
            {
                Flight flight = db.Flights.Find(num,date);
                if (flight == null)
                {
                    return Content(HttpStatusCode.NotFound, string.Format("Flight '{0}' on {1} does not exist in the table.", num,date.ToString("YYYY-MM-dd")));
                }

                db.Flights.Remove(flight);
                db.SaveChanges();
                return Ok("{}");
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
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


    }
}

namespace HolaAPI.Models
{




    public class FlightsStats
    {
        public int IN { get; set; }
        public int OUT { get; set; }
        public int PAX_IN { get; set; }
        public int PAX_OUT { get; set; }

    }

}


















//[Route("api/flights/GetFlightTransportDep/{date}/{activity_fk}")]
//[ResponseType(typeof(IQueryable<FlightDTO>))]
//[HttpGet]
//public IHttpActionResult GetFlightTransportDep(DateTime date, int activity_fk)
//{

//    //Expression<Func<Client, string>> num_flight = a => a.num_dep;
//    //Expression<Func<Client, DateTime>> date_flight = a => a.date_dep;

//    DateTime _date = Convert.ToDateTime(date);
//    DateTime date_next = _date.AddDays(1);
//    //Get all the activity sales for this route
//    var all_activities = from a in db.rel_sale_activities
//                         join b in db.Sales on a.sale_fk equals b.ID
//                         join c in db.Clients on new { b.PNR, b.agency_fk } equals new { c.PNR, c.agency_fk }
//                         join d in db.Events on a.event_fk equals d.ID into gj
//                         from e in gj.DefaultIfEmpty()
//                         where a.activity_fk == activity_fk && a.canceled == false
//                         select new
//                         {
//                             a.ID,
//                             c.date_dep,
//                             c.num_dep,
//                             b.persons,
//                             a.activity_fk,
//                             event_fk = (e == null ? 0 : e.ID)
//                         };
//    //Get only the ones that still not assign and group them by:
//    var all_not_assign = from n in all_activities
//                         group n by new { n.num_dep, n.date_dep, n.event_fk  } into g
//                         where g.Key.event_fk == 0
//                         select new
//                         {
//                             num = g.Key.num_dep,
//                             date = g.Key.date_dep,
//                             sum = g.Sum(a => a.persons)
//                         };
//   //Combine with real flights
//    var flights = from a in all_not_assign
//                  group a by new { a.date, a.num } into g
//                  join f in db.Flights on new {  g.Key.date, g.Key.num } equals new { f.date, f.num }
//                  let last_hour = new TimeSpan(6, 0, 0)
//                  where f.date == _date || (f.date == date_next && f.time < last_hour)
//                  select new FlightDTO { num = g.Key.num, date = g.Key.date, time = f.time, sum = g.Sum(s => s.sum) };

