using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_Default : System.Web.UI.Page
{
    public string Today { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        Today = DateTime.Today.ToString("yyyy-MM-dd");
    }
}