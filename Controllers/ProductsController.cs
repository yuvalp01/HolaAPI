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
    public class ProductsController : ApiController
    {
        private HolaShalomDBEntities db = new HolaShalomDBEntities();


        [ResponseType(typeof(List<ProductDTO>))]
        public IHttpActionResult GetProducts()
        {
            try
            {
                var products = db.Products.Select(a => new ProductDTO
                {
                    ID = a.ID,
                    name = a.name,
                    //code = a.code,
                    rate = a.rate,
                    category = a.category,
                    subcat = a.subcat
                });

                return Ok(products.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }
        }

        [ResponseType(typeof(List<ProductDTO>))]
        public IHttpActionResult GetProducts([FromUri]string types)
        {
            try
            {
                string[] types_array = types.Split(',');
                var products = from a in db.Products
                       where types_array.Contains(a.category)
                       select new ProductDTO
                       {
                           ID = a.ID,
                           name = a.name,
                           //code = a.code,
                           rate = a.rate,
                           category = a.category
                       };
                return Ok(products.ToList());
            }
            catch (Exception ex)
            {
                Exception rootEx = ex.GetBaseException();
                return Content(HttpStatusCode.InternalServerError, rootEx.Message);
            }

        }


        [ResponseType(typeof(Product))]
        public IHttpActionResult Post([FromBody]Product product)
        {
            try
            {
                product.ID = db.Products.OrderByDescending(a => a.ID).FirstOrDefault().ID + 1;
                db.Products.Add(product);
                db.SaveChanges();
                return Ok(product);
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


