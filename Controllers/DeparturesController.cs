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
    public class DeparturesController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();
        // 

        [ActionName("GetList")]
        [ResponseType(typeof(List<Departure>))]
        [HttpGet]
        public IHttpActionResult GetList([FromUri]string depart_list)
        {
            try
            {
                var departures = from a in db.Clients
                       join c in db.DepartPlans on a.hotel_fk equals c.hotel_fk
                       join b in db.Hotels on a.hotel_fk equals b.ID
                       orderby c.time
                       where a.oneway == false && a.depart_list == depart_list
                       let _hotel = b.name + " (Meeting Point: <b>" + (b.meeting_point) + "</b>)" + "<b style='color: black; float: right; margin:5px'>Pickup Time: " + c.time.Value.Hours + ":" + c.time.Value.Minutes + "</b>"
                       select new Departure
                       {
                           title = "[" + a.PNR + "] " + a.names,
                           PNR = a.PNR,
                           names = a.names,
                           phone = a.phone,
                           PAX = a.PAX,
                           hotel = _hotel

                       };
                return Ok(departures.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }


        }



        [ResponseType(typeof(IQueryable<DepartPlanDTO>))]
        [ActionName("GetPlan")]
        [HttpGet]
        public IHttpActionResult GetPlan([FromUri]string depart_list)
        {
            try
            {
                var plan =  from a in db.DepartPlans
                       where a.depart_list == depart_list
                       join b in db.Hotels on a.hotel_fk equals b.ID
                       orderby a.time
                       select new DepartPlanDTO
                       {
                           depart_list = a.depart_list,
                           hotel_fk = a.hotel_fk,
                           hotel = b.name,
                           time = a.time,
                           PAX = a.PAX
                       };
                return Ok(plan);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
                throw;
            }


        }


        [ResponseType(typeof(List<DepartPlanDTO>))]
        [ActionName("CreatePlan")]
        [HttpPost]
        public IHttpActionResult CreatePlan([FromBody] DateTime dateDepStart, [FromBody] List< Flight> flights)
        {
            try
            {

                DepartHelper helper = new DepartHelper(dateDepStart, flights);

                //Clear past lists:
                db.DepartPlans.RemoveRange(db.DepartPlans.Where(a => a.depart_list.StartsWith(dateDepStart)));
                db.Clients.Where(a => a.depart_list.StartsWith(DateDepStartStr)).ToList().ForEach(a => a.depart_list = "");
                db.SaveChanges();
                //Insert and update new:
                insertUpdateDepartPlan();

                return getDepartPlan();

                List<DepartPlanDTO> departPlan = helper.getNewDepartPlan();
                return Ok(departPlan);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
                throw;
            }
        }



        //[ResponseType (typeof( List<DepartPlanDTO>))]
        //[ActionName("CreatePlan")]
        //[HttpPost]
        //public IHttpActionResult  CreatePlan([FromUri] string date_dep_start, [FromUri] string flights)
        //{
        //    try
        //    {
        //        DepartHelper helper = new DepartHelper(date_dep_start, flights);
        //        List<DepartPlanDTO> departPlan = helper.getNewDepartPlan();
        //        return Ok(departPlan);
        //    }
        //    catch (Exception ex)
        //    {
        //        Exception rootEx = ex.GetBaseException();
        //        return Content(HttpStatusCode.InternalServerError, rootEx.Message);
        //        throw;
        //    }
        //}



        [ResponseType(typeof(DepartPlanDTO))]
        [ActionName("UpdatePlan")]
        [HttpPut]
        public IHttpActionResult UpdatePlan([FromBody] DepartPlanDTO line)
        {
            try
            {
                DepartPlan line_to_update = db.DepartPlans.Find(line.depart_list, line.hotel_fk);
                line_to_update.time = line.time;
                db.SaveChanges();
                return Ok( line);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
                throw;
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

