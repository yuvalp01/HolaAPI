using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HolaAPI.Models
{


    public class ReservationDTO
    {

        public string PNR { get; set; }
        public int agency_fk { get; set; }
        public string names { get; set; }
        public int PAX { get; set; }
        public string num_arr { get; set; }
        public DateTime date_arr { get; set; }
        public string num_dep { get; set; }
        public DateTime date_dep { get; set; }
        public string phone { get; set; }
        public int hotel_fk { get; set; }
        public bool canceled { get; set; }
        public string comments { get; set; }
        public DateTime date_update { get; set; }

        public string sale_type { get; set; }
        public int product_fk { get; set; }
        public int remained_pay { get; set; }

        public string hotel_name { get; set; }
        public string agency_name { get; set; }
    }


    public class SaleDTO
    {

        public int ID { get; set; }
        public string PNR { get; set; }
        public int agency_fk { get; set; }
        public int product_fk { get; set; }
        public string product_name { get; set; }
        public int persons { get; set; }
        public decimal remained_pay { get; set; }
        public string sale_type { get; set; }
        public DateTime date_sale { get; set; }
        public DateTime date_update { get; set; }
        public bool canceled { get; set; }
        public string comments { get; set; }

    }


    public class ActivityDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string subcat { get; set; }
        public string direction { get; set; }
        public string city { get; set; }
        public DateTime date_update { get; set; }
    }
    public class FlightDTO
    {

        public string num { get; set; }
        public DateTime date { get; set; }
        public TimeSpan time { get; set; }
        public string destination { get; set; }
        public string direction { get; set; }
        //public Nullable<System.TimeSpan> time_approved { get; set; }
        public DateTime date_update { get; set; }
        public int sum { get; set; }
        public string ph { get; set; }
    }

    public class DepartPlanDTO
    {
        public int event_fk { get; set; }
        public string hotel_name { get; set; }
        public int hotel_fk { get; set; }
        public TimeSpan? time { get; set; }
        public int PAX { get; set; }

    }

    public class ProductDTO
    {

        public int ID { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string category { get; set; }
        public string subcat { get; set; }
        public decimal rate { get; set; }

    }

    public class ClientDTO
    {

        public string PNR { get; set; }
        public string names { get; set; }
        public int PAX { get; set; }
        public string num_arr { get; set; }
        public DateTime date_arr { get; set; }
        public string num_dep { get; set; }
        public DateTime date_dep { get; set; }
        public string phone { get; set; }
        public int hotel_fk { get; set; }
        public string hotel_name { get; set; }
        public int agency_fk { get; set; }
        public string agency_name { get; set; }
        public bool oneway { get; set; }
        public bool canceled { get; set; }
        public string comments { get; set; }
        public DateTime date_update { get; set; }
        public string depart_list { get; set; }
        public string arrival_list_fk { get; set; }
    }

    public class GuideDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
    }

    public class HotelDTO
    {
        public int ID { get; set; }
        public string name { get; set; }
        public string address { get; set; }
    }
}