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
using System.Net.Http.Headers;

namespace HolaAPI.Controllers
{
    public class HotelsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        [ResponseType(typeof(List<HotelDTO>))]
        public IHttpActionResult GetHotels()
        {

            try
            {
                var hotels = db.Hotels.Select(a => new HotelDTO
                {
                    ID = a.ID,
                    name = a.name,
                    address = a.address
                });
                // Create a Cache-Control header for the response
                //var cacheControlHeader = new CacheControlHeaderValue();
                //cacheControlHeader.Private = true;
                //cacheControlHeader.MaxAge = new TimeSpan(0, 10, 0);
                //HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(HttpStatusCode.OK, hotels.ToList());
                //httpResponseMessage.Headers.CacheControl = cacheControlHeader;

                return Ok(hotels.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }


            //    // Find the matching order
            //    Order order = ...;
            //...


            //// Return a response message containing the order and the cache control header
            //OkResultWithCaching<Order> response = new OkResultWithCaching<Order>(order, this)
            //{
            //    CacheControlHeader = cacheControlHeader
            //};
            //    return response;



        }


        [ResponseType(typeof(Hotel))]
        public IHttpActionResult Post([FromBody]Hotel hotel)
        {
            try
            {
                hotel.ID = db.Hotels.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1;
                db.Hotels.Add(hotel);
                db.SaveChanges();
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Hotel))]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Hotel hotel = db.Hotels.Find(id);
                if (hotel == null)
                {
                    return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", id));
                }

                db.Hotels.Remove(hotel);
                db.SaveChanges();
                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);

            }
        }




        // PUT: api/Hotels/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHotel(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotel.ID)
            {
                return BadRequest();
            }

            db.Entry(hotel).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
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
    public class HotelDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
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