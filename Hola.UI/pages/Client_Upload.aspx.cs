using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualBasic.FileIO;
using HolaAPI.Models;

public partial class pages_Client_Upload : System.Web.UI.Page
{

    //readonly string sql_connString = ConfigurationManager.ConnectionStrings["HolaShalomDB"].ConnectionString;
    readonly string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

    protected void btnUpload_Click(object sender, EventArgs e)
    {

        if (ddlAgencies.SelectedIndex != 0)
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string extension = Path.GetExtension(FileUpload1.PostedFile.FileName);
            if (extension == ".csv")
            {
                string filePath = Server.MapPath(FolderPath + "_" + fileName);
                FileUpload1.SaveAs(filePath);
                CsvUpload csvUpload = new CsvUpload();
                string errorMessage = csvUpload.ExtractDataTabletFromCSVFileToDb(filePath);
                if (errorMessage == string.Empty)
                {
                    LoadGrid();
                }
                else
                {

                    lblFeedback.Text = errorMessage;
                    pnlFeedback.CssClass = "alert  alert-dismissable alert-danger";
                    pnlFeedback.Style["display"] = "block";
                    return;
                }
            }
            else
            {


                lblFeedback.Text = "File extension must be '.csv'";
                pnlFeedback.CssClass = "alert  alert-dismissable alert-danger";
                pnlFeedback.Style["display"] = "block";
                return;
            }

        }
        else
        {
            lblFeedback.Text = "Please select agency";
            pnlFeedback.CssClass = "alert  alert-dismissable alert-danger";
            pnlFeedback.Style["display"] = "block";

        }

    }


    private void LoadGrid()
    {

        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {
            GridView1.DataSource = db.Upload_temp.ToList();
            GridView1.DataBind();
        }
    }


    private void LoadAgencies()
    {


        using (HolaShalomDBEntities db = new HolaShalomDBEntities())
        {

            ddlAgencies.DataSource = db.Agencies.ToList();
            ddlAgencies.DataBind();
            ListItem unknown = ddlAgencies.Items.FindByText("UNKNOWN");
            ddlAgencies.Items.Remove(unknown);
            ListItem hola = ddlAgencies.Items.FindByText("Hola Shalom");
            ddlAgencies.Items.Remove(hola);
            ListItem select = new ListItem() { Text = "Select Agency", Value = "0" };
            ddlAgencies.Items.Insert(0, select);
        }


    }

    static List<Hotel> hotels;

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var ddl = e.Row.FindControl("ddlHotels") as DropDownList;
            if (ddl != null)
            {
                //ddl.DataSource = new List<string>() { "0", "1", "2", "3", "4" };
                ddl.DataSource = LoadHotels();
                ddl.DataTextField = "name";
                ddl.DataValueField = "ID";
                ddl.DataBind();
            }
        }
    }

    private List<Hotel> LoadHotels()
    {

        if (hotels == null)
        {
            using (HolaShalomDBEntities db = new HolaShalomDBEntities())
            {
                var _hotels = from a in db.Hotels orderby a.name select a;
                hotels = _hotels.ToList();
                hotels.Insert(0, new Hotel() { ID = 0, name = "Select Hotel" });

            }
        }
        return hotels.ToList();

    }


    protected void GridView1_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "InsertClient") return;
        lblFeedback.Text = e.CommandArgument.ToString();
        int rowIndex = ((GridViewRow)((System.Web.UI.Control)e.CommandSource).BindingContainer).RowIndex;
        DropDownList ddlHotels = GridView1.Rows[rowIndex].FindControl("ddlHotels") as DropDownList;
        CsvUpload csvUpload = new CsvUpload();
        Result result = new Result();
        if (ddlHotels.SelectedValue != "0"&& ddlAgencies.SelectedValue != "0")
        {
            result = csvUpload.updateTables(e.CommandArgument.ToString(), rowIndex, ddlAgencies.SelectedValue, ddlHotels.SelectedValue);
        }
        else
        {
            result.status = "warning";
            result.message = "Please choose hotel and agency";
        }

        GridView gv = sender as GridView;
        gv.Rows[rowIndex].CssClass = result.status;
        lblFeedback.Text = result.message;
        lblFeedback.CssClass = result.status;
        pnlFeedback.Style["display"] = "block";

        pnlFeedback.CssClass = "alert alert-dismissable alert-" + result.status;


    }

    //private Result insertClient(string fieldsStr, int rowIndex)
    //{
    //    Result result = new Result();
    //    int count_success = 0;
    //    int count_client = 0;
    //    int count_sale = 0;

    //    try
    //    {

    //        if ( ddlAgencies.SelectedValue != "0")
    //        {
    //            CsvUpload csvUpload = new CsvUpload();
    //            Client client =   csvUpload.getClientObj(fieldsStr, rowIndex, ddlAgencies.SelectedValue);
    //        }
    //        else
    //        {
    //            result.status = "warning";
    //            result.message = "Please choose hotel and agency";
    //            return result;
    //        }

    //        if ((client.num_dep != "" && client.date_dep == null) || (client.num_dep == "" && client.date_dep != null))
    //        {
    //            result.status = "danger";
    //            result.message = "It is impossible have a flight number without a flight date or vice versa. Please correct the csv file and try again.";
    //            return result;

    //        }
    //        using (SqlConnection cnn = new SqlConnection(sql_connString))
    //        {


    //            string cmdText_Client = @"INSERT INTO [dbo].[Clients]
    //                       ([PNR]
    //                       ,[names]
    //                       ,[PAX]
    //                       ,[num_arr]
    //                       ,[date_arr]
    //                       ,[num_dep]
    //                       ,[date_dep]
    //                       ,[phone]
    //                       ,[hotel_fk]
    //                       ,[agency_fk]
    //                       ,[oneway]
    //                       ,[comments]
    //                       ,[date_update]
    //                       ,[canceled])

    //                values  
    //                       (@PNR
    //                       ,@names
    //                       ,@PAX
    //                       ,@num_arr
    //                       ,@date_arr
    //                       ,@num_dep
    //                       ,@date_dep
    //                       ,@phone
    //                       ,@hotel_fk
    //                       ,@agency_fk
    //                       ,@oneway
    //                       ,@comments
    //                       ,@date_update
    //                       ,@canceled )";


    //            //string cmdText= "insert into Insert values(@PNR,@names,@PAX)";
    //            using (SqlCommand cmd = new SqlCommand(cmdText_Client, cnn))
    //            {
    //                cmd.Parameters.AddWithValue("@PNR", client.PNR);
    //                cmd.Parameters.AddWithValue("@names", client.names);
    //                cmd.Parameters.AddWithValue("@PAX", client.PAX);
    //                cmd.Parameters.AddWithValue("@num_arr", client.num_arr);
    //                cmd.Parameters.AddWithValue("@date_arr", client.date_arr);

    //                if (client.num_dep == "") cmd.Parameters.AddWithValue("@num_dep", DBNull.Value);
    //                else cmd.Parameters.AddWithValue("@num_dep", client.num_dep);

    //                if (client.date_dep == null) cmd.Parameters.AddWithValue("@date_dep", DBNull.Value);
    //                else cmd.Parameters.AddWithValue("@date_dep", client.date_dep);

    //                cmd.Parameters.AddWithValue("@phone", client.phone);
    //                cmd.Parameters.AddWithValue("@comments", client.comments);
    //                cmd.Parameters.AddWithValue("@oneway", client.oneway);
    //                cmd.Parameters.AddWithValue("@agency_fk", client.agency_fk);
    //                cmd.Parameters.AddWithValue("@hotel_fk", client.hotel_fk);
    //                cmd.Parameters.AddWithValue("@date_update", DateTime.Now);
    //                cmd.Parameters.AddWithValue("@canceled", false);
    //                cnn.Open();
    //                count_client = cmd.ExecuteNonQuery();

    //            }

    //            string cmdText_Sale = @"INSERT INTO [dbo].[Sales]
    //                       ([PNR]
    //                       ,[product_fk]
    //                       ,[persons]
    //                       ,[price]
    //                       ,[sale_type]
    //                       ,[date_sale]
    //                       ,[date_update]
    //                       ,[paid]
    //                       ,[canceled]
    //                       ,[agency_fk])

    //                values  
    //                       (@PNR
    //                       ,@product_fk
    //                       ,@persons
    //                       ,@price
    //                       ,@sale_type
    //                       ,@date_sale
    //                       ,@date_update
    //                       ,@paid
    //                       ,@canceled
    //                       ,@agency_fk )";


    //            using (SqlCommand cmd = new SqlCommand(cmdText_Sale, cnn))
    //            {
    //                int product_fk = 1;
    //                decimal price = 4.5M;
    //                if (client.num_dep == "")
    //                {
    //                    product_fk = 2;
    //                    price = 2;
    //                }
    //                cmd.Parameters.AddWithValue("@PNR", client.PNR);
    //                cmd.Parameters.AddWithValue("@product_fk", product_fk);
    //                cmd.Parameters.AddWithValue("@persons", client.PAX);
    //                cmd.Parameters.AddWithValue("@price", price);
    //                cmd.Parameters.AddWithValue("@sale_type", "External");

    //                cmd.Parameters.AddWithValue("@date_sale", DateTime.Today);
    //                cmd.Parameters.AddWithValue("@date_update", DateTime.Now);
    //                cmd.Parameters.AddWithValue("@paid", false);
    //                cmd.Parameters.AddWithValue("@canceled", false);
    //                cmd.Parameters.AddWithValue("@agency_fk", client.agency_fk);

    //                count_sale = cmd.ExecuteNonQuery();
    //                count_success = +count_sale;

    //            }

    //        }


    //    }

    //    catch (SqlException ex)
    //    {
    //        if (ex.Number == 547)
    //        {

    //            result.status = "warning";
    //            result.message = ex.Message; // "One of the flights does not exist in the system. Please enter the flights and try again.";
    //            return result;
    //        }
    //        else if (ex.Number == 2627)
    //        {
    //            result.status = "info";
    //            result.message = String.Format("Reservation <b>[{0}] {1}</b> already exists in the system. No need to insert it again.", client.PNR, client.names);
    //            return result;
    //        }
    //        //lblFeedback.Text = "There was a problem inserting the row into the 'Client' table (internal error message: " + ex.Message + ")";
    //        result.status = "danger";
    //        result.message = "There was a problem inserting the row into the 'Client' table (internal error message: " + ex.Message + ")";
    //        return result;
    //        //return "There was a problem inserting the row into the 'Client' table (internal error message: " + ex.Message + ")";
    //    }

    //    if (count_success == 2)
    //    {
    //        result.status = "success";
    //        result.message = String.Format("[Reservation <b>'[{0}] {1}'</b> was successfully inserted into the database.", client.PNR, client.names);
    //        return result;
    //    }
    //    else
    //    {
    //        result.status = "danger";
    //        result.message = "Please save print screen and contact Yuval 626650117";
    //        return result;
    //    }

    //}

    //private Client getClientObj(string fieldsStr, int rowIndex)
    //{


    //    Client client = new Client();
    //    try
    //    {

    //        string[] fields = fieldsStr.Split('~');
    //        client.PNR = fields[0];
    //        client.names = fields[1];
    //        client.PAX = int.Parse(fields[2]);
    //        client.num_arr = fields[3];
    //        client.date_arr = Convert.ToDateTime(fields[4]);
    //        client.num_dep = fields[5];

    //        if (fields[6] != "")
    //        {
    //            client.date_dep = Convert.ToDateTime(fields[6]);
    //            client.oneway = false;
    //        }
    //        else
    //        {
    //            client.date_dep = null;
    //            client.oneway = true;
    //        }



    //        client.phone = fields[7];
    //        client.comments = fields[8];

    //        client.agency_fk = int.Parse(ddlAgencies.SelectedValue);

    //        DropDownList ddlHotels = GridView1.Rows[rowIndex].FindControl("ddlHotels") as DropDownList;
    //        client.hotel_fk = int.Parse(ddlHotels.SelectedValue);


    //    }
    //    catch (Exception ex)
    //    {
    //        pnlFeedback.Style["display"] = "block";
    //        lblFeedback.Text = "There was a problem creating the  'ClientDTO' object (internal error message: " + ex.Message + ")";
    //    }


    //    return client;

    //}








    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            LoadAgencies();

        }

    }












    protected void lnkDownloadExample_Click(object sender, EventArgs e)
    {
        Response.ContentType = "application/octet-stream";
        Response.AppendHeader("Content-Disposition", "attachment; filename=UploadFile_example.csv");
        string FilePath = Server.MapPath(FolderPath + "UploadFile_example.csv");
        Response.TransmitFile(FilePath);
        Response.End();
    }


}

