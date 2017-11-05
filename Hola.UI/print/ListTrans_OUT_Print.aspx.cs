using HolaAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ListArrival_Print : System.Web.UI.Page
{
    public string DateStart { get; set; }
    public string DateEnd { get; set; }
    public string UrlList { get; set; }

    public string Passengers { get; set; }
    public int TotalP { get; set; }

    public string DATA { get; set; }
    public string Flights { get; set; }

    private HolaShalomDBEntities db = new HolaShalomDBEntities();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //if (Request.Form["_details"] != null)
            //{

            int event_fk = int.Parse(Request.QueryString["event_fk"]);
            DateTime date = DateTime.Parse(Request.QueryString["date"]);
            TimeSpan time = TimeSpan.Parse(Request.QueryString["time"]);
            int activity_fk = int.Parse(Request.QueryString["activity_fk"]);
            int guide_fk = int.Parse(Request.QueryString["guide_fk"]);
            string comments_trans = Request.QueryString["comments_trans"];

            TransportList list = new TransportList();
            Event _event = db.Events.SingleOrDefault(a => a.ID == event_fk);
            list.activity_name = _event.Activity.name;
            list.comments_trans = _event.comments;
            list.date = _event.date;
            list.guide_name = _event.Guide.name;
            list.passengers = GetPassengersList(event_fk);
            TotalP = list.passengers.Sum(a => a.PAX);
            list.pickup_time = _event.time.Value;


            var PNRs = (from a in db.SoldActivities
                        where a.event_fk == event_fk && a.canceled == false
                        select a.PNR).ToList();

            var flights = (from a in db.Clients
                           where PNRs.Contains(a.PNR)
                           select a.Flight1).Distinct();

            list.flights = (from a in flights
                            select new FlightDTO { num = a.num, date = a.date, time = a.time }).ToList();

            var agencies = from a in db.Clients
                           where PNRs.Contains(a.PNR)
                           select a.Agency.name;
            db.Dispose();
            DATA = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            Passengers = Newtonsoft.Json.JsonConvert.SerializeObject(list.passengers);
            Flights = Newtonsoft.Json.JsonConvert.SerializeObject(list.flights);

            DateStart = list.date.ToString("yyyy-MM-dd");
        }
    }

    public List<PassengersRow> GetPassengersList(int event_fk)
    {

        var passengers = (from a in db.SoldActivities
                      join b in db.Clients on new { a.PNR, a.agency_fk } equals new { b.PNR, b.agency_fk }
                      join c in db.DepartPlans.Where(x => x.event_fk == event_fk) on b.hotel_fk equals c.hotel_fk
                      where a.event_fk == event_fk && a.canceled == false
                     
                      select new PassengersRow
                      {
                          PNR = a.PNR,
                          names = b.names,
                          agency_name = b.Agency.name,
                          phone = b.phone,
                          hotel_name = b.Hotel.meeting_point +" ("+ b.Hotel.name+")",
                          PAX = b.PAX,
                          event_fk = a.event_fk.Value,
                          time = c.time.Value,
                          //title = b.Hotel.name + " At " + c.time.Value.Hours + ":" + c.time.Value.Minutes
                           
                      });


        return passengers.OrderBy(a => a.time).ToList();


    }


}