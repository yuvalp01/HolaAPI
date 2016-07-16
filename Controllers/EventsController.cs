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
using System.Data.Entity.Validation;
using System.Web.Http.Results;

namespace HolaAPI.Controllers
{
    public class EventsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        [HttpGet]
        [Route("api/events/{category}")]
        [ResponseType(typeof(List<EventDTO>))]
        public IHttpActionResult GetEvents(string category)
        {
            try
            {

                var events = from a in db.Events
                             join b in db.Activities on a.activity_fk equals b.ID
                             join c in db.Guides on a.guide_fk equals c.ID into gj
                             from d in gj.DefaultIfEmpty()
                             where b.category == category && a.canceled == false
                             select new EventDTO
                             {
                                 ID = a.ID,
                                 date = a.date,
                                 time = a.time.Value,
                                 activity_fk = a.activity_fk,
                                 activity_name = b.name,
                                 guide_fk = d.ID,
                                 guide_name = d.name,
                                 comments = a.comments,
                                 date_update = a.date_update,
                                 direction = b.direction,
                                 
                             };


                return Ok(events.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }


        [HttpGet]
        [Route("api/events/GetEventsWithActivities/{category}")]
        [ResponseType(typeof(List<EventDTO>))]
        public IHttpActionResult GetEventsWithActivities(string category)
        {
            try
            {

                var eventsWithSaleActivity = from a in db.Events
                                             where a.category == category && a.canceled == false
                                             join b in db.SoldActivities.Where(x => x.canceled == false ).GroupBy(c => c.event_fk) on a.ID equals b.Key.Value into jg
                                             from d in jg.DefaultIfEmpty()
                                             select new EventDTO
                                             {
                                                 people = d.Sum(s => (int?)s.Sale.persons) ?? 0,
                                                 ID = a.ID,
                                                 date = a.date,
                                                 time = a.time,
                                                 activity_name = a.Activity.name,
                                                 activity_fk = a.activity_fk,
                                                 guide_fk = a.guide_fk,
                                                 guide_name = a.Guide.name,
                                                 comments = a.comments,
                                                 direction = a.Activity.direction,
                                                 date_update  = a.date_update
                                             };

                List<EventDTO> events = eventsWithSaleActivity.ToList();


                return Ok(events.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }

        [HttpGet]
       // [Route("api/events/transport/{date}/{activity_fk}")]
        [Route("api/events/GetEventsWithActivities/{date}/{activity_fk}")]
        [ResponseType(typeof(List<EventDTO>))]
        public IHttpActionResult GetEvents(DateTime date, int activity_fk)
        {
            try
            {


                return Ok(getEvents(date, activity_fk));
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }


        public List<EventDTO> getEvents(DateTime date, int activity_fk)
        {
            var eventsWithSaleActivity = from a in db.Events
                                         where a.activity_fk == activity_fk && a.date == date && a.canceled == false
                                         join b in db.SoldActivities.Where(x => x.canceled == false && x.activity_fk == activity_fk).GroupBy(c => c.event_fk) on a.ID equals b.Key.Value into jg
                                         from d in jg.DefaultIfEmpty()
                                         select new EventDTO
                                         {
                                             people = d.Sum(s => (int?)s.Sale.persons) ?? 0,
                                             ID = a.ID,
                                             date = a.date,
                                             time = a.time,
                                             activity_name = a.Activity.name,
                                             guide_name = a.Guide.name,
                                             comments = a.comments,
                                             direction = a.Activity.direction,
                                         };

            List<EventDTO> events = eventsWithSaleActivity.ToList();
            return events;
        }

        [HttpPut]
        [Route("api/events/transport/{date}/{activity_fk}/{event_fk}")]
        //[ResponseType(typeof(List<SoldActivities>))]
        public IHttpActionResult PutInEvent(DateTime date, int activity_fk, int event_fk)
        {
            try
            {

                var saleActivityForEvent = from a in db.SoldActivities
                                           join b in db.Events on a.event_fk equals b.ID
                                           where a.activity_fk == activity_fk && b.date == date
                                           select a;
                foreach (SoldActivity sale_activity in saleActivityForEvent)
                {
                    sale_activity.event_fk = event_fk;
                }
                db.SaveChanges();

                List<EventDTO> events = getEvents(date, activity_fk);
                return Ok(events);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }



        [ResponseType(typeof(Event))]
        public IHttpActionResult Post([FromBody]Event _event)
        {
            try
            {
                int id_new = db.Events.Max(a => a.ID) + 1;
                _event.ID = id_new;
                _event.date_update = DateTime.Now;
                _event.canceled = false;
                db.Events.Add(_event);
                db.SaveChanges();
                return Ok(_event);
            }

            catch (DbEntityValidationException ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }
        }




        [ResponseType(typeof(Event))]
        [HttpPost]
        [Route("api/events/CreateTransportList")]
        public IHttpActionResult CreateTransportList([FromBody]Event _event)
        {

            try
            {
                _event.date_update = DateTime.Now;
                _event.canceled = false;
                db.Events.Add(_event);

                db.SaveChanges();
                return Ok(_event);
            }

            catch (DbEntityValidationException ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }
        }





        [ResponseType(typeof(Event))]
        [HttpPut]
        [Route("api/events/CancelEvent/{ID}")]
        public IHttpActionResult CancelEvent(int ID)
        {
            try
            {
                //cancel event:
                var event_to_cancel = db.Events.SingleOrDefault(a => a.ID == ID && a.canceled == false);
                event_to_cancel.canceled = true;
                event_to_cancel.date_update = DateTime.Now;
                //cancel all activities in the event:
                var sale_activities = db.SoldActivities.Where(a => a.event_fk == ID && a.canceled == false);
                sale_activities.ToList().ForEach(a => a.event_fk = 0);

                //var depart_plans = db.DepartPlans.Where(a => a.event_fk == ID
                //delete from DepartPlans table
                if (event_to_cancel.Activity.direction=="OUT")
                {
                    db.Database.ExecuteSqlCommand("delete from DepartPlan where event_fk=" + ID);
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


        [ResponseType(typeof(Event))]
        [HttpPut]
        [ActionName("UpdateEvent")]
        [Route("api/events/UpdateEvent/{ID}")]
        public IHttpActionResult UpdateEvent([FromBody] EventDTO _event, int ID)
        {
            try
            {
                var event_to_update = db.Events.SingleOrDefault(a => a.ID == ID && a.canceled == false);
                event_to_update.time = _event.time;
                event_to_update.guide_fk = _event.guide_fk;
                event_to_update.comments = _event.comments;
                event_to_update.date_update = DateTime.Now;
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
    public class EventDTO
    {
        public int ID { get; set; }
        public DateTime date { get; set; }
        public TimeSpan? time { get; set; }
        public int activity_fk { get; set; }
        public string activity_name { get; set; }
        public int? guide_fk { get; set; }
        public string guide_name { get; set; }
        public string comments { get; set; }
        public string category { get; set; }
        public DateTime date_update { get; set; }
        public bool canceled { get; set; }
        public int? people { get; set; }
        public string direction { get; set; }

        
        public EventDTO()
        {

        }
        public EventDTO(Event _event)
        {
            this.ID = _event.ID;
            this.date = _event.date;
            this.time = _event.time.Value;
            //this.activity_name = _event.activity_name;
            this.guide_fk = _event.guide_fk;
            //this.guide_name = _event.guide_name;
            this.comments = _event.comments;
            this.category = _event.category;
            this.date_update = _event.date_update;
            this.canceled = _event.canceled;
            //this.people = _event.people;

        }
    }
}



//var ttt = xxx.ToList();

//var eventsWithSaleActivity = from a in db.SoldActivities
//                             where a.canceled == false
//                             join b in db.Events on a.event_fk equals b.ID into jg
//                             from c in jg.DefaultIfEmpty()
//                             where c.activity_fk == activity_fk && c.date == date && c.canceled == false
//                             group new { a, c } by new { c.ID, c.time, activity_name = c.Activity.name, guid_name = c.Guide.name, c.comments } into g
//                             select new EventDTO
//                             {
//                                 ID = g.Key.ID,
//                                 time = g.Key.time.Value,
//                                 activity_name = g.Key.activity_name,
//                                 guide_name = g.Key.guid_name,
//                                 comments = g.Key.comments,
//                                 people = g.Sum(s => s.a.Sale.persons)
//                             };