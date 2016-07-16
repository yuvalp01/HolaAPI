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
    public class ClientsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();

        [ResponseType(typeof(IQueryable<ClientDTO>))]
        [HttpGet]
        public IHttpActionResult GetClients()
        {
            try
            {

                var clients = from a in db.Clients
                              join b in db.Hotels on a.hotel_fk equals b.ID
                              join c in db.Agencies on a.agency_fk equals c.ID
                              where a.canceled == false
                              select new ClientDTO
                              {
                                  PNR = a.PNR,
                                  names = a.names,
                                  PAX = a.PAX,
                                  num_arr = a.num_arr,
                                  date_arr = a.date_arr,
                                  num_dep = a.num_dep,
                                  date_dep = a.date_dep,
                                  phone = a.phone,
                                  hotel_fk = a.hotel_fk,
                                  hotel_name = b.name,
                                  agency_fk = a.agency_fk,
                                  agency_name = c.name,
                                  oneway = a.oneway,
                                  comments = a.comments
                              };
                return Ok(clients);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

        }



        [ResponseType(typeof(ClientDTO))]
        [HttpGet]
        [Route("api/clients/GetClient/{agency_fk}/{PNR}")]
        public IHttpActionResult GetClient(int agency_fk, string PNR)
        {
            try
            {
                var client = db.Clients.Where(a => a.agency_fk == agency_fk && a.PNR == PNR && a.canceled == false).Select(a => new ClientDTO
                {
                    PNR = a.PNR,
                    names = a.names,
                    PAX = a.PAX,
                    num_arr = a.num_arr,
                    date_arr = a.date_arr,
                    num_dep = a.num_dep,
                    date_dep = a.date_dep,
                    phone = a.phone,
                    hotel_fk = a.hotel_fk,
                    agency_fk = a.agency_fk,
                    oneway = a.oneway,
                    comments = a.comments
                });
                return Ok(client);
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }

        }



        [ResponseType(typeof(Client))]
        [HttpPut]
        [Route("api/clients/CancelClient/{agency_fk}/{PNR}")]
        public IHttpActionResult CancelClient(int agency_fk, string PNR)
        {
            try
            {
                var client_to_cancel = db.Clients.SingleOrDefault(a => a.PNR == PNR && a.agency_fk == agency_fk && a.canceled == false);
                client_to_cancel.canceled = true;
                client_to_cancel.date_update = DateTime.Now;
                IQueryable<Sale> sales_to_cancel = db.Sales.Where(x => x.PNR == PNR && x.agency_fk==agency_fk);
                foreach (Sale sale in sales_to_cancel)
                {
                    sale.canceled = true;
                    sale.date_update = DateTime.Now;
                    sale.comments = "***Reservation canceled*** " + sale.comments;
                }

                db.SaveChanges();
                return Ok("{}");
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.BadRequest, rootEx.Message);
            }
        }





        [ResponseType(typeof(Client))]
        [HttpPut]
        [Route("api/clients/UpdateClient/{agency_fk}/{PNR}")]
        public IHttpActionResult UpdateClient(int agency_fk, string PNR, [FromBody] ClientDTO client)
        {
            if (client.PAX == 0) throw new Exception("PAX cannot be zero");
            try
            {
                var client_to_update = db.Clients.SingleOrDefault(a => a.agency_fk == agency_fk && a.PNR == PNR && a.canceled == false);
                client_to_update.names = client.names;
                client_to_update.PAX = client.PAX;
                client_to_update.num_arr = client.num_arr;
                client_to_update.date_arr = client.date_arr;
                client_to_update.num_dep = client.num_dep;
                client_to_update.date_dep = client.date_dep;
                client_to_update.phone = client.phone;
                client_to_update.hotel_fk = client.hotel_fk;
                client_to_update.oneway = client.oneway;
                client_to_update.comments = client.comments;
                client_to_update.date_update = DateTime.Now;

                IQueryable<Sale> sales_to_update = db.Sales.Where(x => x.PNR == client.PNR);
                foreach (Sale sale in sales_to_update)
                {
                    //In case of PAX change we have to change also the number of the people in each sale
                    int persons_gap; int persons_old; int persons_new;
                    if (sale.canceled == false && sale.persons != client.PAX)
                    {
                        persons_old = sale.persons;
                        persons_gap = client.PAX - persons_old;
                        persons_new = sale.persons + persons_gap;
                        //make sure no negative number
                        sale.persons = persons_new < 0 ? 0 : persons_new;
                        sale.date_update = DateTime.Now;
                        sale.comments = String.Format("***System update: PAX has changed therefore the number of people has changed in {0} (from {1} to {2})*** {3}", persons_gap, persons_old, sale.persons, sale.comments);
                    }
                }

                db.SaveChanges();
                return Ok("{}");
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


    }
}











//[ResponseType(typeof(Client))]
//public IHttpActionResult Post([FromBody]Client client)
//{
//    try
//    {

//        client.date_update = DateTime.Now;
//        client.canceled = false;
//        db.Clients.Add(client);
//        //extract transportation type from products table. It could be either 'bus' 'one way bus'
//        int _product_fk;
//        if (!client.oneway) { _product_fk = 1; }
//        else { _product_fk = 2; }
//        //all agencies except for Hola Shalom (100) are External. External clients already paid to the agency
//        string _sale_type = "External";
//        decimal _remained_pay = 0;
//        if (client.agency_fk==100)
//        {
//            _sale_type = "Internal";
//            _remained_pay = db.Products.SingleOrDefault(a => a.ID == _product_fk).rate;

//        }

//        Sale sale = new Sale()
//        {
//            PNR = client.PNR,
//            agency_fk = client.agency_fk,
//            product_fk = _product_fk,
//            remained_pay = _remained_pay,
//            persons = client.PAX,
//            sale_type = _sale_type,
//            date_update = DateTime.Now,
//            date_sale = DateTime.Today,
//            canceled = false
//        };
//        db.Sales.Add(sale);
//        db.SaveChanges();
//        return Ok(client);
//    }

//    catch (Exception ex)
//    {
//        Exception rootEx = ex.GetBaseException();
//        SqlException sqleX = rootEx as SqlException;
//        if (sqleX != null)
//        {
//            if (sqleX.Number == 2627)
//            {
//                string message = string.Format("The PNR <b>{0} (agency ID:{1})</b> already exists in the system.", client.PNR, client.agency_fk);
//                return Content(HttpStatusCode.Conflict, message);
//            }
//        }
//        return Content(HttpStatusCode.InternalServerError, rootEx.Message);
//    }


//}
