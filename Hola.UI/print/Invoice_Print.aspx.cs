using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//public class AgencyDTO
//{
//    public int ID { get; set; }
//    public string name { get; set; }
//    public string address { get; set; }
//}

public partial class Invoice_Print : System.Web.UI.Page
{

    public string Date { get; set; }
    public string Year { get; set; }
    public string Month { get; set; }
    public string Sum { get; set; }
    public string Base { get; set; }
    public string VAT { get; set; }
    public string DueDate { get; set; }
    public string Agency_fk { get; set; }

    //public string Address { get; set; }
    //public string Agency { get; set; }


    private void setDate()
    {
        int _month = int.Parse(Request["month"]);
        Agency_fk = Request["agency_fk"];
        string _total = Request["total"];
        Year = Request["year"];
        //Agency = Request["Agency"];
        //Address = Request["Address"];

        Date = DateTime.Today.ToString("yyyy-MM-dd");

        Month = DateTimeFormatInfo.CurrentInfo.GetMonthName(_month);

        decimal sum_d = decimal.Parse(_total);
        Sum = String.Format("{0:C}", sum_d);
        decimal base_d = (sum_d / (decimal)1.1);
        Base = String.Format("{0:C}", base_d);
        VAT = String.Format("{0:C}", (sum_d - base_d));
        DueDate = DateTime.Today.AddMonths(1).ToString("yyyy-MM-dd");



        //using (var client = new HttpClient())
        //{
        //    // New code:
        //    var api_url = ConfigurationManager.AppSettings["api_url"];
        //    client.BaseAddress = new Uri(api_url);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    HttpResponseMessage response = await client.GetAsync("/api/agencies/"+ agency_fk);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        AgencyDTO agency = await response.Content.ReadAsAsync<AgencyDTO>();// .ReadAsAsync  ();// .Content.ReadAsAsync > Product > ();
        //                                                                           //}
        //        Agency = agency.name;
        //        Address = agency.address;

        //    }


        //    //= Math.Round(base_d, 2).ToString("");

        //}

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
            setDate();
            //lblOperator.Text = Agency;
            //lblAddress.Text = Address;
            lblFecha.Text = Date;
            lblMonth.Text = Month + ", " + Year;
            }
            catch (Exception ex )
            {

                Response.Write(ex.Message);
            }


        }
    }
}