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
    public class FlightsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        public IQueryable<FlightDTO> GetFlights()
        {

            return db.Flights.Select(a => new FlightDTO
            {
                num = a.num,
                date = a.date,
                time = a.time,
                destination = a.destination,
                direction = a.direction,
                time_approved = a.time_approved,
                date_update = a.date_update


            });
        }

        public IQueryable<FlightDTO> GetFlights([FromUri] string num, string date)
        {
            DateTime _date = Convert.ToDateTime(date);
            return db.Flights.Where(a => a.num == num && a.date == _date).Select(a => new FlightDTO
            {
                num = a.num,
                date = a.date,
                time = a.time,
                destination = a.destination,
                direction = a.direction,
                time_approved = a.time_approved,
                date_update = a.date_update


            });
        }
        [HttpGet]
        public IQueryable<FlightDTO> GetFlights([FromUri] string direction)
        {

            return db.Flights.Where(a => a.direction == direction).Select(a => new FlightDTO
            {
                num = a.num,
                date = a.date,
                time = a.time,
                destination = a.destination,
                direction = a.direction,
                time_approved = a.time_approved,
                date_update = a.date_update,
                //flight_details = a.num + " - " + a.date.ToString("yyyy-MM-dd") + " - " + a.time


            });
        }

        [ActionName("getFlights2Days")]
        [HttpGet]
        public IQueryable<FlightDTO> getFlights2Days(string direction, string date)

        {
            direction = direction.ToUpper();
            DateTime _date = Convert.ToDateTime(date);
            DateTime date_next = _date.AddDays(1);

            var flights = db.Flights.Where(a => a.direction == direction && a.date >= _date && a.date <= date_next);
            if (direction == "IN")
            {

                return from a in db.Clients
                       let local_num = a.num_arr
                       let local_date = a.date_arr
                       group a by new { local_num, local_date } into g
                       join b in flights on new { num = g.Key.local_num, date = (DateTime)g.Key.local_date } equals new { b.num, date = b.date }
                       where b.date >= _date && b.date <= date_next
                       orderby new { b.date, b.time }
                       select new FlightDTO { num = g.Key.local_num, date = g.Key.local_date, time = b.time, direction = b.direction, sum = g.Sum(s => s.PAX), ph = string.Empty };

            }
            else
            {


                return from a in db.Clients
                       let local_num = a.num_dep
                       let local_date = a.date_dep
                       group a by new { local_num, local_date } into g
                       join b in flights on new { num = g.Key.local_num, date = (DateTime)g.Key.local_date } equals new { b.num, date = b.date }
                       where b.date >= _date && b.date <= date_next
                       orderby new { b.date, b.time }
                       select new FlightDTO { num = g.Key.local_num, date = g.Key.local_date.Value, time = b.time, direction = b.direction, sum = g.Sum(s => s.PAX), ph = string.Empty };

            }


            //var query = from a in db.Clients
            //            group a by new { a.num_arr, a.date_arr } into g
            //            join b in flights on new { num = g.Key.num_arr, date = (DateTime)g.Key.date_arr } equals new { b.num, date = b.date }

            //            //where b.direction == "IN" && b.date >= _date_arr && b.date <= date_next
            //            where b.date >= _date && b.date <= date_next
            //            select new FlightDTO { num = g.Key.num_arr, date = g.Key.date_arr, time = b.time, direction = b.direction, sum = g.Sum(s => s.PAX) , ph=string.Empty };
            //return query;

        }

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

                db.Flights.Add(flight);
                db.SaveChanges();
                return Ok(flight);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        // DELETE: api/Flights/5
        [ResponseType(typeof(Flight))]
        public IHttpActionResult Delete(int PNR)
        {
            try
            {
                Flight flight = db.Flights.Find(PNR);
                if (flight == null)
                {
                    return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", PNR));
                }

                db.Flights.Remove(flight);
                db.SaveChanges();
                return Ok(flight);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);

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
    public class FlightDTO
    {

        public string num { get; set; }
        public System.DateTime date { get; set; }
        public string time { get; set; }
        public string destination { get; set; }
        public string direction { get; set; }
        public Nullable<System.TimeSpan> time_approved { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
        public int sum { get; set; }
        public string ph { get; set; }

        //      public string flight_details { get; set; }
    }
    public class FlightsStats
    {
        public int IN { get; set; }
        public int OUT { get; set; }
        public int PAX_IN { get; set; }
        public int PAX_OUT { get; set; }

    }

}

