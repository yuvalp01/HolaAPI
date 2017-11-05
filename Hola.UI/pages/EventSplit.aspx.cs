using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_EventSplit : System.Web.UI.Page
{
    public string ORIGINAL_LIST { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["ORIGINAL_LIST"]!=null)
            {
                ORIGINAL_LIST = Request.QueryString["ORIGINAL_LIST"];
            }
           
        }
    }
}