using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_PlanRoute : System.Web.UI.Page
{
    public string event_fk { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["event_fk"]!=null)
        {
            event_fk = Request.QueryString["event_fk"];
        }

    }
}