using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;





/// <summary>
/// Summary description for JsonData
/// </summary>
public class DataHelper
{
    public DataHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }




    public static List<FlightDetails> boxFlightInfo(string flights_str)
    {

        string[] flights_array = flights_str.Split('~');
        List<FlightDetails> flights = new List<FlightDetails>();

        FlightDetails flight1 = new FlightDetails();
        string[] flight_arr_1 = flights_array[0].Split('_');
        flight1.num = flight_arr_1[0];
        flight1.date = Convert.ToDateTime(flight_arr_1[1]);
        flights.Add(flight1);

        if (flights_array.Length > 1)
        {
            FlightDetails flight2 = new FlightDetails();
            string[] flight_arr_2 = flights_array[1].Split('_');
            flight2.num = flight_arr_2[0];
            flight2.date = Convert.ToDateTime(flight_arr_2[1]);
            flights.Add(flight2);
        }

        return flights;


    }



}


























//public string getNewDepartLists(string date_dep_start_str, string flights_str)
//{
//    //DateTime date_dep_start = Convert.ToDateTime(date_dep_start_str);
//    //string[] flights_array = flights_str.Split('~');
//    //List<FlightDetails> flights = boxFlightInfo(flights_str);
//    //string depart_list = date_dep_start_str + "_" + flights[0].num;
//    //depart_list = flights.Count > 1 ? depart_list + "_" + flights[1].num : depart_list;
//    string depart_list = get_depart_list( date_dep_start_str,  flights_str);
//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {

//        //Clear past lists:
//        db.Depart_Plan.RemoveRange(db.Depart_Plan.Where(a => a.depart_list.StartsWith(date_dep_start_str)));
//        db.FITs.Where(a => a.depart_list.StartsWith(date_dep_start_str)).ToList().ForEach(a => a.depart_list = "");
//        db.SaveChanges();
//        //Insert and update new:
//        insertUpdateDepartPlan(date_dep_start, depart_list, flights);


//        return getDepartPlan(depart_list);
//    }

//}







//public string getDepartureList(string depart_list)
//{

//    try
//    {
//        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//        {
//            var query = from a in db.FITs
//                        join c in db.Depart_Plan on a.hotel_fk equals c.hotel_fk
//                        join b in db.Hotels on a.hotel_fk equals b.ID
//                        orderby a.hotel_fk
//                        where a.oneway == false && a.depart_list == depart_list
//                        select new { title = "[" + a.PNR + "] " + a.names, a.PNR, a.names, a.phone, a.PAX, hotel = b.name + ": " + c.time.Value.Hours + ":" + c.time.Value.Minutes };
//            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
//            return jsonSer.Serialize(query);
//        }
//    }
//    catch (Exception ex)
//    {
//        return "Data Error: " + ex.Message;
//    }



//}



//public string getFlights2Days_Arr(DateTime date_arr)
//{

//    DateTime date_next = date_arr.AddDays(1);
//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {
//        var query = from a in db.FITs
//                    group a by new { a.num_arr, a.date_arr } into g
//                    join b in db.Flights on new { num = g.Key.num_arr, date = (DateTime)g.Key.date_arr } equals new { b.num, date = b.date }
//                    //where b.direction == "IN" && b.date >= date_arr && b.date <= date_next
//                    select new { num = g.Key.num_arr, date = g.Key.date_arr, b.time, sum = g.Sum(s => s.PAX) };
//        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
//        return jsonSer.Serialize(query);

//    }
//}

//public string getFlights2Days_Dep(DateTime date_dep)
//{

//    DateTime date_next = date_dep.AddDays(1);
//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {

//        var query = from a in db.FITs
//                    group a by new { a.num_dep, a.date_dep } into g
//                    join b in db.Flights on new { num = g.Key.num_dep, date = (DateTime)g.Key.date_dep } equals new { b.num, date = b.date }
//                    where b.direction == "OUT" && b.date >= date_dep && b.date <= date_next
//                    select new { num = g.Key.num_dep, date = g.Key.date_dep, b.time, sum = g.Sum(s => s.PAX) };
//        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
//        return jsonSer.Serialize(query);

