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
                          join b in db.Products on a.tour_fk equals b.ID
                          join c in db.Guides on a.guide_fk equals c.ID
                          select new TourPlanDTO
                          {
                              ID = a.ID,
                              date = a.date,
                              time = a.time.Value.Hours + ":" + a.time.Value.Minutes,
                              tour_fk = a.tour_fk,
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
                return Content(HttpStatusCode.BadRequest, ex.Message);
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
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        // DELETE: api/Products/5
        [ResponseType(typeof(TourPlan))]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                TourPlan tour = db.TourPlans.Find(id);
                if (tour == null)
                {
                    return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", id));
                }

                db.TourPlans.Remove(tour);
                db.SaveChanges();
                return Ok(tour);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);

            }
        }




        //// PUT: api/Hotels/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutHotel(int id, Hotel hotel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != hotel.ID)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(hotel).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!HotelExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}




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
        public string time { get; set; }
        public Nullable<int> tour_fk { get; set; }
        public string tour_name { get; set; }
        public Nullable<int> guide_fk { get; set; }
        public string guide_name { get; set; }
        public string comments { get; set; }
        public Nullable<System.DateTime> date_update { get; set; }
    }

}





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