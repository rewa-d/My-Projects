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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            string connectionString = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString"].ConnectionString;
            SqlConnection myConn = new SqlConnection(connectionString);
            string selSQL = "SELECT * FROM InfoUser WHERE UserName = @username AND Password = @pass";
            SqlCommand cmd = new SqlCommand(selSQL, myConn);
            cmd.Parameters.AddWithValue("@username", txt_name.Text);
            cmd.Parameters.AddWithValue("@pass", txt_pass.Text);
            try
            {
                myConn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Session["fname"] = reader["FirstName"].ToString();
                        Session["lname"] = reader["LastName"].ToString();
                        Session["userid"] = reader["Id"];
                        
                        Response.Redirect("Dashboard.aspx");
                    }

                }
                else
                {
                    lbl_res.Text = "Username and Password do not match";
                }
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