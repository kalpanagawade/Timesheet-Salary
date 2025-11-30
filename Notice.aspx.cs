using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace EmployeeTimesheet_Salary
{
    public partial class Notice : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;


        protected void Page_Load(object sender, EventArgs e)
        {
            //txtCreatedBy.Text = Request.QueryString["Username"];
            //    Get_moduleName();
            if (!IsPostBack) // Ensure it runs only on the first page load
            {
                LoadDashboardCounts();
                LoadNotices();
                CurrentMonth = DateTime.Now.Month;
                CurrentYear = DateTime.Now.Year;
                BindHolidayData();

            }
        }

        private void LoadDashboardCounts()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Total employees
                SqlCommand cmd1 = new SqlCommand("SELECT COUNT(*) FROM iuser", con);
                lblTotalEmployees.Text = cmd1.ExecuteScalar().ToString();

                // Pending timesheets
                SqlCommand cmd2 = new SqlCommand("SELECT COUNT(*) FROM TaskEntries WHERE IsSendForApproval='Y'", con);
                lblPendingTimesheets.Text = cmd2.ExecuteScalar().ToString();

                // Approvals
                SqlCommand cmd3 = new SqlCommand("SELECT COUNT(*) FROM TaskEntries WHERE IsApproval='Y'", con);
                lblApprovals.Text = cmd3.ExecuteScalar().ToString();

                // Payroll pending
                SqlCommand cmd4 = new SqlCommand("SELECT COUNT(*) FROM Salary", con);
                //("SELECT COUNT(*) FROM Payroll WHERE Status='Pending'", con);
                lblPayrollItems.Text = cmd4.ExecuteScalar().ToString();

                con.Close();
            }
        }

        public int CurrentMonth
        {
            get { return (int)(ViewState["CurrentMonth"] ?? DateTime.Now.Month); }
            set { ViewState["CurrentMonth"] = value; }
        }
        public int CurrentYear
        {
            get { return (int)(ViewState["CurrentYear"] ?? DateTime.Now.Year); }
            set { ViewState["CurrentYear"] = value; }
        }

        private void BindHolidayData()
        {
            LblMonth.Text = new DateTime(CurrentYear, CurrentMonth, 1).ToString("MMMM yyyy");

            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT HolidayDate, HolidayName
                         FROM EmployeeHolidays
                         WHERE MONTH(HolidayDate) = @Month AND YEAR(HolidayDate) = @Year
                         ORDER BY HolidayDate";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                da.SelectCommand.Parameters.AddWithValue("@Month", CurrentMonth);
                da.SelectCommand.Parameters.AddWithValue("@Year", CurrentYear);

                DataTable dt = new DataTable();
                da.Fill(dt);

                gvHoliday.DataSource = dt;
                gvHoliday.DataBind();
            }
        }

        protected void btnPrev_Click(object sender, EventArgs e)
        {
            CurrentMonth--;
            if (CurrentMonth < 1)
            {
                CurrentMonth = 12;
                CurrentYear--;
            }
            BindHolidayData();
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            CurrentMonth++;
            if (CurrentMonth > 12)
            {
                CurrentMonth = 1;
                CurrentYear++;
            }
            BindHolidayData();
        }

        private void LoadNotices()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT TOP 10 NoticeText, CreatedDate 
                         FROM NoticeBoard 
                         ORDER BY CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                lstNotice.Items.Clear();
                while (dr.Read())
                {
                    lstNotice.Items.Add($"{dr["NoticeText"]}  ({Convert.ToDateTime(dr["CreatedDate"]).ToString("dd-MMM-yyyy")})");
                }
                dr.Close();
            }
        }

        protected void btnAddNotice_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNotice.Text))
            {
                string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "INSERT INTO NoticeBoard (NoticeText) VALUES (@Notice)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Notice", txtNotice.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                txtNotice.Text = "";
                LoadNotices();
            }
        }


    }
}

