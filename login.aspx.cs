using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeTimesheet_Salary
{
    public partial class Login : System.Web.UI.Page
    {
        protected global::System.Web.UI.WebControls.TextBox txtUsername;
        protected global::System.Web.UI.WebControls.TextBox txtPassword;


        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnlogin_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT COUNT(*) FROM iUser WHERE Userid = @Userid AND UserPIN = @UserPIN";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Use parameters to prevent SQL Injection
                        cmd.Parameters.AddWithValue("@Userid", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@UserPIN", txtPassword.Text.Trim());

                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            string queryA = "SELECT UserLoginName FROM iUser WHERE Userid = @Userid";
                            using (SqlCommand cmdA = new SqlCommand(queryA, conn))
                            {
                                // Use parameters to prevent SQL Injection
                                cmdA.Parameters.AddWithValue("@Userid", txtUsername.Text.Trim());


                                string name = (string)cmdA.ExecuteScalar();

                                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Successfully Logged In!');", true);
                                Response.Redirect("Notice.aspx?Username=" + Server.UrlEncode(name) + "&UserID=" + Server.UrlEncode(txtUsername.Text.Trim()));

                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Login ID or Password is incorrect!');", true);

                        }
                    }
                }
                catch (Exception ex)
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnlogin_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        if (conn.State != ConnectionState.Open)
                            conn.Open();

                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }


    }
}