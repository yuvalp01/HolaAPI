using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterHola : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            favicon_link.Href = "~/"+ ConfigurationManager.AppSettings["favicon"];
            string environment = ConfigurationManager.AppSettings["environment"];
            if (environment!="PROD")
            {
                lblEnvironment.Text = environment;
            }

        }
    }
}