//    }
//}




//public string insertUpdateListDeparture(string date_dep_start_str, string flights_str)
//{
//    DateTime date_dep_start = Convert.ToDateTime(date_dep_start_str);
//    string[] flights_array = flights_str.Split('~');
//    List<FlightDetails> flights = boxFlightInfo(flights_str);
//    string depart_list = date_dep_start_str + "_" + flights[0].num;
//    depart_list = flights.Count > 1 ? depart_list + "_" + flights[1].num : depart_list;



//    try
//    {
//        if (insertUpdateDepartPlan(date_dep_start, depart_list, flights) > 0)
//        {
//            return getDepartPlan(depart_list);
//        }
//        else
//        {
//            return "Error when insert/update depart_list";
//        }
//    }
//    catch (Exception ex)
//    {
//        //if (ex.InnerException !=null)
//        //{
//        //    //SqlException sqlEx = ex as SqlException;
//        //    //if ((sqlEx!=null))
//        //    //{
//        //    //    if (sqlEx.ErrorCode==1)
//        //    //    {
//        //    //        return sqlEx.Message;
//        //    //    }
//        ////    //}
//        //if (ex.Message.Contains("UniqueConstraint"))
//        //{

//        //}



//            //String innerMessage = (ex.InnerException != null) ? ex.InnerException.Message : "";

//        return ex.Message;
//    }

//}


//var query = from a in db.Sales
//            group a by new { a.PNR, a.product_fk, a.sale_type, } into g
//            join b in db.FITs on g.Key.PNR equals b.PNR
//            join c in db.Hotels on b.hotel_fk equals c.ID
//            join d in db.Products on g.Key.product_fk equals d.ID
//            where b.date_arr == date_start
//            select new { b.PNR, b.names, hotel = c.name, b.phone, b.PAX, tour = d.name, sum = g.Sum(s => s.p) };




//private int ___updateArrivalList(DateTime date_dep_start, string depart_list, List<FlightDetails> flights)
//{
//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {
//        DateTime date1 = flights[0].date;
//        string num1 = flights[0].num;

//        var _fits = db.FITs.Select(a=>a);
//        //var _fits = from a in db.FITs
//        //            select a;

//        if (flights.Count == 1)
//        {

//            _fits = _fits.Where(a => a.date_arr == date1 && a.num_arr == num1);

//            //_fits = from a in _fits
//            //        where (a.date_arr == date1 && a.num_arr == num1)
//            //        select a;
//        }
//        else
//        {
//            DateTime date2 = flights[1].date;
//            string num2 = flights[1].num;
//            _fits = _fits.Where(a => (a.date_arr == date1 && a.num_arr == num1) || (a.date_arr == date2 && a.num_arr == num2));
//            //_fits = from a in db.FITs
//            //        where (a.date_arr == date1 && a.num_arr == num1) ||
//            //              (a.date_arr == date2 && a.num_arr == num2)
//            //        select a;
//        }



//        if (_fits != null)
//        {
//            foreach (var item in _fits)
//            {
//                item.depart_list = depart_list;
//            }
//        }

//        return db.SaveChanges();
//    }
//}

//public string getFlights(DateTime date, string direction)
//{

//    DateTime date_depart = date;
//    DateTime date_next = date_depart.AddDays(1);
//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {
//        var query = from a in db.FITs
//                    group a by new { a.num_dep, a.date_dep } into g
//                    join b in db.Flights on new { num = g.Key.num_dep, date = (DateTime)g.Key.date_dep } equals new { b.num, date = b.date }
//                    where b.direction == direction && b.date >= date_depart && b.date <= date_next
//                    select new { num = g.Key.num_dep, date = g.Key.date_dep, b.time, sum = g.Sum(s => s.PAX) };
//        JavaScriptSerializer jsonSer = new JavaScriptSerializer();
//        return jsonSer.Serialize(query);

