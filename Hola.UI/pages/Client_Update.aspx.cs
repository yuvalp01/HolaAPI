using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_Client_Update : System.Web.UI.Page
{
    public string Search { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["search"] !=null)
            {
                Search = Request.QueryString["search"];
            }
        }
    }
}