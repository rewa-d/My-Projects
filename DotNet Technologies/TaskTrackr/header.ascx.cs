using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskTrackr_Project
{
    public partial class header : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
                // Check if the session variable exists and is not null
                if (Session["fname"] != null)
                {
                    // Set the session variable to the label text
                    //lbl_res.Text = "Welcome, " + Session["fname"].ToString() + "!";
                }
                
            


        }
    }
}