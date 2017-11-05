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
using System.Linq.Expressions;

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
        [Route("api/events/GetEvent/{ID}")]
        [ResponseType(typeof(EventDTO))]
        public IHttpActionResult GetEvent(int ID)
        {
            try
            {

                var events = from a in db.Events
                             join b in db.Activities on a.activity_fk equals b.ID
                             join c in db.Guides on a.guide_fk equals c.ID into gj
                             from d in gj.DefaultIfEmpty()
                             where a.ID == ID && a.canceled == false
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


                return Ok(events.FirstOrDefault());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }


        [HttpGet]
        [Route("api/events/GetEventsList/{category}/{daysAgo}")]
        [ResponseType(typeof(List<EventDTO>))]
        public IHttpActionResult GetEventsList(string category, int daysAgo )
        {
            try
            {
                DateTime lastDate = DateTime.Today.AddDays(-daysAgo);
                Expression<Func<Event, bool>> whereLast7days = a => lastDate < a.date;

                var eventsWithSaleActivity = from a in db.Events.Where(whereLast7days)
                                             where a.category == category && a.canceled == false 
                                             join b in db.SoldActivities.Where(x => x.canceled == false).GroupBy(c => c.event_fk) on a.ID equals b.Key.Value into jg
                                             from d in jg.DefaultIfEmpty()
                                             orderby a.date_update descending
                                             //where a.date > DateTime.Today.AddDays(7)
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
                                                 date_update = a.date_update
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




        [HttpPost]
        [Route("api/events/DuplicateEvent/{ID}")]
        [ResponseType(typeof(EventDTO))]
        public IHttpActionResult DuplicateEvent(int ID)
        {

            try
            {
                Event event_to_copy = db.Events.Find(ID);
                int id_new = db.Events.Max(a => a.ID) + 1;
                Event event_to_create = new Event();
                event_to_create.ID = id_new;
                event_to_create.date_update = DateTime.Now;
                event_to_create.canceled = false;
                event_to_create.activity_fk = event_to_copy.activity_fk;
                event_to_create.category = event_to_copy.category;
                event_to_create.date = event_to_copy.date;
                event_to_create.time = event_to_copy.time;

                db.Events.Add(event_to_create);
                db.SaveChanges();
                return Ok(event_to_create);
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
        if (event_to_cancel.Activity.direction == "OUT")
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

