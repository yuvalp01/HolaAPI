using HolaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace HolaAPI.Controllers
{
    public class AgenciesController : ApiController
    {

        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        //GET all
        [ResponseType(typeof(List<AgencyDTO>))]
        public IHttpActionResult GetAgencies()
        {
            List<AgencyDTO> agencies;

            try
            {

                agencies = db.Agencies.OrderBy(a => a.name)
                .Select(a => new AgencyDTO { ID = a.ID, name = a.name, address = a.address }).ToList<AgencyDTO>();
                return Ok(agencies);


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    return Content(HttpStatusCode.BadRequest, "yuv: " + ex.InnerException.Message);
                }
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        ////GET all
        //public IQueryable<AgencyDTO> GetAgencies()
        //{
        //    return db.Agencies.OrderBy(a => a.name).Select(a => new AgencyDTO { ID = a.ID, name = a.name, address = a.address });
        //}


        [Route("api/agencies/{id}")]
        public AgencyDTO GetAgency(int id)
        {
            AgencyDTO agency = db.Agencies.Where(a => a.ID == id).Select(a => new AgencyDTO { ID = a.ID, name = a.name, address = a.address }).FirstOrDefault();

            return agency;

        }



        //POST new
        [ResponseType(typeof(Agency))]
        public IHttpActionResult Post([FromBody]Agency agency)
        {
            try
            {
                agency.ID = db.Agencies.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1;
                db.Agencies.Add(agency);
                db.SaveChanges();
                return Ok(agency);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }


        }



        // DELETE: api//5
        [ResponseType(typeof(Agency))]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                Agency agency = db.Agencies.Find(id);
                if (agency == null)
                {
                    return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", id));
                }

                db.Agencies.Remove(agency);
                db.SaveChanges();
                return Ok(agency);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);

            }
        }


        // GET: api/Agencies/5
        public string Get(int id)
        {
            return "value";
        }



        // PUT: api/Agencies/5
        public void Put(int id, [FromBody]string value)
        {
        }









        //private bool ProductExists(int id)
        //{
        //    return db.Agencies.Count(e => e.ID == id) > 0;
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
namespace HolaAPI.Models
{
    public class AgencyDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
    }

}





// GET: api/Agencies
//public IQueryable<Agency> Get()
//public dynamic Get()
//{

//    return db.Agencies.Select(a => new { a.ID, a.name, a.address });
//}





//// POST: api/Agencies
//public HttpResponseMessage Delete(int id)
//{
//    try
//    {
//        var agency_to_delete= db.Agencies.Where(a=>a.ID== id).FirstOrDefault();
//        db.Agencies.Remove(agency_to_delete);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK);
//    }
//    catch (Exception ex)
//    {
//        //var message = string.Format("Product with id = {0} not found", id);
//        HttpError err = new HttpError(":(((("+ex.Message);
//        return Request.CreateResponse(HttpStatusCode.BadRequest, err);

//    }

//}




//// POST: api/Agencies
//public HttpResponseMessage Post([FromBody]Agency agency)
//{
//    try
//    {
//        agency.ID = db.Agencies.OrderByDescending(a => a.ID).FirstOrDefault().ID+1;
//        db.Agencies.Add(agency);
//        db.SaveChanges();
//        return Request.CreateResponse(HttpStatusCode.OK);
//    }
//    catch (Exception ex)
//    {
//        var msg = Request.CreateResponse(HttpStatusCode.BadRequest);
//        return msg;
//    }

//}



//public IHttpActionResult Get(int id)
//{
//    Product product = _repository.Get(id);
//    if (product == null)
//    {
//        return NotFound(); // Returns a NotFoundResult
//    }
//    return Ok(product);  // Returns an OkNegotiatedContentResult
//}

//public HttpResponseMessage Get()
//{

//    var x  =  db.Agencies.Select(a => new { a.ID, a.name, a.address });
//    // Write the list to the response body.
//    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, x);
//    return response;
//}