//public class ClientDTO
//{

//    public string PNR { get; set; }
//    public string names { get; set; }
//    public int PAX { get; set; }
//    public string num_arr { get; set; }
//    public DateTime date_arr { get; set; }
//    public string num_dep { get; set; }
//    public Nullable<DateTime> date_dep { get; set; }
//    public string phone { get; set; }
//    public int hotel_fk { get; set; }
//    public Nullable<int> agency_fk { get; set; }
//    public bool oneway { get; set; }
//    public bool canceled { get; set; }
//    public string comments { get; set; }
//    public DateTime date_update { get; set; }

//}





//protected void ddlAgencies_SelectedIndexChanged(object sender, EventArgs e)
//{
//    if (ddlAgencies.SelectedIndex!=0)
//    {
//        btnUpload.Enabled = true;
//    }
//    else
//    {
//        btnUpload.Enabled = false;
//    }
//}



//protected void ddlQuantity_SelectedIndexChanged(object sender, EventArgs e)
//{
//    GridViewRow gvr = ((DropDownList)sender).NamingContainer as GridViewRow;
//    if (gvr != null)
//    {
//        decimal price = 0.00M;
//        int quantity = 0;
//        //We can find all the controls in this row and do operations on them
//        var ddlQuantity = gvr.FindControl("ddlHotels") as DropDownList;
//        //var lblPrice = gvr.FindControl("lblPrice") as Label;
//        //var lblAmount = gvr.FindControl("lblAmount") as Label;
//        //if (ddlQuantity != null && lblPrice != null && lblAmount != null)
//        if (ddlQuantity != null)
//        {
//            int.TryParse(ddlQuantity.SelectedValue, out quantity);
//            //decimal.TryParse(lblPrice.Text, out price);

