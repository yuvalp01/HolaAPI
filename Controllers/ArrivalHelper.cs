using System;
using System.Linq;
using System.Collections.Generic;
using HolaAPI.Models;

public class Arrival
{
    public string PNR { get; set; }
    public string names { get; set; }
    public string hotel { get; set; }
    public string phone { get; set; }
    public int PAX { get; set; }
    public int MKF { get; set; }
    public int VDN { get; set; }
    public int FIG { get; set; }
    public int MON { get; set; }

}
public class SaleRow
{
    public string PNR { get; set; }
    public int MKF { get; set; }
    public int VDN { get; set; }
    public int FIG { get; set; }
    public int MON { get; set; }

}
public class FlightDetails
{
    public DateTime date { get; set; }
    public string num { get; set; }
}

/// <summary>
/// Summary description for JsonData
/// </summary>
public class ArrivalHelper
{


    public List<TourPlanDTO> getTourPlan(string date_start, string date_end)
    {
        DateTime dateStart = Convert.ToDateTime(date_start);
        DateTime dateEnd = Convert.ToDateTime(date_end);

        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {
            var query = from a in db.TourPlans
                        join b in db.Products on a.product_fk equals b.ID
                        where a.date >= dateStart && a.date <= dateEnd
                        select new TourPlanDTO
                        {
                            date = a.date,
                            time = a.time.Value,
                            tour_name = b.name,
                            comments = a.comments
                        };



            return query.ToList();

        }


    }


    public List<Arrival> getListArrival(string date_start_str, string flights_str)
    {

        DateTime date_start = Convert.ToDateTime(date_start_str);
        string[] flights_array = flights_str.Split('~');
        List<FlightDetails> flights = DataHelper.boxFlightInfo(flights_str);
        List<SaleRow> salesTable = getSalesTable();
        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {
            //DateTime earliest_date_arrival = _clients.OrderBy(a => a.date_arr).FirstOrDefault().date_arr;
            //DateTime latest_date_departure = _clients.OrderByDescending(a => a.date_dep).FirstOrDefault().date_dep.Value;

            DateTime date1 = flights[0].date;
            string num1 = flights[0].num;

            var _clients = db.Clients.Select(a => a);
            var _clients_filter_1 = _clients.Where(a => a.date_arr == date1 && a.num_arr == num1);
            var clients_filter_all = _clients_filter_1;

            if (flights.Count > 1)
            {
                for (int i = 1; i < flights.Count; i++)
                {
                    DateTime date = flights[i].date;
                    string num = flights[i].num;
                    var clients_filter = _clients.Where(a => a.date_arr == date && a.num_arr == num);
                    clients_filter_all = _clients_filter_1.Concat(clients_filter);

                }
            }


            var query = from a in salesTable
                        join b in clients_filter_all on a.PNR equals b.PNR
                        join c in db.Hotels on b.hotel_fk equals c.ID
                        //where b.date_arr == date_start
                        orderby c.name, b.names
                        select new Arrival
                        {
                            PNR = b.PNR,
                            names = b.names,
                            hotel = c.name,
                            phone = b.phone,
                            PAX = b.PAX,
                            MKF = a.MKF,
                            VDN = a.VDN,
                            FIG = a.FIG,
                            MON = a.MON
                        };




            return query.ToList<Arrival>();

        }
    }

    private List<SaleRow> getSalesTable()
    {
        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {

            List<SaleRow> salesTable = (from a in db.Sales
                                        group a by a.PNR into g
                                        select new SaleRow { PNR = g.Key }).ToList();

            foreach (SaleRow row in salesTable)
            {
                var products = from a in db.Sales
                               where a.PNR == row.PNR
                               group a by new { a.product_fk } into g
                               select new { g.Key.product_fk, people = g.Sum(s => s.persons) };
                foreach (var item in products)
                {
                    switch (item.product_fk)
                    {

                        case 11:
                            row.MKF = item.people;
                            break;
                        case 12:
                            row.VDN = item.people;
                            break;
                        case 13:
                            row.FIG = item.people;
                            break;
                        case 14:
                            row.MON = item.people;
                            break;

                        default:
                            break;
                    }
                }
            }
            return salesTable;

        }

    }

}



//var clients_flight = _clients.Where(a => a.date_arr == date1 && a.num_arr == num1);

//if (flights.Count == 2)
//{
//    DateTime date2 = flights[1].date;
//    string num2 = flights[1].num;
//    clients_flight = _clients.Where(a => (a.date_arr == date1 && a.num_arr == num1) || (a.date_arr == date2 && a.num_arr == num2));

//}