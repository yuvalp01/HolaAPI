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


        public IQueryable<ClientDTO> GetClients()
        {

            return db.Clients.Select(a => new ClientDTO
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
        }


        [ResponseType(typeof(Client))]
        public IHttpActionResult Post([FromBody]Client client)
        {
            try
            {
                client.date_update = DateTime.Now;
                client.deleted = false;
                client.canceled = false;
                db.Clients.Add(client);
                //extract basic service type (bus/one way bus)
                decimal _price;
                int _product_fk;
                if (!client.oneway) { _product_fk = 1; }
                else { _product_fk = 2; }
                _price = db.Products.FirstOrDefault(a => a.ID == _product_fk).rate;

                //decimal bus_price = db.Products.FirstOrDefault(a => a.ID == 1).rate;
                Sale sale = new Sale()
                {
                    PNR = client.PNR,
                    product_fk = _product_fk,
                    price = _price,
                    persons = client.PAX,
                    sale_type = "BIZ",
                    date_update = DateTime.Now,
                    date_sale = DateTime.Today,
                    deleted = false,
                    canceled = false
                
                     
                };
                db.Sales.Add(sale);
                db.SaveChanges();
                return Ok(client);
            }


            catch (DuplicateNameException ex)
            {
                return Content(HttpStatusCode.NotAcceptable, ex.Message);

            }

            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                SqlException sqleX = rootEx as SqlException;
                if (sqleX!=null)
                {
                    if (sqleX.Number==2627)
                    {
                        string message = string.Format("The PNR <b>{0}</b> already exists in the system.", client.PNR); 
                        return Content(HttpStatusCode.Conflict, message);
                    }
                }
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }


        }

        //// DELETE: api/Clients/5
        //[ResponseType(typeof(Client))]
        //public IHttpActionResult Delete(int PNR)
        //{
        //    try
        //    {
        //        Client client = db.Clients.Find(PNR);
        //        if (client == null)
        //        {
        //            return Content(HttpStatusCode.NotFound, string.Format("ID '{0}' does not exist in the table.", PNR));
        //        }

        //        db.Clients.Remove(client);
        //        db.SaveChanges();
        //        return Ok(client);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(HttpStatusCode.BadRequest, ex.Message);

        //    }
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
    public class ClientDTO
    {

        public string PNR { get; set; }
        public string names { get; set; }
        public int PAX { get; set; }
        public string num_arr { get; set; }
        public System.DateTime date_arr { get; set; }
        public string num_dep { get; set; }
        public Nullable<System.DateTime> date_dep { get; set; }
        public string phone { get; set; }
        public int hotel_fk { get; set; }
        public Nullable<int> agency_fk { get; set; }
        public Nullable<bool> oneway { get; set; }
        public Nullable<bool> canceled { get; set; }
        public string comments { get; set; }
        public System.DateTime date_update { get; set; }
        public string depart_list { get; set; }
        public string arrival_list_fk { get; set; }
    }

}

