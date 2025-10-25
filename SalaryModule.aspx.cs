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
                BindEmployeeList();
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

                lblUser.Text = $"Employee: {username} (UserID: {userId})";
                pnlDetails.Visible = true;

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
                    txtAnnual.Text = dr["AnnualIncome"].ToString();
                    txtBasic.Text = dr["BasicSalary"].ToString();
                    txtHRA.Text = dr["HRA"].ToString();
                    txtAllowance.Text = dr["Allowance"].ToString();
                    txtDeduction.Text = dr["Deductions"].ToString();
                    lblNetSalary.Text = "Net Salary: ₹ " + dr["NetSalary"].ToString();
                }
                else
                {
                    txtAnnual.Text = txtBasic.Text = txtHRA.Text = txtAllowance.Text = txtDeduction.Text = "";
                    lblNetSalary.Text = "No salary data found.";
                }
            }
        }

    }
}