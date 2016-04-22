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
                return Content(HttpStatusCode.InternalServerError, ex.Message);
            }


        }



        [ActionName("GetPlan")]
        [HttpGet]
        public IQueryable<DepartPlanDTO> GetPlan([FromUri]string depart_list)
        {

            return from a in db.DepartPlans
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

        }


        [ActionName("CreatePlan")]
        [HttpPost]
        public List<DepartPlanDTO> CreatePlan([FromUri] string date_dep_start, [FromUri] string flights)
        {

            DepartHelper helper = new DepartHelper(date_dep_start, flights);
            List<DepartPlanDTO> departPlan = helper.getNewDepartPlan();
            return departPlan;


        }

        [ActionName("UpdatePlan")]
        [HttpPut]
        public DepartPlanDTO UpdatePlan([FromBody] DepartPlanDTO line)
        {

            DepartPlan line_to_update = db.DepartPlans.Find(line.depart_list, line.hotel_fk);
            line_to_update.time = line.time;
            db.SaveChanges();
            return line;
        }





        // DELETE: api/Arrival/5
        public void Delete(int id)
        {
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

