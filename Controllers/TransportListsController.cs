using HolaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace HolaAPI.Controllers
{
    public class TransportListsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        [Route("api/transportlists/GetFlights/{direction}/{activity_fk}/{date}")]
        [ResponseType(typeof(IQueryable<FlightDTO>))]
        [HttpGet]
        public IHttpActionResult GetFlightTransport(string direction, int activity_fk,  DateTime date)
        {

            List<SaleActivityDTO> notAssignPassengers = GetPotentialPassengers(date, activity_fk, direction);

            //Get only the ones that still not assign and group them by:
            var group_not_assign = from n in notAssignPassengers
                                   group n by new { n.flight, n.event_fk } into g
                                   where g.Key.event_fk == 0
                                   select new
                                   {
                                       num = g.Key.flight.num,
                                       date = g.Key.flight.date,
                                       sum = g.Sum(a => a.persons)
                                   };
            //Combine with real flights
            var flights = from a in group_not_assign
                          group a by new { a.date, a.num } into g
                          join f in db.Flights on new { g.Key.date, g.Key.num } equals new { f.date, f.num }
                          orderby f.date, f.time
                          select new FlightDTO { num = g.Key.num, date = g.Key.date, time = f.time, sum = g.Sum(s => s.sum) };

            return Ok(flights);
        }

        public List<SaleActivityDTO> GetPotentialPassengers(DateTime date, int activity_fk, string direction)
        {

            //TODO: separate between event 0 and the rest

            direction = direction.ToLower();
            DateTime _date = Convert.ToDateTime(date);
            DateTime date_next = _date.AddDays(1);
            TimeSpan last_hour = new TimeSpan(6, 0, 0);

            Expression<Func<Client, bool>> whereFlights = a => a.Flight.date == _date || (a.Flight.date == date_next && a.Flight.time < last_hour);
            if (direction == "out")
            {
                whereFlights = a => a.Flight1.date == _date || (a.Flight1.date == date_next && a.Flight1.time < last_hour);
            }

            //Get all the activity sales for this route
            var unassign_activities = from a in db.SoldActivities
                                      join b in db.Sales on a.sale_fk equals b.ID
                                      join c in db.Clients.Where(whereFlights) on new { b.PNR, b.agency_fk } equals new { c.PNR, c.agency_fk }
                                      join d in db.Events on a.event_fk equals d.ID into gj
                                      from e in gj.DefaultIfEmpty()
                                      where a.activity_fk == activity_fk && a.canceled == false
                                      select new SaleActivityDTO
                                      {
                                          ID = a.ID,
                                          PNR = c.PNR,
                                          agency_fk = c.agency_fk,
                                          sale_fk = a.sale_fk,
                                          hotel_fk = c.hotel_fk,
                                          hotel_name = c.Hotel.name,
                                          activity_fk = activity_fk,
                                          activity_name = a.Activity.name,
                                          persons = a.Sale.persons,
                                          event_fk = (e == null ? 0 : e.ID),
                                          date_activity = (e == null ? new DateTime() : e.date_update),
                                          agency_name = c.Agency.name,
                                          date_update = a.date_update,
                                          flight = direction == "in" ? c.Flight : c.Flight1
                                      };
            List<SaleActivityDTO> list = unassign_activities.ToList();
            return list;
        }


        [ActionName("AssignPassengers")]
        [ResponseType(typeof(List<EventDTO>))]
        [HttpPut]
        public IHttpActionResult AssignPassengers([FromBody]ListDetails details)
        {
            List<DateTime> dates = details.flights.Select(a => a.date).ToList();
            List<string> nums = details.flights.Select(a => a.num).ToList();
            Expression<Func<Client, bool>> whereSelectedFlights = a => dates.Contains(a.date_arr) && nums.Contains(a.num_arr);

            if (details.direction == "out")
            {
                whereSelectedFlights = a => dates.Contains(a.date_dep) && nums.Contains(a.num_dep);
            }


            var soldActivies = from a in db.SoldActivities
                               join b in db.Sales on a.sale_fk equals b.ID
                               join c in db.Clients.Where(whereSelectedFlights) on new { b.PNR, b.agency_fk } equals new { c.PNR, c.agency_fk }
                               where a.activity_fk == details.activity_fk && a.canceled == false
                               select a;

            foreach (SoldActivity se in soldActivies)
            {
                se.event_fk = details.event_fk;
            }
            db.SaveChanges();
            EventsController eventCtrl = new EventsController();
            return Ok(eventCtrl.getEvents(details.date, details.activity_fk));

        }


        [ResponseType(typeof(List<DepartPlanDTO>))]
        [ActionName("GetCreatePlan")]
        [HttpPost]
        public IHttpActionResult GetCreatePlan([FromBody] ListDetails details)
        {
            try
            {


                DateTime dateStart = details.date;
                List<Flight> selected_flights = details.flights;

                List<DateTime> dates = selected_flights.Select(a => a.date).ToList();
                List<string> nums = selected_flights.Select(a => a.num).ToList();

                var unAssignPassengers = GetPotentialPassengers(dateStart, details.activity_fk, "out");
                var selectedInFlights = unAssignPassengers.Where(a => dates.Contains(a.flight.date) && nums.Contains(a.flight.num));
                var departPlan = from a in selectedInFlights
                                 group a by new { a.hotel_fk, a.hotel_name } into g
                                 select new DepartPlan
                                 {
                                     hotel_fk = g.Key.hotel_fk,
                                     // hotel_name = g.Key.hotel_name,
                                     PAX = g.Sum(a => a.persons),
                                     event_fk = details.event_fk
                                 };


                //Clear past lists:
                db.DepartPlans.RemoveRange(db.DepartPlans.Where(a => a.event_fk == details.event_fk));

                db.DepartPlans.AddRange(departPlan.ToList());
                db.SaveChanges();

                return Ok(getDepartPlan(details.event_fk));
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }
        }




        [ResponseType(typeof(IQueryable<DepartPlanDTO>))]
        [Route("api/transportlists/GetDepartPlan/{event_fk}")]
        [HttpGet]
        public IHttpActionResult GetDepartPlan(int event_fk)
        {
            try
            {
                var plan = getDepartPlan(event_fk);
                return Ok(plan);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
                throw;
            }


        }



        private List<DepartPlanDTO> getDepartPlan(int event_fk)
        {

            var query = from a in db.DepartPlans
                        join b in db.Hotels on a.hotel_fk equals b.ID
                        where a.event_fk == event_fk
                        orderby a.time
                        select new DepartPlanDTO
                        {
                            event_fk = a.event_fk,
                            hotel_fk = a.hotel_fk,
                            hotel_name = b.name,
                            time = a.time,
                            PAX = a.PAX

                        };

            return query.ToList();

        }


        [ResponseType(typeof(DepartPlanDTO))]
        [ActionName("UpdateDepartPlan")]
        [HttpPut]
        public IHttpActionResult UpdateDepartPlan([FromBody] DepartPlanDTO line)
        {
            try
            {
                DepartPlan line_to_update = db.DepartPlans.Find(line.event_fk, line.hotel_fk);
                line_to_update.time = line.time;
                db.SaveChanges();
                return Ok(line);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
                throw;
            }

        }




        [ResponseType(typeof(DepartPlanDTO))]
        [ActionName("UpdateDepartEventTime")]
        [HttpPut]
        public IHttpActionResult UpdateDepartEventTime([FromUri] int event_fk)
        {
            try
            {
                DepartPlan earliestTime = db.DepartPlans.Where(a=>a.event_fk ==event_fk ).OrderBy(a=>a.time).FirstOrDefault();
                db.Events.Find(event_fk).time = earliestTime.time;
                db.SaveChanges();
                return Ok("{}");
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
                throw;
            }

        }




        









        //public List<EventDTO> GetTourPlan(DateTime dateStart, int event_fk)
        //{

        //    var sale_activities = from a in db.rel_sale_activities
        //                          where a.event_fk == event_fk && a.canceled == false
        //                          select a;

        //    var clients = from a in db.Clients
        //                  join b in db.Sales on new { a.PNR, a.agency_fk } equals new { b.PNR, b.agency_fk }
        //                  select new { a.PNR, a.agency_fk, a.date_dep, sale_fk = b.ID };


        //    DateTime dateLast = (from a in clients
        //                         join b in sale_activities on a.sale_fk equals b.sale_fk
        //                         orderby a.date_dep descending
        //                         select a.date_dep).FirstOrDefault();
        //    var tour_plan = from a in db.Events
        //                    where a.date >= dateStart && a.date <= dateLast && a.category == "tour"
        //                    select new EventDTO
        //                    {
        //                        ID = a.ID,
        //                        date = a.date,
        //                        time = a.time.Value,
        //                        activity_fk = a.activity_fk,
        //                        activity_name = a.Activity.name,
        //                        guide_fk = a.guide_fk,
        //                        guide_name = a.Guide.name,
        //                        comments = a.comments,
        //                        date_update = a.date_update
        //                    };
        //    List<EventDTO> tour_planDTO = new List<EventDTO>(tour_plan);
        //    return tour_planDTO.ToList(); ;

        //}

        //public List<ArrivalRow> GetPassengersList(int event_fk)
        //{

        //    var sale_activities_in = (from a in db.rel_sale_activities
        //                              where a.event_fk == event_fk && a.canceled == false //&& a.Activity.direction == "IN"
        //                              select a);
        //    List<string> PNRs = sale_activities_in.Select(b => b.PNR).ToList();
        //    List<ArrivalRow> arrivals = new List<ArrivalRow>();
        //    var existing_tours = (from a in db.rel_sale_activities
        //                          where PNRs.Contains(a.PNR) && a.Activity.category == "tour"
        //                          select (a.activity_fk)).Distinct();


        //    foreach (rel_sale_activities sa in sale_activities_in)
        //    {
        //        ArrivalRow arrival = new ArrivalRow();
        //        arrival.PNR = sa.PNR;
        //        arrival.names = db.Clients.SingleOrDefault(a => a.PNR == sa.PNR && a.agency_fk == sa.Sale.agency_fk).names;
        //        arrival.agency_name = sa.Agency.name;

        //        foreach (var activity_fk in existing_tours)
        //        {
        //            var tour_activities = db.rel_sale_activities.Where(a => a.PNR == sa.Sale.PNR && a.agency_fk == sa.Sale.agency_fk && sa.canceled == false && sa.activity_fk == activity_fk);
        //            arrival.activities.Add(new ActivityPair()
        //            {
        //                activity_fk = activity_fk,
        //                sum = tour_activities.Sum(a => (int?)a.Sale.persons) ?? 0
        //            });
        //        }
        //        arrivals.Add(arrival);
        //    }
        //    return arrivals;


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


public class TransportList
{
    public string activity_name { get; set; }
    public string guide_name { get; set; }
    public string comments_trans { get; set; }
    public DateTime date { get; set; }
    public TimeSpan pickup_time { get; set; }
    public List<PassengersRow> passengers { get; set; }
    public List<EventDTO> tour_plan { get; set; }
    public List<string> agencies { get; set; }

}



public class PassengersRow
{
    public string PNR { get; set; }
    public string names { get; set; }
    public string agency_name { get; set; }
    public string hotel_name { get; set; }
    public int hotel_fk { get; set; }
    public string phone { get; set; }
    public int PAX { get; set; }
    public int event_fk { get; set; }
    public TimeSpan time { get; set; }
    public string title { get; set; }
    public List<ActivityPair> activities { get; set; }

    public PassengersRow()
    {
        activities = new List<ActivityPair>();
    }
    //public int MKF { get; set; }
    //public int VDN { get; set; }
    //public int FIG { get; set; }
    //public int MON { get; set; }

}

public class ActivityPair
{
    public int activity_fk { get; set; }
    public int sum { get; set; }
}

public class ListDetails
{

    public DateTime date { get; set; }
    public TimeSpan time { get; set; }
    public int activity_fk { get; set; }
    public int event_fk { get; set; }
    public int? guide_fk { get; set; }
    public string comments_trans { get; set; }
    public string direction { get; set; }
    public int people { get; set; }
    public List<Flight> flights { get; set; }


}


public class SaleActivityDTO
{


    public int ID { get; set; }
    public string PNR { get; set; }
    public int agency_fk { get; set; }
    public string agency_name { get; set; }
    public int hotel_fk { get; set; }
    public string hotel_name { get; set; }
    public int persons { get; set; }
    public int sale_fk { get; set; }
    public int activity_fk { get; set; }
    public string activity_name { get; set; }
    public int event_fk { get; set; }
    public DateTime date_activity { get; set; }
    public DateTime date_update { get; set; }
    public Flight flight { get; set; }


}












//[ResponseType(typeof(List<DepartPlanDTO>))]
//[ActionName("CreatePlan_")]
//[HttpPost]
//public IHttpActionResult CreatePlan_([FromBody] ListDetails details)
//{
//    try
//    {
//        if (details.event_fk == 0)
//        {
//            EventsController eventCtrl = new EventsController();
//            Event newEvent = new Event()
//            {
//                activity_fk = details.activity_fk,
//                canceled = false,
//                category = "transport",
//                comments = details.comments_trans,
//                date = details.date,
//                date_update = DateTime.Now,
//                guide_fk = details.guide_fk
//            };
//            eventCtrl.Post(newEvent);
//            db.SaveChanges();
//            details.event_fk = newEvent.ID;
//        }
//        //int event_fk = details.event_fk;
//        DateTime dateStart = details.date;
//        List<Flight> selected_flights = details.flights;
//        //Clear past lists:
//        db.DepartPlans.RemoveRange(db.DepartPlans.Where(a => a.event_fk == details.event_fk));

//        List<DateTime> dates = selected_flights.Select(a => a.date).ToList();
//        List<string> nums = selected_flights.Select(a => a.num).ToList();

//        var unAssignPassengers = GetPotentialPassengers(dateStart, details.activity_fk, "out");
//        var selectedFlights = unAssignPassengers.Where(a => dates.Contains(a.flight.date) && nums.Contains(a.flight.num));
//        var departPlan = from a in selectedFlights
//                         group a by new { a.hotel_fk, a.hotel_name } into g
//                         select new DepartPlan
//                         {
//                             hotel_fk = g.Key.hotel_fk,
//                             // hotel_name = g.Key.hotel_name,
//                             PAX = g.Sum(a => a.persons),
//                             event_fk = details.event_fk
//                         };



//        db.DepartPlans.AddRange(departPlan.ToList());
//        db.SaveChanges();


//        return Ok(getDepartPlan(details.event_fk));
//    }
//    catch (Exception ex)
//    {
//        Exception rootEx = ex.GetBaseException();
//        return Content(HttpStatusCode.InternalServerError, rootEx.Message);
//    }
//}




//[ActionName("UpdateEventSaleEventsArr")]
//[ResponseType(typeof(EventDTO))]
//[HttpPost]
//public IHttpActionResult UpdateEventSaleEventsArr([FromBody]ListDetails details)
//{
//    int id_new = db.Events.Max(a => a.ID) + 1;
//    Event new_evenet = new Event()
//    {
//        ID = id_new,
//        activity_fk = details.activity_fk,
//        canceled = false,
//        category = "transport",
//        comments = details.comments_trans,
//        date = details.date,
//        date_update = DateTime.Now,
//        guide_fk = details.guide_fk,
//        time = details.time
//    };

//    db.Events.Add(new_evenet);
//    db.SaveChanges();


//    return Ok(new EventDTO(new_evenet));
//}


//[ActionName("GetArrivalList")]
//[ResponseType(typeof(ArrivalList))]
//[HttpPut]
//public IHttpActionResult GetArrivalList([FromBody]ListDetails details)
//{
//    ArrivalList list = new ArrivalList();
//    list.activity_name = db.Activities.SingleOrDefault(a => a.ID == details.activity_fk).name;
//    Event _event = db.Events.SingleOrDefault(a => a.ID == details.event_fk);
//    list.comments_trans = _event.comments;
//    list.date = _event.date;
//    list.guide_name = db.Guides.SingleOrDefault(a => a.ID == _event.guide_fk).name;
//    list.passengers = GetPassengersList(details.event_fk);
//    list.pickup_time = _event.time.Value;
//    list.tour_plan = GetTourPlan(_event.date, details.event_fk);

//    return Ok(list);
//}


//[ActionName("GetDepartureList")]
//[ResponseType(typeof(ArrivalList))]
//[HttpPut]
//public IHttpActionResult GetDepartureList([FromBody]ListDetails details)
//{
//    ArrivalList list = new ArrivalList();
//    list.activity_name = db.Activities.SingleOrDefault(a => a.ID == details.activity_fk).name;
//    Event _event = db.Events.SingleOrDefault(a => a.ID == details.event_fk);
//    list.comments_trans = _event.comments;
//    list.date = _event.date;
//    list.guide_name = db.Guides.SingleOrDefault(a => a.ID == _event.guide_fk).name;
//    list.passengers = GetPassengersList(details.event_fk);
//    list.pickup_time = _event.time.Value;

//    return Ok(list);
//}





















//[ResponseType(typeof(List<Arrival>))]
//public IHttpActionResult GetArrivals([FromUri] string date_start, [FromUri] string flights)
//{
//    try
//    {
//        ArrivalHelper helper = new ArrivalHelper();
//        var arrivals = helper.getListArrival(date_start, flights);
//        return Ok(arrivals);
//    }
//    catch (Exception ex)
//    {
//        Exception rootEx = ex.GetBaseException();
//        return Content(HttpStatusCode.InternalServerError, rootEx.Message);
//    }


//}

//[ResponseType(typeof(List<EventDTO>))]
//[ActionName("GetPlan")]
//[HttpGet]
//public IHttpActionResult GetPlan([FromUri] string date_start, [FromUri] string date_end)
//{
//    try
//    {
//        ArrivalHelper helper = new ArrivalHelper();
//        var tourPlan = helper.getTourPlan(date_start, date_end);
//        return Ok(tourPlan);
//    }
//    catch (Exception ex)
//    {
//        Exception rootEx = ex.GetBaseException();
//        return Content(HttpStatusCode.InternalServerError, rootEx.Message);
//    }


//}












//if (details.event_fk == 0)
//{
//    EventsController eventCtrl = new EventsController();
//    Event newEvent = new Event()
//    {
//        activity_fk = details.activity_fk,
//        canceled = false,
//        category = "transport",
//        comments = details.comments_trans,
//        date = details.date,
//        date_update = DateTime.Now,
//        guide_fk = details.guide_fk
//    };
//    eventCtrl.Post(newEvent);
//    db.SaveChanges();
//    details.event_fk = newEvent.ID;
//}
//int event_fk = details.event_fk;

//private List<SaleRow> getSalesTable()
//{
//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {

//        List<SaleRow> salesTable = (from a in db.Sales
//                                    group a by a.PNR into g
//                                    select new SaleRow { PNR = g.Key }).ToList();

//        foreach (SaleRow row in salesTable)
//        {
//            var products = from a in db.Sales
//                           where a.PNR == row.PNR
//                           group a by new { a.product_fk } into g
//                           select new { g.Key.product_fk, people = g.Sum(s => s.persons) };
//            foreach (var item in products)
//            {
//                switch (item.product_fk)
//                {

//                    case 11:
//                        row.MKF = item.people;
//                        break;
//                    case 12:
//                        row.VDN = item.people;
//                        break;
//                    case 13:
//                        row.FIG = item.people;
//                        break;
//                    case 14:
//                        row.MON = item.people;
//                        break;

//                    default:
//                        break;
//                }
//            }
//        }
//        return salesTable;

//    }

//}








//namespace HolaAPI.Models
//{
//    public class Arrival_Info
//    {

//        public string date_start { get; set; }
//        public string date_end { get; set; }
//        public string flights { get; set; }

//    }



//}



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