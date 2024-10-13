using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TaskTrackr_Project
{
    public partial class Task_Edit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            { 
            string parameterValue;
                if (Request.QueryString["Id"] != null)
                {
                    // Retrieve the value of the parameter
                    parameterValue = Request.QueryString["Id"];

                    string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString2"].ConnectionString;
                    SqlConnection myConn = new SqlConnection(connectionstring);
                    string selSQL = "SELECT * FROM TaskList WHERE TaskId=@Id";
                    SqlCommand cmd = new SqlCommand(selSQL, myConn);
                    cmd.Parameters.AddWithValue("@Id", parameterValue);

                    try
                    {
                        myConn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();


                        // Check if the reader has rows
                        if (reader.HasRows)
                        {

                            reader.Read();

                            DateTime dateValue1 = Convert.ToDateTime(reader["StartDate"]);
                            DateTime dateValue2 = Convert.ToDateTime(reader["EndDate"]);
                            DateTime dateValue3 = Convert.ToDateTime(reader["DueDate"]);

                            txt_title.Text = reader["TaskTitle"].ToString();
                            txt_desc.Text = reader["Description"].ToString();
                            txt_start.Text = dateValue1.ToString("yyyy-MM-dd");
                            txt_end.Text = dateValue2.ToString("yyyy-MM-dd");
                            txt_due.Text = dateValue3.ToString("yyyy-MM-dd");
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        lbl_res.Text = ex.Message;
                    }
                    finally
                    {
                        myConn.Close();
                    }
                   
                }
                
            }
        }

        protected void btn_edit_Click(object sender, EventArgs e)
        {
            string parameterValue;
            if (Request.QueryString["Id"] != null)
            {
                parameterValue = Request.QueryString["Id"];
                string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString2"].ConnectionString;
                string updSQL = "UPDATE TaskList SET TaskTitle=@title,Description=@desc,StartDate=@start,EndDate=@end,DueDate=@due WHERE TaskId=@Id";
                SqlConnection myConn = new SqlConnection(connectionstring);

                SqlCommand cmd = new SqlCommand(updSQL, myConn);
                cmd.Parameters.AddWithValue("@Id", parameterValue);
                cmd.Parameters.AddWithValue("@title", txt_title.Text);
                cmd.Parameters.AddWithValue("@desc", txt_desc.Text);
                cmd.Parameters.AddWithValue("@start", txt_start.Text);
                cmd.Parameters.AddWithValue("@end", txt_end.Text);
                cmd.Parameters.AddWithValue("@due", txt_due.Text);

                int updated = 0;
                try
                {
                    myConn.Open();
                    updated = cmd.ExecuteNonQuery();
                    lbl_res.Text = txt_title.Text;
                    //Response.Redirect("Dashboard.aspx");
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
            Response.Redirect("Dashboard.aspx");
        }
    }
}