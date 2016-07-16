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
using System.Data.SqlClient;

namespace HolaAPI.Controllers
{
    public class ReservationsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        [ResponseType(typeof(ReservationDTO))]
        [ActionName("PostBasicReservation")]
        public IHttpActionResult PostBasicReservation([FromBody]ReservationDTO reservation)
        {
            if (reservation.PAX == 0) throw new Exception("PAX cannot be zero");
            Client client = new Client();
            try
            {
                //Create client object

                client.PNR = reservation.PNR;
                client.agency_fk = reservation.agency_fk;
                client.comments = reservation.comments;
                client.date_arr = reservation.date_arr;
                client.date_dep = reservation.date_dep;
                client.hotel_fk = reservation.hotel_fk;
                client.names = reservation.names;
                client.num_arr = reservation.num_arr;
                client.num_dep = reservation.num_dep;
                client.PAX = reservation.PAX;
                client.phone = reservation.phone;
                client.date_update = DateTime.Now;
                client.canceled = false;

                db.Clients.Add(client);
                //Create sale object:
                //all agencies except for Hola Shalom (100) are External. External clients already paid to the agency so remained_pay will always be 0
                string _sale_type = "Internal";
                if (client.agency_fk != 100)
                {
                    _sale_type = "External";
                    reservation.remained_pay = 0;
                }
                Sale sale = new Sale()
                {
                    PNR = reservation.PNR,
                    agency_fk = reservation.agency_fk,
                    product_fk = reservation.product_fk,
                    remained_pay = reservation.remained_pay,
                    persons = reservation.PAX,
                    sale_type = _sale_type,
                    date_sale = DateTime.Today,
                    date_update = DateTime.Now,
                    canceled = false
                };
                db.Sales.Add(sale);

                var activities = from a in db.Products
                                 join b in db.Rel_product_activity on a.ID equals b.product_fk
                                 join c in db.Activities on b.activity_fk equals c.ID
                                 where a.ID == reservation.product_fk
                                 select c;

                foreach (var activity in activities)
                {
                    SoldActivity sale_activity = new SoldActivity()
                    {
                        PNR = reservation.PNR,
                        agency_fk = reservation.agency_fk,
                        activity_fk = activity.ID,
                        Sale = sale,
                        event_fk  = 0,
                        date_update = DateTime.Now,
                        canceled = false,

                    };
                    db.SoldActivities.Add(sale_activity);
                };

                db.SaveChanges();
                return Ok(client);
            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                SqlException sqleX = rootEx as SqlException;
                if (sqleX != null)
                {
                    if (sqleX.Number == 2627)
                    {
                        string message = string.Format("The PNR <b>{0} (agency ID:{1})</b> already exists in the system.", client.PNR, client.agency_fk);
                        return Content(HttpStatusCode.Conflict, message);
                    }
                }

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













