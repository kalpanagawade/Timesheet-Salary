using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeTimesheet_Salary
{
    public partial class TeamTimesheet : System.Web.UI.Page
    {
        string connStr = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTeamMembers();
                
            }

        }

        //private void LoadTeamMembers()
        //{
        //    using (SqlConnection con = new SqlConnection(connStr))
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter(
        //            "SELECT UserId, UserLoginName FROM kalpana..iuser WHERE ManagerId='100042'", con);
        //        DataTable dt = new DataTable();
        //        da.Fill(dt);
        //        gvTeamMembers.DataSource = dt;
        //        gvTeamMembers.DataBind();
        //    }
        //}

        private void LoadTeamMembers()
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                string userId = Request.QueryString["UserID"];

                if (string.IsNullOrEmpty(userId))
                    return; // safety check

                SqlDataAdapter da = new SqlDataAdapter(
                    "SELECT UserId, UserLoginName FROM kalpana..iuser WHERE ManagerId = @ManagerId", con);

                da.SelectCommand.Parameters.AddWithValue("@ManagerId", userId);

                DataTable dt = new DataTable();
                da.Fill(dt);
                gvTeamMembers.DataSource = dt;
                gvTeamMembers.DataBind();
            }
        }
        protected void gvTeamMembers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectMember")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string selectedUserId = gvTeamMembers.DataKeys[rowIndex].Value.ToString();
                string selectedUserName = gvTeamMembers.Rows[rowIndex].Cells[1].Text;

                ViewState["SelectedUserId"] = selectedUserId;

                // Default month/year = current
                ViewState["Month"] = DateTime.Now.Month;
                ViewState["Year"] = DateTime.Now.Year;

                LoadTimesheet(selectedUserId);

                id_name.Text = $"<h2>{selectedUserName} (ID: {selectedUserId})</h2>";
                //id_name.Text = selectedUserId;
                gvTeamMembers.Visible = false;
                btnBack.Visible = true;
                timesheetSection.Visible = true;
            }
        }

        private void LoadTimesheet(string userId)
        {
            int month = Convert.ToInt32(ViewState["Month"]);
            int year = Convert.ToInt32(ViewState["Year"]);

            lblMonth.InnerText = new DateTime(year, month, 1).ToString("MMMM yyyy");

            // Get tasks from DB
            DataTable dtTasks = new DataTable();
            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT TaskID,TaskDate, Description, TimeSpent, Type, WorkCompletion " +
                    "FROM kalpana..TaskEntries " +
                    "WHERE UserId=@UserId AND MONTH(TaskDate)=@Month AND YEAR(TaskDate)=@Year and IsSendForApproval='Y'", con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtTasks);
            }

            // Fill missing dates
            DataTable dtFullMonth = new DataTable();
            dtFullMonth.Columns.Add("TaskID", typeof(int)); // ✅ New column for CommandArgument
            dtFullMonth.Columns.Add("TaskDate", typeof(DateTime));
            dtFullMonth.Columns.Add("Description");
            dtFullMonth.Columns.Add("TimeSpent");
            dtFullMonth.Columns.Add("Type");

            // Fill full month data
            DateTime startDate = new DateTime(year, month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                DataRow[] found = dtTasks.Select($"TaskDate = #{date:MM/dd/yyyy}#");
                if (found.Length > 0)
                {
                    dtFullMonth.ImportRow(found[0]);
                    //btnApprove.Enabled = true;
                    //btnNotApprove.Enabled = true;
                }
                else
                {
                    // For missing days, TaskID can be null or 0
                    dtFullMonth.Rows.Add(DBNull.Value, date, "Not filled timesheet", DBNull.Value, DBNull.Value);
                    //btnApprove.Enabled = false;
                    //btnNotApprove.Enabled = false;
                }
            }

            gvTimesheet.DataSource = dtFullMonth;
            gvTimesheet.DataBind();
        }

        protected void gvTimesheet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Approve")
            {
                string taskId = e.CommandArgument.ToString();
                // ✅ Update database to Approved
                ApproveTask(taskId, true);
            }
            else if (e.CommandName == "NotApprove")
            {
                string taskId = e.CommandArgument.ToString();
                // ❌ Update database to Not Approved
                ApproveTask(taskId, false);
            }
        }

        private void ApproveTask(string taskId, bool isApproved)
        {
            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_UpdateTaskApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TaskID", taskId);
                    cmd.Parameters.AddWithValue("@IsApproved", isApproved);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        protected void btnPrevMonth_Click(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(ViewState["Month"]);
            int year = Convert.ToInt32(ViewState["Year"]);

            month--;
            if (month == 0)
            {
                month = 12;
                year--;
            }

            ViewState["Month"] = month;
            ViewState["Year"] = year;

            LoadTimesheet(ViewState["SelectedUserId"].ToString());
        }

        protected void btnNextMonth_Click(object sender, EventArgs e)
        {
            int month = Convert.ToInt32(ViewState["Month"]);
            int year = Convert.ToInt32(ViewState["Year"]);

            month++;
            if (month == 13)
            {
                month = 1;
                year++;
            }

            ViewState["Month"] = month;
            ViewState["Year"] = year;

            LoadTimesheet(ViewState["SelectedUserId"].ToString());
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            id_name.Text = "<h2>Team Members</h2>";
            gvTeamMembers.Visible = true;
            btnBack.Visible = false;
            timesheetSection.Visible = false;
        }

        protected void btnApproveSelected_Click(object sender, EventArgs e)
        {
            ProcessMultipleApprovals(true);
        }

        protected void btnRejectSelected_Click(object sender, EventArgs e)
        {
            ProcessMultipleApprovals(false);
        }

        private void ProcessMultipleApprovals(bool isApproved)
        {
            string selectedTaskIds = "";
            string userId = ViewState["SelectedUserId"].ToString();

            foreach (GridViewRow row in gvTimesheet.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                HiddenField hfTaskID = (HiddenField)row.FindControl("hfTaskID");

                if (chk != null && chk.Checked && hfTaskID != null && !string.IsNullOrEmpty(hfTaskID.Value))
                {
                    if (selectedTaskIds.Length > 0)
                        selectedTaskIds += ",";
                    selectedTaskIds += hfTaskID.Value;
                }
            }

            if (string.IsNullOrEmpty(selectedTaskIds))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select at least one task.');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(connStr))
            {
                using (SqlCommand cmd = new SqlCommand("PRC_UpdateMultipleTaskApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TaskIDs", selectedTaskIds);
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@IsApproved", isApproved ? "Y" : "N");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            string status = isApproved ? "approved" : "rejected";
            ClientScript.RegisterStartupScript(this.GetType(), "alert",
                $"alert('Selected task(s) {status} successfully.');", true);

            LoadTimesheet(userId);
        }


        protected void gvTimesheet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chk = (CheckBox)e.Row.FindControl("chkSelect");
                HiddenField hfDescription = (HiddenField)e.Row.FindControl("hfDescription");

                if (hfDescription != null && chk != null)
                {
                    //string desc = hfDescription.Value.ToLower();
                    //if (desc.Contains("not filled timesheet"))
                    if (hfDescription.Value.ToLower().Contains("not filled timesheet"))
                    {
                        chk.Enabled = false; // disable checkbox
                        e.Row.ForeColor = System.Drawing.Color.Gray; // optional: gray out text
                    }
                }
            }
        }
    }

}