using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace EmployeeTimesheet_Salary
{
    public partial class SalaryModule : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindEmployees();
                BindEmployeeList();
                ddlYear.Items.Add("-- Select Year --");
                for (int y = 2020; y <= DateTime.Now.Year + 1; y++)
                    ddlYear.Items.Add(y.ToString());

                ddlMonth.Items.Add("-- Select Month --");
                for (int m = 1; m <= 12; m++)
                    ddlMonth.Items.Add(new ListItem(
                        CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m), m.ToString()));
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            SalMdlDiv.Style["display"] = "block";
            DtlSalDiv.Style["display"] = "none";
            //pnlDetails.Visible = false;   // ✅ hide again
            pnlSalary.Visible = false;
            ViewState["ShowDetails"] = false;
        }
        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchId.Text = "";
            txtSearchName.Text = "";
            BindEmployeeList();
            //pnlDetails.Visible = false;
            pnlSalary.Visible = false;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEmployeeList(txtSearchId.Text.Trim(), txtSearchName.Text.Trim());
            //pnlDetails.Visible = false;
            pnlSalary.Visible = false;
        }

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
        void BindEmployees()
        {
            using (SqlDataAdapter da = new SqlDataAdapter(
                "SELECT UserId,UserLoginName,UserEmailId FROM kalpana..iUser", connStr))
            {
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvEmployees.DataSource = dt;
                gvEmployees.DataBind();
            }
        }

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

        //protected void gvEmployees_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        //{
        //    if (e.CommandName == "ShowDetails")
        //    {
        //        string[] a = e.CommandArgument.ToString().Split('|');
        //        ViewState["SelectedUserId"] = a[0];
        //        lblUser.Text = "Employee: " + a[1] + " (" + a[0] + ")";
        //        pnlSalary.Visible = true;
        //    }
        //}

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

                //pnlDetails.Visible = true;
                pnlSalary.Visible = true;

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
                int Year = 1901;
                BindSalaryDetails(userId, Year);
            }
        }

        private void BindSalaryDetails(string userId, int Year)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                //string query = @"SELECT TOP 1 * FROM kalpana..Salary WHERE UserId = @UserId and Year=@Year";
                string query = @"SELECT S.*,M.Deduction,M.NetSalary as MonthlyNetSalary FROM kalpana..Salary as S left join kalpana..SalaryMonthly as M on M.UserID=S.UserID and M.Year=S.Year WHERE S.UserId = @UserId and S.Year=@Year";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Year", Year);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    txtBonus.Text = dr["Bonus"].ToString();
                    txtBasic.Text = dr["BasicSalary"].ToString();
                    txtHRA.Text = dr["HRA"].ToString();
                    txtAllowance.Text = dr["Allowance"].ToString();
                    txtDeduction.Text = dr["Deduction"].ToString();
                    lblNetSalary.Text = "Net Salary: ₹ " + dr["MonthlyNetSalary"].ToString();
                    lblMonSalary.Text = "Month Salary: ₹ " + dr["MonthlySalary"].ToString();
                    lblAnnual.Text = "Annual Income : ₹ " + dr["AnnualIncome"].ToString();
                }
                else
                {
                    txtBonus.Text = txtBasic.Text = txtHRA.Text = txtAllowance.Text = txtDeduction.Text = "";
                    lblNetSalary.Text = "No Net salary data found.";
                    lblMonSalary.Text = "No Month salary data found.";
                    lblAnnual.Text = "No Annual Income data found.";
                }
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
        protected void gvEmployees_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
        {
            gvEmployees.PageIndex = e.NewPageIndex;
            BindEmployees();
            BindEmployeeList(txtSearchId.Text.Trim(), txtSearchName.Text.Trim());
        }

        //protected void FilterChanged(object sender, EventArgs e)
        //{
        //    bool lockYear = ddlMonth.SelectedIndex > 0;
        //    txtBasic.ReadOnly = txtHRA.ReadOnly =
        //    txtAllowance.ReadOnly = txtBonus.ReadOnly = lockYear;
        //}

        protected void FilterChanged(object sender, EventArgs e)
        {
            if (ViewState["SelectedUserId"] == null)
                return;

            string userId = ViewState["SelectedUserId"].ToString();


            if (!string.IsNullOrEmpty(ddlMonth.SelectedValue) && !string.IsNullOrEmpty(ddlYear.SelectedValue))
            {
                int selectedYear = Convert.ToInt32(ddlYear.SelectedValue);
                int selectedMonth = Convert.ToInt32(ddlMonth.SelectedValue);
                BindTaskDetails(userId, selectedMonth, selectedYear);
                BindSalaryDetails(userId, selectedYear);

                // After salary details loaded once, no need to reload every time
                //Deduction will auto calculate inside BindTaskDetails()
            }

            if (!string.IsNullOrEmpty(ddlYear.SelectedValue))
            {
                int selectedYear = Convert.ToInt32(ddlYear.SelectedValue);
                BindSalaryDetails(userId, selectedYear);
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
                //CalculateDeduction();
            }
        }
        protected void btnSaveSalary_Click(object sender, EventArgs e)
        {
            string userId = lblUser.Attributes["data-userid"];
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("PRC_SaveSalary", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                //cmd.Parameters.AddWithValue("@UserId", ViewState["SelectedUserId"]);
                cmd.Parameters.AddWithValue("@Year", ddlYear.SelectedValue);
                cmd.Parameters.AddWithValue("@BasicSalary", txtBasic.Text);
                cmd.Parameters.AddWithValue("@HRA", txtHRA.Text);
                cmd.Parameters.AddWithValue("@Allowance", txtAllowance.Text);
                //cmd.Parameters.AddWithValue("@Bonus", txtBonus.Text);
                if (!string.IsNullOrEmpty(txtBonus.Text))
                {
                    cmd.Parameters.AddWithValue("@Bonus", txtBonus.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Bonus", 0.00m);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            lblMessage.Text = "Yearly salary saved";
        }

        protected void btnReleaseSalary_Click(object sender, EventArgs e)
        {
            int leaveDays = 2;
            decimal deduction = 500;
            txtDeduction.Text = deduction.ToString("0.00");

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("PRC_ReleaseMonthlySalary", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", ViewState["SelectedUserId"]);
                cmd.Parameters.AddWithValue("@Year", ddlYear.SelectedValue);
                cmd.Parameters.AddWithValue("@Month", ddlMonth.SelectedValue);
                cmd.Parameters.AddWithValue("@LeaveDays", leaveDays);
                cmd.Parameters.AddWithValue("@Deduction", deduction);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            lblMessage.Text = "Salary released";
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

    }
}