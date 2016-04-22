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
    public class GuidesController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        public IQueryable<GuideDTO> Get()
        {

            return db.Guides.Select(a => new GuideDTO { ID = a.ID, name = a.name, phone = a.phone });
        }


        [ResponseType(typeof(Guide))]
        public IHttpActionResult Post([FromBody]Guide guide)
        {
            try
            {
                guide.ID = db.Guides.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1;
                db.Guides.Add(guide);
                db.SaveChanges();
                return Ok(guide);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }


        }

        // DELETE: api/Products/5
        [ResponseType(typeof(Guide))]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Guide guide = db.Guides.Find(id);
                if (guide == null)
                {
                    return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", id));
                }

                db.Guides.Remove(guide);
                db.SaveChanges();
                return Ok(guide);
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

        private bool GuideExists(int id)
        {
            return db.Guides.Count(e => e.ID == id) > 0;
        }
    }
}

namespace HolaAPI.Models
{
    public class GuideDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
    }

}



