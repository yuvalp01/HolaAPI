using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Tracing;

namespace HolaAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
           // config.EnableSystemDiagnosticsTracing();
            //config.Services.Replace(typeof(ITraceWriter), new MyTraceWriter());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
              name: "ActionApi",
              routeTemplate: "api/{controller}/{action}/{direction}/{date}",
              defaults: new { date = RouteParameter.Optional }
              );


            config.Routes.MapHttpRoute(
              name: "ActionApi_Dep",
              routeTemplate: "api/{controller}/{action}/{depart_list}",
              defaults: new { depart_list = RouteParameter.Optional }
              );

            //config.Routes.MapHttpRoute(
            //  name: "ActionApi",
            //  routeTemplate: "api/{controller}/{action}/{direction}",
            //  defaults: new { direction = RouteParameter.Optional }
            //  );
            //          config.Routes.MapHttpRoute(
            //name: "ActionApi",
            //routeTemplate: "api/{controller}/{action}/{date}/{direction}",
            //defaults: new { date = RouteParameter.Optional }
            //);



            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
