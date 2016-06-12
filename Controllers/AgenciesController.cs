using HolaAPI.Models;
using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Tracing;
using System.Diagnostics.Tracing;
using System.IO;

namespace HolaAPI.Controllers
{
    public class AgenciesController : ApiController
    {

        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        //GET all
        [ResponseType(typeof(List<AgencyDTO>))]
        public IHttpActionResult GetAgencies()
        {
            try
            {

                //SystemDiagnosticsTraceWriter traceWriter = Configuration.Services.GetTraceWriter();
                //traceWriter.Trace(Request, "My Category", TraceLevel.Info, "{0}", "This is a test trace message.");


                //ITraceWriter traceWriter = Configuration.Services.GetTraceWriter();
                //traceWriter.Trace(Request, "My Category", TraceLevel.Error, "{0}", "This is a test trace message.");

                var agencies = db.Agencies.OrderBy(a => a.name)
                  .Select(a => new AgencyDTO { ID = a.ID, name = a.name, address = a.address }).ToList<AgencyDTO>();
                return Ok(agencies);


            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                Logger.Write(rootEx.Message +  "Stack Trace: "+ rootEx.StackTrace);
                return Content( HttpStatusCode.InternalServerError, rootEx.Message);
            }
        }

        [Route("api/agencies/{id}")]
        [ResponseType(typeof(AgencyDTO))]
        public IHttpActionResult GetAgency(int id)
        {
            try
            {

                var agency = db.Agencies.Where(a => a.ID == id).Select(a => new AgencyDTO
                {
                    ID = a.ID, name = a.name, address = a.address
                }).SingleOrDefault();
                return Ok( agency);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }
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
namespace HolaAPI.Models
{
    public class AgencyDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
    }

}



//// DELETE: api//5
//[ResponseType(typeof(Agency))]
//public IHttpActionResult Delete(int id)
//{
//    try
//    {
//        Agency agency = db.Agencies.Find(id);
//        if (agency == null)
//        {
//            return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", id));
//        }

//        db.Agencies.Remove(agency);
//        db.SaveChanges();
//        return Ok(agency);
//    }
//    catch (Exception ex)
//    {
//        return Content(HttpStatusCode.BadRequest, ex.Message);

//    }
//}
