using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace EmployeeTimesheet_Salary
{
    public partial class SalaryModule : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        string selectedUserId = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindEmployeeList();
            }
            else
            {
                // 👇 Restore div visibility after postback
                if (ViewState["ShowDetails"] != null && (bool)ViewState["ShowDetails"])
                {
                    SalMdlDiv.Style["display"] = "none";
                    DtlSalDiv.Style["display"] = "block";
                }
                else
                {
                    SalMdlDiv.Style["display"] = "block";
                    DtlSalDiv.Style["display"] = "none";
                }
            }
        }

        // 1️⃣ Bind All Employees Initially or Search Results
        private void BindEmployeeList(string empId = "", string empName = "")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT UserId, UserLoginName, UserEmailId 
                                 FROM kalpana..iUser 
                                 WHERE (@UserId = '' OR UserId LIKE '%' + @UserId + '%')
                                   AND (@UserName = '' OR UserLoginName LIKE '%' + @UserName + '%')
                                 ORDER BY UserId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", empId);
                cmd.Parameters.AddWithValue("@UserName", empName);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvEmployees.DataSource = dt;
                gvEmployees.DataBind();
            }
        }

        // 2️⃣ Pagination
        protected void gvEmployees_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvEmployees.PageIndex = e.NewPageIndex;
            BindEmployeeList(txtSearchId.Text.Trim(), txtSearchName.Text.Trim());
        }

        // 3️⃣ Search Button
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEmployeeList(txtSearchId.Text.Trim(), txtSearchName.Text.Trim());
            pnlDetails.Visible = false;
        }

        // 4️⃣ Clear Button
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchId.Text = "";
            txtSearchName.Text = "";
            BindEmployeeList();
            pnlDetails.Visible = false;
        }

        // 5️⃣ When Click on User ID
        protected void gvEmployees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowDetails")
            {
                string[] args = e.CommandArgument.ToString().Split('|');
                string userId = args[0];
                string username = args[1];
                lblUser.Attributes["data-userid"] = userId;

                lblUser.Text = $"Employee: {username} (UserID: {userId})";
                //pnlDetails.Visible = true;
                SalMdlDiv.Style["display"] = "none";

                // Show Detail Salary Div
                DtlSalDiv.Style["display"] = "block";

                pnlDetails.Visible = true;

                ViewState["ShowDetails"] = true;

                BindTaskDetails(userId);
                BindSalaryDetails(userId);
            }
        }

        // 6️⃣ Approved Task Details
        private void BindTaskDetails(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT TaskDate, Description, TimeSpent, Type,
                                        IsHoliday, IsLeave, IsWeeklyOff, IsCompOff, IsApproval
                                 FROM kalpana..TaskEntries
                                 WHERE UserId = @UserId AND IsApproval = 'Y'
                                 ORDER BY TaskDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTasks.DataSource = dt;
                gvTasks.DataBind();
            }
        }

        // 7️⃣ Salary Details
        private void BindSalaryDetails(string userId)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = @"SELECT TOP 1 * FROM kalpana..Salary WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtBonus.Text = dr["Bonus"].ToString();
                    txtBasic.Text = dr["BasicSalary"].ToString();
                    txtHRA.Text = dr["HRA"].ToString();
                    txtAllowance.Text = dr["Allowance"].ToString();
                    txtDeduction.Text = dr["Deductions"].ToString();
                    lblNetSalary.Text = "Net Salary: ₹ " + dr["MonthlyNetSalary"].ToString();
                    lblMonSalary.Text = "Month Salary: ₹ " + dr["MonthlySalary"].ToString();
                    lblAnnual.Text = "Annual Income : ₹ " + dr["AnnualIncome"].ToString();
                }
                else
                {
                    txtBonus.Text = txtBasic.Text = txtHRA.Text = txtAllowance.Text = txtDeduction.Text = "";
                    lblNetSalary.Text = "No Net salary data found.";
                    lblMonSalary.Text = "No Month salary data found.";
                    lblAnnual.Text= "No Annual Income data found.";
                }
            }
        }

        protected void btnSaveSalary_Click(object sender, EventArgs e)
        {
            string userId = lblUser.Attributes["data-userid"];
            //Request.QueryString["UserID"];
            if (string.IsNullOrEmpty(userId))
            {
                lblMessage.Text = "⚠️ Please select an employee first.";
                return;
            }

            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("PRC_SaveSalary", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Sample values — replace with TextBox inputs if needed
                cmd.Parameters.AddWithValue("@UserId", userId);
                if (!string.IsNullOrEmpty(txtBonus.Text))
                {
                    cmd.Parameters.AddWithValue("@Bonus", txtBonus.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Bonus", 0.00m);
                }
                cmd.Parameters.AddWithValue("@BasicSalary", txtBasic.Text);
                cmd.Parameters.AddWithValue("@HRA", txtHRA.Text);
                cmd.Parameters.AddWithValue("@Allowance", txtAllowance.Text);
                cmd.Parameters.AddWithValue("@Deductions", txtDeduction.Text);

                conn.Open();
                int rows = cmd.ExecuteNonQuery();
                conn.Close();

                lblMessage.Text = "✅ Salary saved successfully!";
                BindSalaryDetails(userId);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            SalMdlDiv.Style["display"] = "block";
            DtlSalDiv.Style["display"] = "none";
            pnlDetails.Visible = false;   // ✅ hide again
            ViewState["ShowDetails"] = false;
        }

    }
}