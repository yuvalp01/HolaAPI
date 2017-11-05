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
    public string UrlPlan { get; set; }

    public string Passengers { get; set; }
    public int TotalP { get; set; }
    public string TourNames { get; set; }
    public string TourPlan { get; set; }
    public string Flights { get; set; }

    
    public string DATA { get; set; }

    private HolaShalomDBEntities db = new HolaShalomDBEntities();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

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
            list.pickup_time = _event.time.Value;

            

            List<string> PNRs = (from a in db.SoldActivities
                                 where a.event_fk == event_fk && a.canceled == false
                                 select a.PNR).ToList();

            var flights = (from a in db.Clients
                           where PNRs.Contains(a.PNR)
                           select a.Flight).Distinct();

            list.flights= (from a in flights
                    select new FlightDTO { num = a.num, date = a.date, time = a.time }).ToList();

            DateTime lastDepDate = (from a in db.Clients
                                    where PNRs.Contains(a.PNR)
                                    orderby a.date_dep descending
                                    select a.date_dep).FirstOrDefault();
            DateEnd = lastDepDate.ToString("yyyy-MM-dd");
            list.tour_plan = GetTourPlan(_event.date, lastDepDate, event_fk);

            var existing_tours_fks = list.tour_plan.Select(a => a.activity_fk).Distinct().ToList();
            var existing_tours_names = from a in db.Activities
                                       where existing_tours_fks.Contains(a.ID)
                                       select a.subcat;




            list.passengers = GetPassengersList(event_fk, PNRs, existing_tours_fks);

            TotalP = list.passengers.Sum(a => a.PAX);


            var agencies = from a in db.Clients
                           where PNRs.Contains(a.PNR)
                           select a.Agency.name;


            TourNames = Newtonsoft.Json.JsonConvert.SerializeObject(existing_tours_names);


            db.Dispose();
            DATA = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            Passengers = Newtonsoft.Json.JsonConvert.SerializeObject(list.passengers);
            TourPlan = Newtonsoft.Json.JsonConvert.SerializeObject(list.tour_plan);
            Flights = Newtonsoft.Json.JsonConvert.SerializeObject(list.flights);
            DateStart = list.date.ToString("yyyy-MM-dd");
        }
    }



    public List<EventDTO> GetTourPlan(DateTime dateStart, DateTime lastDepDate, int event_fk)
    {

        var tour_plan = from a in db.Events
                        where a.date >= dateStart && a.date <= lastDepDate && a.category == "tour" && a.canceled == false

                        select new EventDTO
                        {
                            ID = a.ID,
                            date = a.date,
                            time = a.time.Value,
                            activity_fk = a.activity_fk,
                            activity_name = a.Activity.name,
                            guide_fk = a.guide_fk,
                            guide_name = a.Guide.name,
                            comments = a.comments,
                            date_update = a.date_update,
                            subcat = a.Activity.subcat
                        };
        List<EventDTO> tour_planDTO = new List<EventDTO>(tour_plan);
        return tour_planDTO.ToList(); ;

    }

    public List<PassengersRow> GetPassengersList(int event_fk, List<string> PNRs, List<int> tour_fks)
    {

        var lines = (from a in db.SoldActivities
                     where a.event_fk == event_fk && a.canceled == false //&& a.Activity.direction == "IN"
                     select a);
        //List<string> PNRs = lines.Select(b => b.PNR).ToList();
        List<PassengersRow> arrivals = new List<PassengersRow>();
        //var existing_tours = (from a in db.SoldActivities
        //                      where PNRs.Contains(a.PNR) && a.Activity.category == "tour"
        //                      select (a.activity_fk)).Distinct();

        foreach (SoldActivity line in lines)
        {
            PassengersRow passenger = new PassengersRow();
            passenger.PNR = line.PNR;
            Client client = db.Clients.SingleOrDefault(a => a.PNR == line.PNR && a.agency_fk == line.Sale.agency_fk);
            passenger.names = client.names;
            passenger.agency_name = client.Agency.name;
            passenger.phone = client.phone;
            passenger.hotel_name = client.Hotel.name; //db.Clients.Where(a => a.PNR == line.PNR && a.agency_fk == line.agency_fk).Select(b => b.Hotel.name).SingleOrDefault();
            passenger.PAX = client.PAX;
            passenger.num_arr = client.num_arr;
            passenger.days = (client.date_dep - client.date_arr).Days;



            foreach (var activity_fk in tour_fks)
            {

                var xxx = from a in db.SoldActivities
                          where a.activity_fk == activity_fk
                          select a.Activity.name;


                var tour_activities = db.SoldActivities
                    .Where(
                     a => 
                     a.PNR == line.Sale.PNR &&
                     a.agency_fk == line.Sale.agency_fk &&
                     a.canceled == false &&
                     a.activity_fk == activity_fk
                     );
                    //&&
                    // //a.agency_fk == line.Sale.agency_fk &&
                    // //a.canceled == false &&
                    // a.activity_fk == activity_fk);
                passenger.activities.Add(new ActivityPair()
                {
                    activity_fk = activity_fk,
                    sum = tour_activities.Sum(a => (int?)a.Sale.persons) ?? 0
                });
            }
            arrivals.Add(passenger);


        }
        arrivals = arrivals.OrderBy(a => a.hotel_name).ToList();
        return arrivals;


    }


}