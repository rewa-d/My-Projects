using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskTrackr_Project
{
    public partial class Task_Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_add_Click(object sender, EventArgs e)
        {
            string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString2"].ConnectionString;
            SqlConnection myConn = new SqlConnection(connectionstring);
            string insSQL = "INSERT INTO TaskList VALUES(@title,@desc,@start,@end,@due,@userid,@status)";
            SqlCommand cmd = new SqlCommand(insSQL, myConn);
            cmd.Parameters.AddWithValue("@title", txt_title.Text);
            cmd.Parameters.AddWithValue("@desc", txt_desc.Text);
            cmd.Parameters.AddWithValue("@start", txt_start.Text);
            cmd.Parameters.AddWithValue("@end", txt_end.Text);
            cmd.Parameters.AddWithValue("@due", txt_due.Text);
            cmd.Parameters.AddWithValue("@userid", Session["userid"]);
            cmd.Parameters.AddWithValue("@status", "Active");
            int added = 0;
            try
            {
                myConn.Open();
                added = cmd.ExecuteNonQuery();
                Response.Redirect("Dashboard.aspx");
            }
            catch (Exception err)
            {
                lbl_res.Text = err.Message;
            }
            finally
            {
                myConn.Close();
            }
        }
    }
}