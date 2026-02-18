using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Services;

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

                      
                        if (string.IsNullOrWhiteSpace(txtUsername.Text) ||
                            string.IsNullOrWhiteSpace(txtPassword.Text))
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "empty",
                                "alert('Username and Password cannot be empty!');", true);
                            return;
                        }

                        if (count > 0)
                        {
                            string queryA = "SELECT UserLoginName FROM iUser WHERE Userid = @Userid";
                            using (SqlCommand cmdA = new SqlCommand(queryA, conn))
                            {
                                cmdA.Parameters.AddWithValue("@Userid", txtUsername.Text.Trim());

                                string name = Convert.ToString(cmdA.ExecuteScalar());

                                string url = "Notice.aspx?Username=" + Server.UrlEncode(name) +
                                             "&UserID=" + Server.UrlEncode(txtUsername.Text.Trim());

                                // ✅ T001 – Valid Login
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
                                    "alert('User logged in successfully!'); window.location='" + url + "';", true);
                            }
                        }
                        else
                        {
                            // ✅ T002 & T003 – Invalid Username or Password
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
                                "alert('Login ID or Password is incorrect');", true);
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

        public static string generatedOTP = "";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void SendOTP(string emailOrPhone)
        {
            Random rnd = new Random();
            generatedOTP = rnd.Next(100000, 999999).ToString();




            // Send Email or SMS here
            // Example email code:
            // EmailService.Send(emailOrPhone, "Your OTP", "Your OTP is " + generatedOTP);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string VerifyOTP(string otp)
        {
            if (otp == generatedOTP)
                return "OK";
            else
                return "FAIL";
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static void UpdatePassword(string newPassword)
        {
            //return "UPDATED";
            // Update password into SQL database
            // Example:
            // UPDATE Users SET Password = @newPassword WHERE Email = @email
        }



    }
}