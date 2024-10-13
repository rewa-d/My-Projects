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
    public partial class Dashboard : System.Web.UI.Page
    {
        int counter;
        int rowCount=0;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
                string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString2"].ConnectionString;
                SqlConnection myConn = new SqlConnection(connectionstring);
                string selSQL = "SELECT * FROM TaskList where userid=@id";
                SqlCommand cmd = new SqlCommand(selSQL, myConn);
            cmd.Parameters.AddWithValue("@id", Session["userid"].ToString());
            
                try
                {

                    myConn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    if (reader != null)
                    {
                    TableRow row = new TableRow();
                    TableHeaderRow headerRow = new TableHeaderRow();

                    // Define header cells
                    TableHeaderCell headerCell1 = new TableHeaderCell();
                    headerCell1.Text = "Sr. No.";
                    headerRow.Cells.Add(headerCell1);

                    TableHeaderCell headerCell2 = new TableHeaderCell();
                    headerCell2.Text = "Title";
                    headerRow.Cells.Add(headerCell2);

                    TableHeaderCell headerCell3 = new TableHeaderCell();
                    headerCell3.Text = "Description";
                    headerRow.Cells.Add(headerCell3);

                    TableHeaderCell headerCell4 = new TableHeaderCell();
                    headerCell4.Text = "Start Date";
                    headerRow.Cells.Add(headerCell4);

                    TableHeaderCell headerCell5 = new TableHeaderCell();
                    headerCell5.Text = "End Date";
                    headerRow.Cells.Add(headerCell5);

                    TableHeaderCell headerCell6 = new TableHeaderCell();
                    headerCell6.Text = "Due Date";
                    headerRow.Cells.Add(headerCell6);

                    TableHeaderCell headerCell7 = new TableHeaderCell();
                    headerCell7.Text = "Status";
                    headerRow.Cells.Add(headerCell7);

                    TableHeaderCell headerCell8 = new TableHeaderCell();
                    headerCell8.Text = "Action";
                    headerCell8.ColumnSpan = 3;
                    headerCell8.CssClass = "text-center";
                    headerRow.Cells.Add(headerCell8);

                    table_view.Rows.Add(headerRow);
                    while (reader.Read())
                        {
                            rowCount++;
                            TableRow row1 = new TableRow();


                            TableCell cell1 = new TableCell();
                            cell1.Text = rowCount.ToString();
                            row1.Cells.Add(cell1);

                            TableCell cell2 = new TableCell();
                            cell2.Text = reader["TaskTitle"].ToString();
                            row1.Cells.Add(cell2);

                            TableCell cell3 = new TableCell();
                            cell3.Text = reader["Description"].ToString();
                            row1.Cells.Add(cell3);

                            TableCell cell4 = new TableCell();
                            cell4.Text = reader["StartDate"].ToString();
                            row1.Cells.Add(cell4);

                            TableCell cell5 = new TableCell();
                            cell5.Text = reader["EndDate"].ToString();
                            row1.Cells.Add(cell5);

                            TableCell cell6 = new TableCell();
                            cell6.Text = reader["DueDate"].ToString();
                            row1.Cells.Add(cell6);

                            TableCell cell7 = new TableCell();
                            cell7.Text = reader["Status"].ToString();
                            row1.Cells.Add(cell7);

                            TableCell cell8 = new TableCell();
                            Button button1 = new Button();
                            button1.ID = "btnRow_i" + rowCount; // Unique ID for each button
                            button1.Text = "Edit";
                            button1.CommandArgument = reader["TaskId"].ToString();
                            button1.Command += new CommandEventHandler(btn_edit); // Attach a Click event handler
                        button1.CssClass = "btn btn-success mx-3";
                        cell8.Controls.Add(button1);
                            row1.Cells.Add(cell8);

                            TableCell cell9 = new TableCell();
                            Button button2 = new Button();
                            button2.ID = "btnRow_e" + rowCount; // Unique ID for each button
                            button2.Text = "Delete";
                            button2.CommandArgument = reader["TaskId"].ToString();
                            button2.Command += new CommandEventHandler(btn_del); // Attach a Click event handler
                            button2.CssClass = "btn btn-danger mx-3";
                            cell8.Controls.Add(button2);
                            row1.Cells.Add(cell9);

                            TableCell cell10 = new TableCell();
                            Button button3 = new Button();
                            button3.ID = "btnRow_m" + rowCount; // Unique ID for each button
                            button3.Text = "Mark As Done";
                            button3.CommandArgument = reader["TaskId"].ToString();
                            button3.Command += new CommandEventHandler(btn_mkdone); // Attach a Click event handler
                            button3.CssClass = "btn btn-primary mx-3";
                            cell8.Controls.Add(button3);
                            row1.Cells.Add(cell10);
                          
                        // Add the row to the table
                        table_view.Rows.Add(row1);
                        }
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
        
               


        protected void btn_add_Click1(object sender, EventArgs e)
        {
            Response.Redirect("Task_Add.aspx");
        }

        protected void btn_mkdone(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            
            string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString2"].ConnectionString;
            SqlConnection myConn = new SqlConnection(connectionstring);
            string updSQL = "UPDATE TaskList SET Status='Completed' WHERE TaskId=@id";
            Console.WriteLine(updSQL);
            SqlCommand cmd = new SqlCommand(updSQL, myConn);
            cmd.Parameters.AddWithValue("@id", id);
            int updated = 0;
            try
            {
                myConn.Open();
                updated = cmd.ExecuteNonQuery();
                lbl_res.Text = updated.ToString() + " record updated";
            }
            catch (Exception err)
            {
                lbl_res.Text = err.Message;
            }
            finally
            {
                myConn.Close();
            }
            Response.Redirect("Dashboard.aspx");
        }

        protected void btn_del(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);

            string connectionstring = WebConfigurationManager.ConnectionStrings["InfoUserConnectionString2"].ConnectionString;
            SqlConnection myConn = new SqlConnection(connectionstring);
            string delSQL = "DELETE FROM TaskList WHERE TaskId=@id";
            
            SqlCommand cmd = new SqlCommand(delSQL, myConn);
            cmd.Parameters.AddWithValue("@id", id);
            int deleted = 0;
            try
            {
                myConn.Open();
                deleted = cmd.ExecuteNonQuery();
                
            }
            catch (Exception err)
            {
                lbl_res.Text = err.Message;
            }
            finally
            {
                myConn.Close();
            }
            Response.Redirect("Dashboard.aspx");
        }

        protected void btn_edit(object sender, CommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            Response.Redirect("Task_Edit.aspx?Id=" + id);
        }
    }
}