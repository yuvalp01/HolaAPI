using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Microsoft.VisualBasic.FileIO;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using HolaAPI.Models;

public class Result
{
    public string status { get; set; }
    public string message { get; set; }
}

/// <summary>
/// Summary description for CsvUpload
/// </summary>
///
public class CsvUpload
{
    public CsvUpload()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    public string ExtractDataTabletFromCSVFileToDb(string csv_file_path)
    {

        try
        {

            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                csvReader.SetDelimiters(new string[] { ";" });
                csvReader.HasFieldsEnclosedInQuotes = true;
                using (HolaShalomDBEntities db = new HolaShalomDBEntities())
                {
                    db.Database.ExecuteSqlCommand("DELETE FROM [Upload_temp]");
                    csvReader.ReadFields();
                    int count = 0;
                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        Upload_temp rowUpload = new Upload_temp();

                        rowUpload.PNR = fieldData[0];
                        rowUpload.names = fieldData[1];
                        rowUpload.phone = fieldData[2];
                        rowUpload.date_arr = Convert.ToDateTime(fieldData[3]);
                        rowUpload.num_arr = fieldData[4];
                        rowUpload.PAX = int.Parse(fieldData[5]);
                        rowUpload.date_dep = Convert.ToDateTime(fieldData[6]);
                        rowUpload.num_dep = fieldData[7];
                        rowUpload.hotel_name = fieldData[8];
                        rowUpload.comments = fieldData[9];
                        db.Upload_temp.Add(rowUpload);
                         count +=  db.SaveChanges();

                    }
                    if (count>50)
                    {
                        throw new Exception("CSV file limited to 50 records");
                    }
                }
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            Exception rootEx = ex.GetBaseException();
            string error = "There was a problem with the CSV file (internal error message: " + rootEx.Message + ")";
            Logger.Write(error);
            return error;
        }
    }




    public Result updateTables(string fieldsStr, int rowIndex, string agency_fk, string hotel_fk)
    {
        Result result = new Result();
        Client client_new = new Client();
        try
        {


            using (HolaShalomDBEntities db = new HolaShalomDBEntities())
            {
                client_new = getClientObj(fieldsStr);

                if ((client_new.num_dep != "" && client_new.date_dep == null) || (client_new.num_dep == "" && client_new.date_dep != null))
                {
                    result.status = "danger";
                    result.message = "It is impossible have a flight number without a flight date or vice versa. Please correct the csv file and try again.";
                    return result;
                }

                client_new.hotel_fk = int.Parse(hotel_fk);
                client_new.agency_fk = int.Parse(agency_fk);
                client_new.date_update = DateTime.Now;

                db.Clients.Add(client_new);

                // Create sale object
                Sale sale_new = new Sale();
                sale_new.PNR = client_new.PNR;
                sale_new.agency_fk = client_new.agency_fk;
                sale_new.date_sale = DateTime.Today;
                sale_new.date_update = DateTime.Now;
                sale_new.canceled = false;
                sale_new.persons = client_new.PAX;
                sale_new.sale_type = "External";
                sale_new.product_fk = 1;
                sale_new.remained_pay = 0;

                db.Sales.Add(sale_new);



                var activities = from a in db.Activities
                                 join b in db.Rel_product_activity on a.ID equals b.activity_fk
                                 where b.product_fk == sale_new.product_fk
                                 select a;

                foreach (var activity in activities)
                {
                    SoldActivity saleEvent = new SoldActivity();
                    saleEvent.Sale = sale_new;
                    saleEvent.PNR = sale_new.PNR;
                    saleEvent.agency_fk = sale_new.agency_fk;
                    saleEvent.date_update = sale_new.date_update;
                    saleEvent.canceled = sale_new.canceled;
                    saleEvent.activity_fk = activity.ID;
                    saleEvent.event_fk = 0;
                    db.SoldActivities.Add(saleEvent);
                }

                db.SaveChanges();
                result.status = "success";
                result.message = String.Format("[Reservation <b>'[{0}] {1}'</b> was successfully inserted into the database.", client_new.PNR, client_new.names);

            }
        }
        catch (DbUpdateException ex)
        {
            SqlException ex_sql = ex.GetBaseException() as SqlException;
            if (ex_sql != null)
            {
                switch (ex_sql.Number)
                {
                    case 547:
                        result.status = "warning";
                        result.message = ex_sql.Message; // "One of the flights does not exist in the system. Please enter the flights and try again.";
                        return result;
                    case 2627:
                        result.status = "info";
                        result.message = String.Format("Reservation <b>[{0}] {1}</b> already exists in the system. No need to insert it again.", client_new.PNR, client_new.names);
                        return result;
                    default:
                        result.status = "danger";
                        result.message = "SQL Exception: " + ex_sql.Message;
                        return result;
                }
            }
            else
            {
                result.status = "danger";
                result.message = "SQL Exception: " + ex_sql.Message;
            }

        }
        catch (Exception ex)
        {
            result.status = "danger";
            result.message = "General Exception: " + ex.Message;
        }

        return result;
    }

    ////TODO merge into the sender method
    //private void UpdateProductsPrices(ref Sale sale)
    //{
    //    //TODO: write logic

    //    sale.remained_pay = 0;
    //    sale.product_fk = 1;
    //}

    public Client getClientObj(string fieldsStr)
    {

        Client client = new Client();
        try
        {

            string[] fields = fieldsStr.Split('~');
            client.PNR = fields[0];
            client.names = fields[1];
            client.phone = fields[2];
            client.date_arr = Convert.ToDateTime(fields[3]);
            client.num_arr = fields[4];
            client.PAX = int.Parse(fields[5]);
            client.date_dep = Convert.ToDateTime(fields[6]);
            client.num_dep = fields[7];
            client.comments = fields[8];

        }
        catch (Exception ex)
        {
            throw new Exception("There was a problem creating the  'ClientDTO' object (internal error message: " + ex.Message + ")");
        }

        return client;
    }
}



//var csv_ = new List<Dictionary<string, string>[]>(); // or, List<YourClass>
//var csv = new List<string[]>(); // or, List<YourClass>
//var clients = new List<Client>();
//var lines = System.IO.File.ReadAllLines(csv_file_path);

//Dictionary<string, string> xx = new Dictionary<string, string>();


//foreach (string line in lines)
//{
//    Client client = new Client()
//    {


//    };
//    csv.Add(line.Split(';'));
//} // or, populate YourClass          
//string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(csv);







