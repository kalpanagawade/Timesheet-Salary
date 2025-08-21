using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;

namespace EmployeeTimesheet_Salary
{
    public partial class Notice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCreatedBy.Text = Request.QueryString["Username"];
                Get_moduleName();
            if (!IsPostBack) // Ensure it runs only on the first page load
            {

            }
        }

        private void Get_moduleName()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("PRCGET_UserLogin_N_Page_Name", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserId", Request.QueryString["UserID"]);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string pageName = reader["Page_name"].ToString().Trim();

                                if (pageName.Equals("CreateUpdateUser.aspx", StringComparison.OrdinalIgnoreCase))
                                {
                                    Button btnEmpDts = new Button();
                                    btnEmpDts.ID = "btnEmpDts";
                                    btnEmpDts.Text = "Employee Details";
                                    btnEmpDts.CssClass = "home-active";
                                    btnEmpDts.Click += new EventHandler(btnEmpDts_Click);
                                    phDynamicButtons.Controls.Add(btnEmpDts);
                                }
                                else if (pageName.Equals("timesheet.aspx", StringComparison.OrdinalIgnoreCase))
                                {
                                    Button btnEmpTimSht = new Button();
                                    btnEmpTimSht.ID = "btnEmpTimSht";
                                    btnEmpTimSht.Text = "Employee Timesheet";
                                    btnEmpTimSht.CssClass = "home-active";
                                    btnEmpTimSht.Click += new EventHandler(btnEmpTimSht_Click);
                                    phDynamicButtons.Controls.Add(btnEmpTimSht);
                                }
                                else if (pageName.Equals("TeamTimesheet.aspx", StringComparison.OrdinalIgnoreCase))
                                {
                                    Button btnTeamTimSht = new Button();
                                    btnTeamTimSht.ID = "btnTeamTimSht";
                                    btnTeamTimSht.Text = "Team Timesheet";
                                    btnTeamTimSht.CssClass = "home-active";
                                    btnTeamTimSht.Click += new EventHandler(btnTeamTimSht_Click);
                                    phDynamicButtons.Controls.Add(btnTeamTimSht);
                                }

                                // Add more cases if you have more page names
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "Get_moduleName");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        logCmd.ExecuteNonQuery();
                    }

                  
                }
            }
        }

        protected void btnTeamTimSht_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            try
            {
                // Retrieve UserID from query string
                string userId = Request.QueryString["UserID"];

                // Optional: Validate userId (null/empty)
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("UserID is missing in query string.");

                // Redirect to CreateUpdateUser.aspx
                Response.Redirect("TeamTimesheet.aspx?UserID=" + Server.UrlEncode(userId) +
                                  "&Username=" + Server.UrlEncode(txtCreatedBy.Text.Trim()), false);
            }
            catch (Exception ex)
            {
                // Log the error using a SqlConnection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnTeamTimSht_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        conn.Open();
                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        protected void btnEmpDts_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            try
            {
                // Retrieve UserID from query string
                string userId = Request.QueryString["UserID"];

                // Optional: Validate userId (null/empty)
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("UserID is missing in query string.");

                // Redirect to CreateUpdateUser.aspx
                Response.Redirect("CreateUpdateUser.aspx?UserID=" + Server.UrlEncode(userId) +
                                  "&Username=" + Server.UrlEncode(txtCreatedBy.Text.Trim()), false);
            }
            catch (Exception ex)
            {
                // Log the error using a SqlConnection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnEmpDts_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        conn.Open();
                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        protected void btnEmpTimSht_Click(object sender, EventArgs e)
                {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            try
            {
                
                // Redirect to timesheet.aspx
                string userId = Request.QueryString["UserID"];

                if (string.IsNullOrEmpty(userId))
                    throw new Exception("UserID is missing in query string.");
                Response.Redirect("timesheet.aspx?UserID=" + Server.UrlEncode(userId) + "&Username=" + Server.UrlEncode(txtCreatedBy.Text.Trim()));

            }
            catch (Exception ex)
            {
                // Log the error using a SqlConnection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", " btnEmpTimSht_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        conn.Open();
                        logCmd.ExecuteNonQuery();
                    }
                }
            }

        }

            }
        }