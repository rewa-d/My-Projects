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
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_signup_Click(object sender, EventArgs e)
        {
            if (IsValid)
            {
                string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString"].ConnectionString;
                SqlConnection myConn = new SqlConnection(connectionstring);
                string insSQL = "INSERT INTO InfoUser VALUES(@fname,@lname,@email,@username,@pass)";
                SqlCommand cmd = new SqlCommand(insSQL, myConn);
                cmd.Parameters.AddWithValue("@fname", txt_fname.Text);
                cmd.Parameters.AddWithValue("@lname", txt_lname.Text);
                cmd.Parameters.AddWithValue("@email", txt_email.Text);
                cmd.Parameters.AddWithValue("@username", txt_user.Text);
                cmd.Parameters.AddWithValue("@pass", txt_pass.Text);
                int added = 0;
                try
                {
                    myConn.Open();
                    added = cmd.ExecuteNonQuery();
                    Response.Redirect("Login.aspx");
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
}


