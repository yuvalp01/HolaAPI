using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using HolaAPI.Models;

namespace HolaAPI.Models
{
    public class DepartPlanDTO
    {
        public string depart_list { get; set; }
        public string hotel { get; set; }
        public int hotel_fk { get; set; }
        public Nullable<System.TimeSpan> time { get; set; }
        public int PAX { get; set; }

    }
//    select new Departure  { title = "[" + a.PNR + "] " + a.names, a.PNR, a.names, a.phone, a.PAX, hotel = b.name + ": " + c.time.Value.Hours + ":" + c.time.Value.Minutes
//};

public class Departure
    {
        public string PNR { get; set; }
        public string names { get; set; }
        public string title { get; set; }
        public TimeSpan time { get; set; }
        public string hotel { get; set; }
        public string phone { get; set; }
        public int PAX { get; set; }
        public string meeting_point { get; set; }
        //public int MKF { get; set; }
        //public int VDN { get; set; }
        //public int FIG { get; set; }
        //public int MON { get; set; }

    }

}


/// <summary>
/// Summary description for JsonData
/// </summary>
public class DepartHelper
{
    //public string FlightStr { get; set; }
    public string DateDepStartStr { get; set; }
    public string Depart_list { get; set; }
    public List<FlightDetails> Flights { get; set; }
    public DateTime Date_dep_start { get; set; }


    public DepartHelper(string date_dep_start_str, string flights_str)
    {
        DateDepStartStr = date_dep_start_str;
        Date_dep_start = Convert.ToDateTime(date_dep_start_str);
        Flights = DataHelper.boxFlightInfo(flights_str);
        string depart_list = date_dep_start_str + "_" + Flights[0].num;
        Depart_list = Flights.Count > 1 ? depart_list + "_" + Flights[1].num : depart_list;

    }


    public List<DepartPlanDTO> getNewDepartPlan()
    {
        //DateTime date_dep_start = Convert.ToDateTime(date_dep_start_str);

        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {

            //Clear past lists:
            db.DepartPlans.RemoveRange(db.DepartPlans.Where(a => a.depart_list.StartsWith(DateDepStartStr)));
            db.Clients.Where(a => a.depart_list.StartsWith(DateDepStartStr)).ToList().ForEach(a => a.depart_list = "");
            db.SaveChanges();
            //Insert and update new:
            insertUpdateDepartPlan();
            return getDepartPlan();
        }

    }
    public List<DepartPlanDTO> getDepartPlan()
    {

        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {
            var query = from a in db.DepartPlans
                        join b in db.Hotels on a.hotel_fk equals b.ID
                        where a.depart_list == Depart_list
                        orderby a.time
                        select new DepartPlanDTO { depart_list = a.depart_list, hotel_fk = a.hotel_fk, hotel = b.name, time = a.time.Value, PAX=a.PAX }; //a.time.Value.Hours + ":" + a.time.Value.Minutes };

            return query.ToList();
        }
    }
    //public string getDepartureList()
    //{

    //    try
    //    {
    //        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
    //        {
    //            var query = from a in db.Clients
    //                        join c in db.DepartPlans on a.hotel_fk equals c.hotel_fk
    //                        join b in db.Hotels on a.hotel_fk equals b.ID
    //                        orderby c.time
    //                        where a.oneway == false && a.depart_list == Depart_list
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

    private int insertUpdateDepartPlan()
    {

        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {
            DateTime date1 = Flights[0].date;
            string num1 = Flights[0].num;

            var _clients = from a in db.Clients
                           select a;

            if (Flights.Count == 1)
            {
                _clients = from a in _clients
                           where (a.date_dep.Value == date1 && a.num_dep == num1)
                           select a;
            }
            else
            {
                DateTime date2 = Flights[1].date;
                string num2 = Flights[1].num;
                _clients = from a in db.Clients
                           where (a.date_dep.Value == date1 && a.num_dep == num1) ||
                                 (a.date_dep.Value == date2 && a.num_dep == num2)
                           select a;
            }



            if (_clients != null)
            {
                foreach (var item in _clients)
                {
                    item.depart_list = Depart_list;
                }
            }

            List<Client> clients = _clients.ToList();
            var query = from a in clients
                        group a by new { a.hotel_fk, a.depart_list, } into g
                        where g.Key.depart_list == Depart_list
                        select new DepartPlan
                        {
                            date_dep_start = Date_dep_start,
                            depart_list = Depart_list,
                            hotel_fk = (int)g.Key.hotel_fk,
                            PAX = g.Sum(a => a.PAX)
                            
                        };

            db.DepartPlans.AddRange(query);
            return db.SaveChanges();

        }

    }


}








//private List<FlightDetails> boxFlightInfo(string flights_str)
//{

//    string[] flights_array = flights_str.Split('~');
//    List<FlightDetails> flights = new List<FlightDetails>();

//    FlightDetails flight1 = new FlightDetails();
//    string[] flight_arr_1 = flights_array[0].Split('_');
//    flight1.num = flight_arr_1[0];
//    flight1.date = Convert.ToDateTime(flight_arr_1[1]);
//    flights.Add(flight1);

//    if (flights_array.Length > 1)
//    {
//        FlightDetails flight2 = new FlightDetails();
//        string[] flight_arr_2 = flights_array[1].Split('_');
//        flight2.num = flight_arr_2[0];
//        flight2.date = Convert.ToDateTime(flight_arr_2[1]);
//        flights.Add(flight2);
//    }

//    return flights;


//}