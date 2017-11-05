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
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
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


