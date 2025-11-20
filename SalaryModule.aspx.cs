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
                    LoadMonth();
                    LoadYear();
                    gvTasks.RowDataBound += gvTasks_RowDataBound;
                }
            }
        }
        private void LoadMonth()
        {
            ddlMonth.Items.Clear();
            ddlMonth.Items.Add(new ListItem("-- Select Month --", ""));

            ddlMonth.Items.Add(new ListItem("January", "1"));
            ddlMonth.Items.Add(new ListItem("February", "2"));
            ddlMonth.Items.Add(new ListItem("March", "3"));
            ddlMonth.Items.Add(new ListItem("April", "4"));
            ddlMonth.Items.Add(new ListItem("May", "5"));
            ddlMonth.Items.Add(new ListItem("June", "6"));
            ddlMonth.Items.Add(new ListItem("July", "7"));
            ddlMonth.Items.Add(new ListItem("August", "8"));
            ddlMonth.Items.Add(new ListItem("September", "9"));
            ddlMonth.Items.Add(new ListItem("October", "10"));
            ddlMonth.Items.Add(new ListItem("November", "11"));
            ddlMonth.Items.Add(new ListItem("December", "12"));
        }

        private void LoadYear()
        {
            ddlYear.Items.Clear();
            ddlYear.Items.Add(new ListItem("-- Select Year --", ""));

            int currentYear = DateTime.Now.Year;
            for (int yr = 2000; yr <= currentYear + 1; yr++)
            {
                ddlYear.Items.Add(new ListItem(yr.ToString(), yr.ToString()));
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
                ViewState["SelectedUserId"] = userId;
                lblUser.Text = $"Employee: {username} (UserID: {userId})";
                //pnlDetails.Visible = true;
                SalMdlDiv.Style["display"] = "none";

                // Show Detail Salary Div
                DtlSalDiv.Style["display"] = "block";

                pnlDetails.Visible = true;

                ViewState["ShowDetails"] = true;
                // 🔹🔹 RESET PREVIOUS DATA (IMPORTANT FIX)
                gvTasks.DataSource = null;
                gvTasks.DataBind();

                txtDeduction.Text = "";
                ViewState["LeaveDays"] = null;

                ddlMonth.ClearSelection();
                ddlYear.ClearSelection();
                // 🔹🔹 End Reset
                //BindTaskDetails(userId);
                BindSalaryDetails(userId);
            }
        }

        //wip nOT wORKING check.gvTasks_RowDataBound
        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string status = DataBinder.Eval(e.Row.DataItem, "Status").ToString();

                switch (status)
                {
                    case "Holiday":
                        e.Row.Attributes["style"] = "background-color:#ffcccb !important;"; // Light Red
                        break;

                    case "Leave":
                        e.Row.Attributes["style"] = "background-color:#ffff99 !important;"; // Light Yellow
                        break;

                    case "Weekly Off":
                        e.Row.Attributes["style"] = "background-color:#add8e6 !important;"; // Light Blue
                        break;

                    case "Comp Off":
                        e.Row.Attributes["style"] = "background-color:lightgreen !important;";
                        break;
                }
            }
        }



        private void BindTaskDetails(string userId, int month, int year)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("GetTaskSummaryWithTotal", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvTasks.DataSource = dt;
                gvTasks.DataBind();
                int leaveCount = 0;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Status"].ToString() == "Leave")
                    {
                        int.TryParse(row["Total Days"].ToString(), out leaveCount);
                        break; // stop loop once found
                    }
                }

                ViewState["LeaveDays"] = leaveCount;

                // Call to calculate deduction
                CalculateDeduction();
            }
        }

        private void CalculateDeduction()
        {
            if (ViewState["SelectedUserId"] == null || ViewState["LeaveDays"] == null)
                return;

            // Salary from screen
            decimal monthSalary = 0;
            decimal.TryParse(lblMonSalary.Text.Replace("Month Salary: ₹ ", "").Trim(), out monthSalary);

            // Get month and year from dropdowns
            int month = Convert.ToInt32(ddlMonth.SelectedValue);
            int year = Convert.ToInt32(ddlYear.SelectedValue);

            // Count leave days
            int leaveDays = Convert.ToInt32(ViewState["LeaveDays"]);

            // Get days in month
            int totalDays = DateTime.DaysInMonth(year, month);

            // Formula
            decimal deduction = (monthSalary / totalDays) * leaveDays;

            // Show result
            txtDeduction.Text = deduction.ToString("0.00");
        }



        //protected void FilterChanged(object sender, EventArgs e)
        //{
        //    if (ViewState["SelectedUserId"] == null)
        //        return; // prevents crash if no employee selected first

        //    if (!string.IsNullOrEmpty(ddlMonth.SelectedValue) && !string.IsNullOrEmpty(ddlYear.SelectedValue))
        //    {
        //        string userId = ViewState["SelectedUserId"].ToString();
        //        int selectedMonth = Convert.ToInt32(ddlMonth.SelectedValue);
        //        int selectedYear = Convert.ToInt32(ddlYear.SelectedValue);

        //        BindTaskDetails(userId, selectedMonth, selectedYear);
        //    }
        //}

        protected void FilterChanged(object sender, EventArgs e)
        {
            if (ViewState["SelectedUserId"] == null)
                return;

            if (!string.IsNullOrEmpty(ddlMonth.SelectedValue) && !string.IsNullOrEmpty(ddlYear.SelectedValue))
            {
                string userId = ViewState["SelectedUserId"].ToString();
                int selectedMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                int selectedYear = Convert.ToInt32(ddlYear.SelectedValue);

                BindTaskDetails(userId, selectedMonth, selectedYear);

                // After salary details loaded once, no need to reload every time
                //Deduction will auto calculate inside BindTaskDetails()
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