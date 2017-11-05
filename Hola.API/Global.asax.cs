using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace HolaAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.Formatters.Remove(GlobalConfiguration.Configuration.Formatters.XmlFormatter);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }


        //public override void Init()
        //{
        //    base.Init();
        //    this.AcquireRequestState += showRouteValues;
        //}

        //protected void showRouteValues(object sender, EventArgs e)
        //{
        //    var context = HttpContext.Current;
        //    if (context == null)
        //        return;
        //    var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));
        //}
    }
}
