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
    public class ActivitiesController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();



        [HttpGet]
        [Route("api/activities/{category}/{direction?}")]
        [ResponseType(typeof(IQueryable<ActivityDTO>))]
        public IHttpActionResult Get(string category,string direction=null)
        {
            try
            {

                var activities = db.Activities.Select(a => new ActivityDTO
                {
                    ID = a.ID,
                    name = a.name,
                    category = a.category,
                    direction = a.direction
                }).Where(a=>a.category==category);
                if (direction!=null)
                {
                    activities =   activities.Where(a => a.direction == direction);
                }
                return Ok(activities);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
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