//    }
//}



//private int _insertUpdateDepartPlan(string date_str, string flights)
//{
//    //if (context.Request["date_dep"] != null && context.Request["flights"] != null)

//    DateTime date = Convert.ToDateTime(date_str);
//    string[] flights_array = flights.Split('~');

//    string[] flight_1 = flights_array[0].Split('_');
//    string num1 = flight_1[0];
//    DateTime date1 = Convert.ToDateTime(flight_1[1]);

//    string[] flight_2 = flights_array[1].Split('_');
//    string num2 = flight_2[0];
//    DateTime date2 = Convert.ToDateTime(flight_2[1]);
//    string depart_list = date_str + "_" + num1 + "_" + num2;

//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {
//        var _fits = from a in db.FITs
//                    where (a.date_dep.Value == date1 && a.num_dep == num1) || (a.date_dep.Value == date2 && a.num_dep == num2)
//                    select a;
//        if (_fits != null)
//        {
//            foreach (var item in _fits)
//            {
//                item.depart_list = depart_list;
//            }
//        }
//        List<FIT> fits = _fits.ToList();
//        var query = from a in fits
//                    group a by new { a.hotel_fk, a.depart_list } into g
//                    //join b in db.Hotels on g.Key.hotel_fk equals b.ID
//                    where g.Key.depart_list == depart_list
//                    select new Depart_Plan
//                    {
//                        date_dep_start = date,
//                        depart_list = depart_list,
//                        hotel_fk = (int)g.Key.hotel_fk
//                    };

//        db.Depart_Plan.AddRange(query);
//        return db.SaveChanges();

//    }

//}



//public int UpdateAllDep(string date, string flights)
//{
//    //string date_dep_str = context.Request["date_dep"];
//    DateTime date_dep_start = Convert.ToDateTime(date);
//    string[] flights_arr = flights.Split('~');

//    string[] flight_1 = flights_arr[0].Split('_');
//    string num1 = flight_1[0];
//    DateTime date1 = Convert.ToDateTime(flight_1[1]);

//    string[] flight_2 = flights_arr[1].Split('_');
//    string num2 = flight_2[0];
//    DateTime date2 = Convert.ToDateTime(flight_2[1]);
//    string depart_list = date + "_" + num1 + "_" + num2;


//    using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//    {
//        var _fits = from a in db.FITs
//                    where (a.date_dep.Value == date1 && a.num_dep == num1) || (a.date_dep.Value == date2 && a.num_dep == num2)
//                    select a;
//        if (_fits != null)
//        {
//            foreach (var item in _fits)
//            {
//                item.depart_list = depart_list;
//            }
//        }
//        List<FIT> fits = _fits.ToList();
//        var query = from a in fits
//                    group a by new { a.hotel_fk, a.depart_list } into g
//                    //join b in db.Hotels on g.Key.hotel_fk equals b.ID
//                    where g.Key.depart_list == depart_list
//                    select new Depart_Plan
//                    {
//                        date_dep_start = date_dep_start,
//                        depart_list = depart_list,
//                        hotel_fk = (int)g.Key.hotel_fk
//                    };

//        db.Depart_Plan.AddRange(query);
//        return db.SaveChanges();
//    }
//}







//public string _getDepartureList(HttpContext context)
//{

//    try
//    {
//        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
//        {
//            var query = from a in db.FITs
//                        join b in db.Hotels on a.hotel_fk equals b.ID
//                        where a.oneway == false && a.date_dep == new DateTime(2016, 4, 1)
//                        select new { title = "[" + a.PNR + "] " + a.names, a.PNR, a.names, a.phone, a.PAX, hotel = b.name };
//            JavaScriptSerializer jsonSer = new JavaScriptSerializer();
//            return jsonSer.Serialize(query);
//        }
//    }
//    catch (Exception ex)
//    {
//        return "Data Error: " + ex.Message;
//    }

//}