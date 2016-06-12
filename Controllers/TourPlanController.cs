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
    public class TourPlanController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        [ResponseType(typeof(List<TourPlanDTO>))]
        public IHttpActionResult GetTourPlan()
        {
            try
            {

                var tour_plans = from a in db.TourPlans
                                 join b in db.Products on a.product_fk equals b.ID
                                 join c in db.Guides on a.guide_fk equals c.ID
                                 select new TourPlanDTO
                                 {
                                     ID = a.ID,
                                     date = a.date,
                                     time = a.time.Value,//.Hours + ":" + a.time.Value.Minutes,
                                     product_fk = a.product_fk,
                                     tour_name = b.name,
                                     guide_fk = a.guide_fk,
                                     guide_name = c.name,
                                     comments = a.comments,
                                     date_update = a.date_update
                                 };
                return Ok(tour_plans.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }


        [ResponseType(typeof(TourPlan))]
        public IHttpActionResult Post([FromBody]TourPlan tour)
        {
            try
            {
                tour.ID = db.TourPlans.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1;
                tour.date_update = DateTime.Now;
                db.TourPlans.Add(tour);
                db.SaveChanges();
                return Ok(tour);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }


        }



        [ResponseType(typeof(TourPlan))]
        [HttpPut]
        [Route("api/tourplan/CancelPlan/{ID}")]
        public IHttpActionResult CancelClient(int ID)
        {
            try
            {
                var plan_to_cancel = db.TourPlans.SingleOrDefault(a => a.ID == ID && a.canceled == false);
                plan_to_cancel.canceled = true;
                plan_to_cancel.date_update = DateTime.Now;

                db.SaveChanges();
                return Ok(plan_to_cancel);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }


        [ResponseType(typeof(TourPlan))]
        [HttpPut]
        [ActionName("UpdatePlan")]

        public IHttpActionResult UpdatePlan([FromBody] TourPlanDTO plan)
        {
            try
            {
                var plan_to_update = db.TourPlans.SingleOrDefault(a => a.ID == plan.ID && a.canceled == false);
                plan_to_update.time = plan.time;
                plan_to_update.guide_fk = plan.guide_fk;
                plan_to_update.comments = plan.comments;
                plan_to_update.date_update = DateTime.Now;
                db.SaveChanges();
                return Ok(plan_to_update);
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

        private bool HotelExists(int id)
        {
            return db.Hotels.Count(e => e.ID == id) > 0;
        }
    }
}

namespace HolaAPI.Models
{
    public class TourPlanDTO
    {
        public int ID { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }
        public Nullable<int> product_fk { get; set; }
        public string tour_name { get; set; }
        public Nullable<int> guide_fk { get; set; }
        public string guide_name { get; set; }
        public string comments { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
    }

}


//// DELETE: api/Products/5
//[ResponseType(typeof(TourPlan))]
//public IHttpActionResult Delete(int id)
//{
//    try
//    {
//        TourPlan tour = db.TourPlans.Find(id);
//        if (tour == null)
//        {
//            return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", id));
//        }

//        db.TourPlans.Remove(tour);
//        db.SaveChanges();
//        return Ok(tour);
//    }
//    catch (Exception ex)
//    {
//        return Content(HttpStatusCode.BadRequest, ex.Message);

//    }
//}


//// POST: api/Hotels
//[ResponseType(typeof(Hotel))]
//public async Task<IHttpActionResult> PostHotel(Hotel hotel)
//{
//    if (!ModelState.IsValid)
//    {
//        return BadRequest(ModelState);
//    }

//    db.Hotels.Add(hotel);

//    try
//    {
//        await db.SaveChangesAsync();
//    }
//    catch (DbUpdateException)
//    {
//        if (HotelExists(hotel.ID))
//        {
//            return Conflict();
//        }
//        else
//        {
//            throw;
//        }
//    }

//    return CreatedAtRoute("DefaultApi", new { id = hotel.ID }, hotel);
//}