//            //lblAmount.Text = (price * quantity).ToString();
//        }
//    }
//}




//OleDbConnection Econ;
//SqlConnection con;


//string constr, Query, sqlconn;



//private void ExcelConn(string FilePath)
//{

//    //constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
//    constr = string.Format(ConfigurationManager.ConnectionStrings["ExcelConString"].ConnectionString, FilePath);

//    Econ = new OleDbConnection(constr);

//}
//private void connection()
//{
//    sqlconn = ConfigurationManager.ConnectionStrings["HolaShalomDB"].ConnectionString;
//    con = new SqlConnection(sqlconn);

//}


//private void InsertExcelRecords(string FilePath)
//{
//    ExcelConn(FilePath);
//    using (SqlConnection cnn = new SqlConnection(sql_connString))
//    {
//        using (var cmd = cnn.CreateCommand())
//        {
//            cnn.Open();
//            cmd.CommandText = "DELETE FROM Upload_temp";
//            cmd.ExecuteNonQuery();
//        }
//    }


//    Query = string.Format("Select [PNR],[names],[PAX],[num_arr],[date_arr],[num_dep],[date_dep],[phone],[hotel_name],[comments] FROM [{0}]", "Sheet1$");
//    OleDbCommand Ecom = new OleDbCommand(Query, Econ);
//    Econ.Open();

