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

namespace EmployeeTimesheet_Salary
{
    public partial class timesheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCreatedBy.Text = Request.QueryString["Username"];
           
            if (!IsPostBack)
            {
                txtTimeSpent.Text = "12:00"; // Default value
                ViewState["IsLoggedIn"] = false; // Default state
                DateTime today = DateTime.Today;
                hfMonthYear.Value = today.ToString("yyyy-MM-dd");
                string UserId= Request.QueryString["UserID"];
                GenerateCalendar(today, UserId);
                //txtInTime.Text = DateTime.Now.ToString("HH:mm:ss");
                //txtOutTime.Text = DateTime.Now.ToString("HH:mm:ss");
                BindGrid();
                //Btn_Tsave.Enabled = false;

            }
        }
        protected void btnTriggerBindGrid_Click(object sender, EventArgs e)
        {
            BindGrid(); // your method to bind data to GridView etc.
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

        private void GenerateCalendar(DateTime startDate, string userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            try
            {

                lblMonthYear.Text = startDate.ToString("MMMM yyyy");

                int daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                DateTime firstDay = new DateTime(startDate.Year, startDate.Month, 1);
                int startDayOfWeek = (int)firstDay.DayOfWeek;

                Dictionary<int, string> statusData = GetStatusData(startDate.Year, startDate.Month, userId);

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
                                else if (content == "Leave") cssClass = "leave";
                                else if (content == "CompOff") cssClass = "compoff";
                            }

                            html += $"<td class='{cssClass}' onclick=\"openTaskModal('{startDate.Year}-{startDate.Month:00}-{day:00}')\">" +
                                    $"<strong>{day}</strong><br />{content}</td>";
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


        protected void btnEmpTimSht_Click(object sender, EventArgs e)
        {
            // Redirect to timesheet.aspx
            string userId = Request.QueryString["UserID"];
            Response.Redirect("Notice.aspx?UserID=" + Server.UrlEncode(userId) + "&Username=" + Server.UrlEncode(txtCreatedBy.Text.Trim()));
        }

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
                string input = txtModalDate.Text.Trim();  // Make sure no leading/trailing spaces
                DateTime taskDateG = DateTime.ParseExact(input, "dd-MM-yyyy", CultureInfo.InvariantCulture).Date;

                string userId = Request.QueryString["UserID"];

                // Debug check: Print values
                System.Diagnostics.Debug.WriteLine("UserId: " + userId);
                System.Diagnostics.Debug.WriteLine("TaskDate: " + taskDateG.ToString("yyyy-MM-dd"));

                using (SqlCommand cmd = new SqlCommand("Get_BindGrid_TaskEntries", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", userId); // Must be '100000'
                    cmd.Parameters.AddWithValue("@TaskDate", taskDateG.ToString("yyyy-MM-dd")); //"2025-06-02" / taskDateG.ToString("yyyy-MM-dd") format yyyy-MM-dd// Hrutik

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        

                        ViewState["GridData"] = dt;
                        lblItemStatus.Text = "Rows loaded: " + dt.Rows.Count;

                        //DataTable dta = ViewState["GridData"] as DataTable;

                        if (dt != null)
                        {
                            bool matchFound = false;

                            foreach (DataRow row in dt.Rows)
                            {
                                string description = row["Description"].ToString().Trim().ToLower();

                                if (description == "holiday" || description == "leave" ||
                                    description == "weekly" || description == "comp")
                                {
                                    matchFound = true;
                                    break; // Exit loop early when a match is found
                                }
                            }

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

                        if (dt.Rows.Count >= 1)
                        {
                            Btn_TAprov.Enabled = true;
                        }
                        else
                        {
                            Btn_TAprov.Enabled = false;
                        }

                        gvTasks.DataSource = dt;
                        gvTasks.DataBind();

                    }
                }
            }
        }



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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{result}');", true);
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

    }
}