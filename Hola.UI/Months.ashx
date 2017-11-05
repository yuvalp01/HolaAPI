<%@ WebHandler Language="C#" Class="Months" %>

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;

public class Months : IHttpHandler
{
    public class MonthsClass
    {
        public int ID { get; set; }
        public string name { get; set; }
    }
    public void ProcessRequest(HttpContext context)
    {
        //context.Response.ContentType = "text/plain";
        context.Response.ContentType = "application/json";

        List<string> monthNames = DateTimeFormatInfo.CurrentInfo.MonthNames.Take(12).ToList();


        var _monthSelectList = monthNames.Select(
           m => new MonthsClass { ID = monthNames.IndexOf(m) + 1, name = m });
        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
        var json = jsonSer.Serialize(_monthSelectList);

        context.Response.Write(json);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}