//    DataSet ds = new DataSet();
//    OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
//    Econ.Close();
//    oda.Fill(ds);
//    DataTable Exceldt = ds.Tables[0];
//    connection();
//    //creating object of SqlBulkCopy    
//    SqlBulkCopy objbulk = new SqlBulkCopy(con);
//    //assigning Destination table name    
//    objbulk.DestinationTableName = "Upload_temp";
//    //Mapping Table column    
//    objbulk.ColumnMappings.Add("PNR", "PNR");
//    objbulk.ColumnMappings.Add("names", "names");
//    objbulk.ColumnMappings.Add("PAX", "PAX");
//    objbulk.ColumnMappings.Add("num_arr", "num_arr");
//    objbulk.ColumnMappings.Add("date_arr", "date_arr");
//    objbulk.ColumnMappings.Add("num_dep", "num_dep");
//    objbulk.ColumnMappings.Add("date_dep", "date_dep");
//    objbulk.ColumnMappings.Add("phone", "phone");
//    objbulk.ColumnMappings.Add("hotel_name", "hotel_name");
//    //objbulk.ColumnMappings.Add("agency_name", "agency_name");
//    objbulk.ColumnMappings.Add("comments", "comments");
//    //inserting Datatable Records to DataBase    
//    con.Open();
//    objbulk.WriteToServer(Exceldt);
//    con.Close();

//}

