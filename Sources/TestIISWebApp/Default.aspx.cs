using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TestIISWebApp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string imageUrl = Page.ClientScript.GetWebResourceUrl(typeof(WebCustomControl1), "TestIISWebApp.th.jpg");
            Image1.ImageUrl = imageUrl; 
        }
    }
}