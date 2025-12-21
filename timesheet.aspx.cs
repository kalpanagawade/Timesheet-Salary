using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Globalization;
using Antlr.Runtime.Misc;

namespace EmployeeTimesheet_Salary
{

    public partial class timesheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //txtCreatedBy.Text = Request.QueryString["Username"];
           
            if (!IsPostBack)
            {
                ddlWorkPlace.SelectedValue = "Sel";   // or "Office"

                // Enable login button
                btnLoginLogout.Enabled = true;
                btnLoginLogout.CssClass = "btn btn-primary";
                txtTimeSpent.Text = "00:00"; // Default value
                ViewState["IsLoggedIn"] = false; // Default state
                DateTime today = DateTime.Today;
                hfMonthYear.Value = today.ToString("yyyy-MM-dd");
                string UserId= Request.QueryString["UserID"];
                GenerateCalendar(today, UserId);
                //txtInTime.Text = DateTime.Now.ToString("HH:mm:ss");
                //txtOutTime.Text = DateTime.Now.ToString("HH:mm:ss");
                BindGrid();
                //Btn_Tsave.Enabled = false;

                if (HasOpenLogin(UserId))
                {
                    btnLoginLogout.Text = "Logout"; // already logged in
                }
                else
                {
                    // check if already logged out today
                    if (HasLoggedOutToday(UserId))
                    {
                        btnLoginLogout.Enabled = false; // already logged out today
                    }
                    else
                    {
                        btnLoginLogout.Text = "Login"; // fresh login
                    }
                }

                DataRow log = GetTodayLog(UserId);
                if (log != null)
                {
                    if (log["In_Time"] != DBNull.Value)
                        txtInTime.Text = Convert.ToDateTime(log["In_Time"]).ToString("HH:mm:ss");

                    if (log["Out_Time"] != DBNull.Value)
                        txtOutTime.Text = Convert.ToDateTime(log["Out_Time"]).ToString("HH:mm:ss");

                    //if (log["Out_Time"] == DBNull.Value)
                    //    btnLoginLogout.Text = "Logout"; // still logged in
                    //else
                    //    btnLoginLogout.Enabled = false; // already logged out
                }
                //else
                //{
                //    btnLoginLogout.Text = "Login"; // no record for today
                //}
            }


        }
        protected void btnTriggerBindGrid_Click(object sender, EventArgs e)
        {
            BindGrid(); // your method to bind data to GridView etc.

            ScriptManager.RegisterStartupScript(this, GetType(), "ReopenModal", "$('#taskModal').modal('show');", true);

        }
        //protected void btnLoginLogout_Click(object sender, EventArgs e)
        //{
        //    bool isLoggedIn = ViewState["IsLoggedIn"] != null && (bool)ViewState["IsLoggedIn"];

        //    if (!isLoggedIn)
        //    {
        //        // Logging in
        //        txtInTime.Text = DateTime.Now.ToString("HH:mm:ss");
        //        btnLoginLogout.Text = "Logout";
        //        ViewState["IsLoggedIn"] = true;
        //    }
        //    else
        //    {
        //        // Logging out
        //        txtOutTime.Text = DateTime.Now.ToString("HH:mm:ss");
        //        btnLoginLogout.Enabled = false;
        //    }
        //}
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            DateTime current = DateTime.Parse(hfMonthYear.Value);
            DateTime prevMonth = current.AddMonths(-1);
            hfMonthYear.Value = prevMonth.ToString("yyyy-MM-dd");
            string UserId = Request.QueryString["UserID"];
            GenerateCalendar(prevMonth, UserId);
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            DateTime current = DateTime.Parse(hfMonthYear.Value);
            DateTime nextMonth = current.AddMonths(1);
            hfMonthYear.Value = nextMonth.ToString("yyyy-MM-dd");
            string UserId = Request.QueryString["UserID"];
            GenerateCalendar(nextMonth, UserId);
        }

        private Dictionary<int, string> GetStatusData(int year, int month, string userId)
        {
            Dictionary<int, string> statusData = new Dictionary<int, string>();
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_GetCalendarStatus", conn))
                {
                    try
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Year", year);
                        cmd.Parameters.AddWithValue("@Month", month);
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int day = reader.GetInt32(0);
                                string status = reader.GetString(1);
                                statusData[day] = status;
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                        {
                            logCmd.CommandType = CommandType.StoredProcedure;
                            logCmd.Parameters.AddWithValue("@MethodName", " GetStatusData");
                            logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                            logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                            logCmd.ExecuteNonQuery();
                        }

                    }

                }

                return statusData;
            }
        }

        private Dictionary<DateTime, (string InTime, string OutTime)> GetAttendanceData(int year, int month, string userId)
        {
            var result = new Dictionary<DateTime, (string, string)>();
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT LogDate, In_Time, Out_Time
            FROM UserLogTimes
            WHERE UserId = @UserId
              AND YEAR(LogDate) = @Year
              AND MONTH(LogDate) = @Month", conn))
                {
                    cmd.Parameters.AddWithValue("@Year", year);
                    cmd.Parameters.AddWithValue("@Month", month);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // ✅ Dictionary key = Date only (remove time)
                            DateTime logDate = Convert.ToDateTime(dr["LogDate"]).Date;

                            string inTime = dr["In_Time"] != DBNull.Value ? Convert.ToDateTime(dr["In_Time"]).ToString("HH:mm:ss") : "00:00:00";
                            string outTime = dr["Out_Time"] != DBNull.Value ? Convert.ToDateTime(dr["Out_Time"]).ToString("HH:mm:ss") : "00:00:00";

                            result[logDate] = (inTime, outTime);
                        }
                    }
                }
            }

            return result;
        }


        private void GenerateCalendar(DateTime startDate, string userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            try
            {
                lblMonthYear.Text = startDate.ToString("MMMM yyyy");

                int daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                DateTime firstDay = new DateTime(startDate.Year, startDate.Month, 1);
                int startDayOfWeek = (int)firstDay.DayOfWeek;

                // ✅ Get attendance data (already fixed in GetAttendanceData with .Date)
                Dictionary<DateTime, (string InTime, string OutTime)> attendanceData =
                    GetAttendanceData(startDate.Year, startDate.Month, userId);

                // 🔎 DEBUG: dump dictionary values to Output window
                foreach (var kvp in attendanceData)
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"[Key={kvp.Key:dd-MM-yyyy HH:mm:ss}, Value=({kvp.Value.InTime}, {kvp.Value.OutTime})]");
                }

                // ✅ Get status data
                Dictionary<int, string> statusData =
                    GetStatusData(startDate.Year, startDate.Month, userId);

                int day = 1;
                string html = "";

                for (int week = 0; week < 6; week++)
                {
                    html += "<tr>";
                    for (int weekday = 0; weekday < 7; weekday++)
                    {
                        if ((week == 0 && weekday < startDayOfWeek) || day > daysInMonth)
                        {
                            html += "<td></td>";
                        }
                        else
                        {
                            string cssClass = "";
                            string content = "";

                            if (statusData.ContainsKey(day))
                            {
                                content = statusData[day];
                                if (content == "Pending") cssClass = "pending";
                                else if (content == "Weekly Off") cssClass = "weeklyoff";
                                else if (content == "In your bucket") cssClass = "bucket";
                                else if (content == "Holiday") cssClass = "holiday";
                                else if (content == "Approved") cssClass = "approved";
                                else if (content == "NotApproved") cssClass = "NotApproved";
                                else if (content == "Leave") cssClass = "leave";
                                else if (content == "CompOff") cssClass = "compoff";
                            }

                            DateTime cellDate = new DateTime(startDate.Year, startDate.Month, day).Date;

                            if (cellDate <= DateTime.Today)
                            {
                                // ✅ lookup using dictionary
                                string inTime = attendanceData.ContainsKey(cellDate) ? attendanceData[cellDate].InTime : "00:00:00";
                                string outTime = attendanceData.ContainsKey(cellDate) ? attendanceData[cellDate].OutTime : "00:00:00";

                                html += $"<td class='{cssClass}' onclick=\"openTaskModal('{cellDate:yyyy-MM-dd}', '{inTime}', '{outTime}')\">" +
                                        $"<strong>{day}</strong><br />{content}</td>";
                            }
                            else
                            {
                                html += $"<td class='{cssClass}'>" +
                                        $"<strong>{day}</strong><br />{content}</td>";
                            }

                            day++;
                        }
                    }
                    html += "</tr>";

                    if (day > daysInMonth)
                        break;
                }

                litCalendar.Text = html;
            }
            catch (Exception ex)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "GenerateCalendar");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }




        //private void GenerateCalendar(DateTime startDate, string userId)
        //{
        //    string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        //    try
        //    {

        //        lblMonthYear.Text = startDate.ToString("MMMM yyyy");

        //        int daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
        //        DateTime firstDay = new DateTime(startDate.Year, startDate.Month, 1);
        //        int startDayOfWeek = (int)firstDay.DayOfWeek;

        //        Dictionary<DateTime, (string InTime, string OutTime)> attendanceData = GetAttendanceData(startDate.Year, startDate.Month, userId);

        //        Dictionary<int, string> statusData = GetStatusData(startDate.Year, startDate.Month, userId);

        //        int day = 1;
        //        string html = "";

        //        for (int week = 0; week < 6; week++)
        //        {
        //            html += "<tr>";
        //            for (int weekday = 0; weekday < 7; weekday++)
        //            {
        //                if ((week == 0 && weekday < startDayOfWeek) || day > daysInMonth)
        //                {
        //                    html += "<td></td>";
        //                }
        //                else
        //                {
        //                    string cssClass = "";
        //                    string content = "";

        //                    if (statusData.ContainsKey(day))
        //                    {
        //                        content = statusData[day];
        //                        if (content == "Pending") cssClass = "pending";
        //                        else if (content == "Weekly Off") cssClass = "weeklyoff";
        //                        else if (content == "In your bucket") cssClass = "bucket";
        //                        else if (content == "Holiday") cssClass = "holiday";
        //                        else if (content == "Approved") cssClass = "approved";
        //                        else if (content == "Leave") cssClass = "leave";
        //                        else if (content == "CompOff") cssClass = "compoff";
        //                    }

        //                    //html += $"<td class='{cssClass}' onclick=\"openTaskModal('{startDate.Year}-{startDate.Month:00}-{day:00}')\">" +
        //                    //        $"<strong>{day}</strong><br />{content}</td>";
        //                    DateTime cellDate = new DateTime(startDate.Year, startDate.Month, day);

        //                    if (cellDate <= DateTime.Today)
        //                    {
        //                        string inTime = attendanceData.ContainsKey(cellDate) ? attendanceData[cellDate].InTime : "--:--:--";
        //                        string outTime = attendanceData.ContainsKey(cellDate) ? attendanceData[cellDate].OutTime : "--:--:--";

        //                        html += $"<td class='{cssClass}' onclick=\"openTaskModal('{cellDate:yyyy-MM-dd}', '{inTime}', '{outTime}')\">" +
        //                                $"<strong>{day}</strong><br />{content}</td>";
        //                    }
        //                    else
        //                    {
        //                        html += $"<td class='{cssClass}'>" +
        //                                $"<strong>{day}</strong><br />{content}</td>";
        //                    }


        //                    day++;
        //                }
        //            }
        //            html += "</tr>";

        //            if (day > daysInMonth)
        //                break;
        //        }

        //        litCalendar.Text = html;
        //    }
        //    catch (Exception ex)
        //    {
        //        using (SqlConnection conn = new SqlConnection(connString))
        //        {
        //            conn.Open();
        //            using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
        //            {

        //                logCmd.CommandType = CommandType.StoredProcedure;
        //                logCmd.Parameters.AddWithValue("@MethodName", "GenerateCalendar");
        //                logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
        //                logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

        //                logCmd.ExecuteNonQuery();
        //            }
        //        }

        //    }

        //}


        //protected void btnEmpTimSht_Click(object sender, EventArgs e)
        //{
        //    // Redirect to timesheet.aspx
        //    string userId = Request.QueryString["UserID"];
        //    Response.Redirect("Notice.aspx?UserID=" + Server.UrlEncode(userId) + "&Username=" + Server.UrlEncode(txtCreatedBy.Text.Trim()));
        //}

       //Start Grid button

        //This Metod is unnessary calling in Grid
        //protected void gvTasks_RowEditing(object sender, GridViewEditEventArgs e)
        //{
        //    gvTasks.EditIndex = e.NewEditIndex;
        //    BindGrid();
        //}

        protected void gvTasks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
          
                GridViewRow row = gvTasks.Rows[e.RowIndex];

                int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Value);
                string date = ((TextBox)row.FindControl("txtDate")).Text;
                string desc = ((TextBox)row.FindControl("txtDescription")).Text;
                string timeSpent = ((TextBox)row.FindControl("txtTimeSpent")).Text;

                string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            try
            { 

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string updateQuery = "UPDATE TaskEntries SET TaskDate=@Date, Description=@Desc, TimeSpent=@TimeSpent WHERE TaskID=@ID";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@Desc", desc);
                        cmd.Parameters.AddWithValue("@TimeSpent", timeSpent);
                        cmd.Parameters.AddWithValue("@ID", taskId);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                gvTasks.EditIndex = -1;
                BindGrid();
            }
            catch (Exception ex)
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {

                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "gvTasks_RowUpdating");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        logCmd.ExecuteNonQuery();
                    }
                }

            }

        }

        protected void gvTasks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTasks.EditIndex = -1;
            BindGrid();
        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string input = txtModalDate.Text.Trim();
                DateTime taskDateG = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture).Date;

                string userId = Request.QueryString["UserID"];

                using (SqlCommand cmd = new SqlCommand("Get_BindGrid_TaskEntries", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TaskDate", taskDateG.ToString("yyyy-MM-dd"));

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // 🔹 Load disabled dates from TaskEntries
                        disabledDates = new HashSet<DateTime>();
                        using (SqlCommand cmd2 = new SqlCommand(
                            "SELECT TaskDate FROM TaskEntries where TaskDate Not in (SELECT TaskDate FROM TaskEntries WHERE UserId = @UserId  and IsInYourBucket ='Y'  AND IsPending != 'Y' and IsHoliday != 'Y' and IsLeave != 'Y' and IsWeeklyOff != 'Y' and IsCompOff != 'Y' and IsSendForApproval != 'Y') order by TaskDate asc", conn))
                        {
                            cmd2.Parameters.AddWithValue("@UserId", userId);

                            if (conn.State != ConnectionState.Open)
                                conn.Open();

                            using (SqlDataReader rdr = cmd2.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    disabledDates.Add(Convert.ToDateTime(rdr["TaskDate"]).Date);
                                }
                            }
                        }

                        ViewState["GridData"] = dt;
                        lblItemStatus.Text = "Rows loaded: " + dt.Rows.Count;

                        // === your existing holiday/leave check ===
                        if (dt != null)
                        {
                            bool matchFound = dt.AsEnumerable().Any(r =>
                                new[] { "holiday", "leave", "weekly", "comp" }
                                .Contains(r["Description"].ToString().Trim().ToLower()));

                            if (matchFound)
                            {
                                allrows.Style["display"] = "none";
                                Btn_Tsave.Enabled = false;
                            }
                            else
                            {
                                allrows.Style["display"] = "block";
                                Btn_Tsave.Enabled = true;
                            }
                        }
                        else
                        {
                            allrows.Style["display"] = "block";
                            Btn_Tsave.Enabled = true;
                        }

                        Btn_TAprov.Enabled = (dt.Rows.Count >= 1);

                        gvTasks.DataSource = dt;
                        gvTasks.DataBind();
                    }
                }
            }
        }



        //private void BindGrid()
        //{
        //    string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        string input = txtModalDate.Text.Trim();  // Make sure no leading/trailing spaces
        //        DateTime taskDateG = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture).Date;

        //        string userId = Request.QueryString["UserID"];

        //        // Debug check: Print values
        //        System.Diagnostics.Debug.WriteLine("UserId: " + userId);
        //        System.Diagnostics.Debug.WriteLine("TaskDate: " + taskDateG.ToString("yyyy-MM-dd"));

        //        using (SqlCommand cmd = new SqlCommand("Get_BindGrid_TaskEntries", conn))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.AddWithValue("@UserId", userId); // Must be '100000'
        //            cmd.Parameters.AddWithValue("@TaskDate", taskDateG.ToString("yyyy-MM-dd")); //"2025-06-02" / taskDateG.ToString("yyyy-MM-dd") format yyyy-MM-dd// Hrutik

        //            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
        //            {
        //                DataTable dt = new DataTable();
        //                da.Fill(dt);



        //                ViewState["GridData"] = dt;
        //                lblItemStatus.Text = "Rows loaded: " + dt.Rows.Count;

        //                //DataTable dta = ViewState["GridData"] as DataTable;

        //                if (dt != null)
        //                {
        //                    bool matchFound = false;

        //                    foreach (DataRow row in dt.Rows)
        //                    {
        //                        string description = row["Description"].ToString().Trim().ToLower();

        //                        if (description == "holiday" || description == "leave" ||
        //                            description == "weekly" || description == "comp")
        //                        {
        //                            matchFound = true;
        //                            break; // Exit loop early when a match is found
        //                        }
        //                    }

        //                    if (matchFound)
        //                    {
        //                        allrows.Style["display"] = "none";
        //                        Btn_Tsave.Enabled = false;
        //                    }
        //                    else
        //                    {
        //                        allrows.Style["display"] = "block";
        //                        Btn_Tsave.Enabled = true;
        //                    }
        //                }
        //                else
        //                {
        //                    allrows.Style["display"] = "block";
        //                    Btn_Tsave.Enabled = true;
        //                }

        //                if (dt.Rows.Count >= 1)
        //                {
        //                    Btn_TAprov.Enabled = true;
        //                }
        //                else
        //                {
        //                    Btn_TAprov.Enabled = false;
        //                }

        //                gvTasks.DataSource = dt;
        //                gvTasks.DataBind();

        //            }
        //        }
        //    }
        //}



        protected void gvTasks_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvTasks.PageIndex = e.NewPageIndex;
            BindGrid(); // rebind the data
        }

        protected void gvTasks_DataBound(object sender, EventArgs e)
        {
            int rowCount = gvTasks.Rows.Count;
            int totalCount = 0;

            // Try getting the total count from the data source (if known), fallback to row count
            if (gvTasks.DataSource is DataTable dt)
            {
                totalCount = dt.Rows.Count;
            }
            else if (ViewState["GridData"] is DataTable dt2)
            {
                totalCount = dt2.Rows.Count;
            }
            else
            {
                totalCount = rowCount;
            }

            if (rowCount > 0)
            {
                int startItem = gvTasks.PageIndex * gvTasks.PageSize + 1;
                int endItem = startItem + rowCount - 1;
                lblItemStatus.Text = $"{endItem} of {totalCount} item(s)";
            }
            else
            {
                lblItemStatus.Text = "No productivity item to show.";
            }
        }

        protected void gvTasks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand("PRC_Modify_TaskEntries", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Flag", "D");
                cmd.Parameters.AddWithValue("@TaskID", taskId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            BindGrid(); // your method to reload data
        }

        protected void gvTasks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRow")
            {
                int taskId = Convert.ToInt32(e.CommandArgument);

                DataTable dt = ViewState["GridData"] as DataTable;
                if (dt != null)
                {
                    DataRow[] selectedRow = dt.Select("TaskID = " + taskId);
                    if (selectedRow.Length > 0)
                    {
                        DataRow row = selectedRow[0];

                        // Set values in modal controls
                        //txtModalDate.Text = Convert.ToDateTime(row["TaskDate"]).ToString("yyyy-MM-dd");
                        ddlModalType.SelectedValue = row["Type"].ToString();
                        ddlWorkCompletion.SelectedValue = row["WorkCompletion"].ToString();
                        txtTimeSpent.Text = TimeSpan.Parse(row["TimeSpent"].ToString()).ToString(@"hh\:mm");
                        txtDescription.Text = row["Description"].ToString();

                        // Set TaskID in hidden field
                        hfSelectedTaskID.Value = taskId.ToString();

                        // Set Save button to Edit mode
                        Btn_Tsave.Text = "Edit";
                        Btn_Tsave.Enabled = true;

                        // Open modal
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    }
                }
            }
        }

        private HashSet<DateTime> disabledDates;

        //private void LoadDisabledDates()
        //{
        //    disabledDates = new HashSet<DateTime>();

        //    string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        conn.Open();
        //        using (SqlCommand cmd = new SqlCommand(
        //            "SELECT StatusDate FROM CalendarStatus WHERE StatusType != 'In your bucket'", conn))
        //        {
        //            using (SqlDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    if (reader["StatusDate"] != DBNull.Value)
        //                    {
        //                        disabledDates.Add(Convert.ToDateTime(reader["StatusDate"]));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime taskDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "TaskDate")).Date;

                // Compare only the DATE part
                if (disabledDates != null && disabledDates.Any(d => d.Date == taskDate))
                {
                    LinkButton lnkEdit = (LinkButton)e.Row.FindControl("lnkEdit");
                    if (lnkEdit != null)
                    {
                        lnkEdit.Enabled = false;
                        lnkEdit.CssClass += " disabled";
                    }

                    LinkButton lnkDelete = (LinkButton)e.Row.FindControl("lnkDelete");
                    if (lnkDelete != null)
                    {
                        lnkDelete.Enabled = false;
                        lnkDelete.CssClass += " disabled";
                    }
                }
            }
        }


        protected void Btn_Tsave_Click(object sender, EventArgs e)
        {
           using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString))
            if (Btn_Tsave.Text == "Edit" && !string.IsNullOrEmpty(hfSelectedTaskID.Value))
            {
                int taskId = Convert.ToInt32(hfSelectedTaskID.Value);
                    string input = txtModalDate.Text; // Example: "13-02-2025"
                    DateTime taskDate = DateTime.ParseExact(input, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
                    string formattedDate = taskDate.ToString("yyyy-MM-dd");
                    string description = txtDescription.Text;
                string timeSpent = txtTimeSpent.Text;

                using (SqlCommand cmd = new SqlCommand("PRC_Modify_TaskEntries", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", "E");
                    cmd.Parameters.AddWithValue("@TaskID", taskId);
                    cmd.Parameters.AddWithValue("@TaskDate", formattedDate);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@TimeSpent", TimeSpan.Parse(timeSpent));
                    cmd.Parameters.AddWithValue("@Type", ddlModalType.SelectedValue);
                    cmd.Parameters.AddWithValue("@WorkCompletion", ddlWorkCompletion.SelectedValue);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                if (string.IsNullOrEmpty(txtModalDate.Text) || ddlModalType.SelectedValue == "")
                {
                    // Validation failed — reopen modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
                    return;
                }


                BindGrid(); // Refresh grid
                ClearModal();
            }
           else  if (Btn_Tsave.Text == "Save")
                {
                    string selectedDayType = hfDayType.Value; // Get selected day type like "Holiday", "Leave"
                    string input = txtModalDate.Text; // Example: "13-02-2025"
                    DateTime taskDate = DateTime.ParseExact(input, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).Date;
                    string formattedDate = taskDate.ToString("yyyy-MM-dd");

                    string userId = Request.QueryString["UserID"];
                    using (SqlCommand cmd = new SqlCommand("Ins_TaskEntries", conn))

                      if (selectedDayType == "Holiday" || selectedDayType == "Leave" ||selectedDayType == "Weekly" || selectedDayType == "Comp")
                      {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TaskDate", formattedDate);
                        cmd.Parameters.AddWithValue("@Description", selectedDayType);
                        cmd.Parameters.AddWithValue("@TimeSpent", txtTimeSpent.Text);
                        //cmd.Parameters.AddWithValue("@type", ddlModalType.SelectedValue);
                        //cmd.Parameters.AddWithValue("@WorkCompletion", ddlWorkCompletion.SelectedValue);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@Flag", "A");

                            conn.Open();
                        cmd.ExecuteNonQuery();
                            BindGrid();
                            

                        }
                        else
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TaskDate", formattedDate);
                            cmd.Parameters.AddWithValue("@Description", txtDescription.Text);
                            cmd.Parameters.AddWithValue("@TimeSpent", txtTimeSpent.Text);
                            cmd.Parameters.AddWithValue("@type", ddlModalType.SelectedValue);
                            cmd.Parameters.AddWithValue("@WorkCompletion", ddlWorkCompletion.SelectedValue);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            cmd.Parameters.AddWithValue("@Flag", "P");
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            BindGrid();

                            //Hrutik 28092025
                            using (SqlConnection conn1 = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString))
                            
                            using (SqlCommand cmd1 = new SqlCommand("PRC_InsertLoginLogout", conn1))
                            {
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@UserId", userId);
                                cmd1.Parameters.AddWithValue("@Action", "Both");

                                // Parse the date from textbox (only the date part)
                                DateTime logDate = DateTime.ParseExact(
                                    txtModalDate.Text,
                                    "dd-MM-yyyy",
                                    CultureInfo.InvariantCulture
                                );

                                // Combine date + time safely
                                DateTime inDateTime;
                                DateTime outDateTime;
                                bool hasInTime = !string.IsNullOrWhiteSpace(inTimeText.Text) && inTimeText.Text != "00:00:00";
                                bool hasOutTime = !string.IsNullOrWhiteSpace(outTimeText.Text) && outTimeText.Text != "00:00:00";

                                if (hasInTime)
                                {
                                    inDateTime = DateTime.ParseExact(
                                        txtModalDate.Text + " " + inTimeText.Text,
                                        "dd-MM-yyyy HH:mm:ss",
                                        CultureInfo.InvariantCulture
                                    );
                                    cmd1.Parameters.AddWithValue("@In_Time", inDateTime);
                                }
                                else
                                {
                                    cmd1.Parameters.AddWithValue("@In_Time", DBNull.Value);
                                }

                                if (hasOutTime)
                                {
                                    outDateTime = DateTime.ParseExact(
                                        txtModalDate.Text + " " + outTimeText.Text,
                                        "dd-MM-yyyy HH:mm:ss",
                                        CultureInfo.InvariantCulture
                                    );
                                    cmd1.Parameters.AddWithValue("@Out_Time", outDateTime);
                                }
                                else
                                {
                                    cmd1.Parameters.AddWithValue("@Out_Time", DBNull.Value);
                                }

                                // Always add LogDate
                                cmd1.Parameters.AddWithValue("@LogDate", logDate);

                                conn1.Open();
                                cmd1.ExecuteNonQuery();
                            }


                        }

                }

            //Btn_TAprov.Enabled = true;

        }

        protected void Btn_SndFrApprov_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string input = txtModalDate.Text.Trim();  // "dd-MM-yyyy"
                DateTime taskDate = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture).Date;
                string userId = Request.QueryString["UserID"];

                conn.Open();

                using (SqlCommand cmd = new SqlCommand("PRC_SendForApproval", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.Parameters.AddWithValue("@TaskDate", taskDate);

                    SqlParameter outputMsg = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 200)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputMsg);

                    cmd.ExecuteNonQuery();

                    string result = outputMsg.Value.ToString();
                    //                string script = $@"
                    //alert('{result}');
                    //$('#taskModal').modal('hide'); 
                    //";
                    string script = $@"
                alert('{result}');
                $('#taskModal').modal('hide');
                setTimeout(function(){{
                    location.reload();
                }}, 500);
            ";


                    ScriptManager.RegisterStartupScript(this, this.GetType(), "CloseModalWithAlert", script, true);

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{result}');", true);
                }
            }
            //Response.Redirect(Request.RawUrl);
        }





        private void ClearModal()
        {
            txtModalDate.Text = "";
            txtDescription.Text = "";
            txtTimeSpent.Text = "";
            ddlModalType.SelectedIndex = 0;
            ddlWorkCompletion.SelectedIndex = 0;
            hfSelectedTaskID.Value = "";
            Btn_Tsave.Text = "Save";
            Btn_Tsave.Enabled = false;
        }

        private DataRow GetTodayLog(string userId)
        {
            using (SqlConnection conn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"
        SELECT TOP 1 * 
        FROM UserLogTimes
        WHERE UserId = @UserId AND LogDate = CAST(GETDATE() AS DATE)
        ORDER BY Id DESC", conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                return dt.Rows.Count > 0 ? dt.Rows[0] : null;
            }
        }



        private bool HasLoggedOutToday(string userId)
        {
            bool exists = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) 
                                             FROM UserLogTimes 
                                             WHERE UserId = @UserId 
                                               AND LogDate = CAST(GETDATE() AS DATE) 
                                               AND Out_Time IS NOT NULL", conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                exists = count > 0;
            }
            return exists;
        }

        private bool HasOpenLogin(string userId)
        {
            bool exists = false;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"SELECT COUNT(*) 
                                             FROM UserLogTimes 
                                             WHERE UserId = @UserId 
                                               AND LogDate = CAST(GETDATE() AS DATE) 
                                               AND Out_Time IS NULL", conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                conn.Open();
                int count = (int)cmd.ExecuteScalar();
                exists = count > 0;
            }
            return exists;
        }

        //protected void btnLoginLogout_Click(object sender, EventArgs e)
        //{
        //    string userId = Request.QueryString["UserID"]; // or Session["UserId"].ToString()
        //    LogManager logManager = new LogManager();

        //    if (btnLoginLogout.Text == "Login")
        //    {
        //        // --- Insert Login ---
        //        logManager.InsertLoginLogout(userId, "LOGIN");

        //        // --- Fill In-Time Textbox from DB ---
        //        DataRow log = GetTodayLog(userId);
        //        if (log != null && log["In_Time"] != DBNull.Value)
        //            txtInTime.Text = Convert.ToDateTime(log["In_Time"]).ToString("HH:mm:ss");

        //        btnLoginLogout.Text = "Logout"; // switch button
        //    }
        //    else if (btnLoginLogout.Text == "Logout")
        //    {
        //        // --- Update Logout ---
        //        logManager.InsertLoginLogout(userId, "LOGOUT");

        //        // --- Fill Out-Time Textbox from DB ---
        //        DataRow log = GetTodayLog(userId);
        //        if (log != null && log["Out_Time"] != DBNull.Value)
        //            txtOutTime.Text = Convert.ToDateTime(log["Out_Time"]).ToString("HH:mm:ss");

        //        btnLoginLogout.Enabled = false; // disable button after logout
        //    }
        //}





        //protected void btnLoginLogout_Click(object sender, EventArgs e)
        //{
        //    string userId = Request.QueryString["UserID"];  // use consistent variable name
        //    LogManager logManager = new LogManager();

        //    if (btnLoginLogout.Text == "Login")
        //    {
        //        logManager.InsertLoginLogout(userId, "LOGIN");
        //        btnLoginLogout.Text = "Logout";
        //    }
        //    else if (btnLoginLogout.Text == "Logout")
        //    {
        //        logManager.InsertLoginLogout(userId, "LOGOUT");
        //        btnLoginLogout.Enabled = false;
        //    }
        //}

        protected void btnLoginLogout_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            LogManager logManager = new LogManager();

            if (btnLoginLogout.Text == "Login")
            {
                logManager.InsertLoginLogout(userId, "LOGIN");

                DataRow log = GetTodayLog(userId);
                if (log != null && log["In_Time"] != DBNull.Value)
                    txtInTime.Text = Convert.ToDateTime(log["In_Time"]).ToString("HH:mm:ss");

                btnLoginLogout.Text = "Logout";
            }
            else
            {
                logManager.InsertLoginLogout(userId, "LOGOUT");

                DataRow log = GetTodayLog(userId);
                if (log != null && log["Out_Time"] != DBNull.Value)
                    txtOutTime.Text = Convert.ToDateTime(log["Out_Time"]).ToString("HH:mm:ss");

                btnLoginLogout.Enabled = false;
            }
        }


    }

    public class LogManager
    {
        private string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        public void InsertLoginLogout(string userId, string action, DateTime? logDate = null)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand("PRC_InsertLoginLogout", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Action", action);

                if (logDate.HasValue)
                    cmd.Parameters.AddWithValue("@LogDate", logDate.Value);
                else
                    cmd.Parameters.AddWithValue("@LogDate", DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}