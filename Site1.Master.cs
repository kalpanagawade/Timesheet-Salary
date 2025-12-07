using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmployeeTimesheet_Salary
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            // Always rebuild buttons before ViewState loads
            DataTable dt;

            if (Session["UserModules"] != null)
            {
                dt = (DataTable)Session["UserModules"];
            }
            else
            {
                dt = GetModuleNames(); // Fetch from DB once
                Session["UserModules"] = dt;
            }

            CreateButtons(dt);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblUsername.Text = Request.QueryString["Username"];
            }
        }

        // 🔹 Fetch user modules from DB
        private DataTable GetModuleNames()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            string userId = Request.QueryString["UserID"];
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connString))
            using (SqlCommand cmd = new SqlCommand("PRCGET_UserLogin_N_Page_Name", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // 🔹 Create buttons based on DB result
        private void CreateButtons(DataTable dt)
        {
            navContainer.Controls.Clear();

            foreach (DataRow row in dt.Rows)
            {
                string pageName = row["Page_name"].ToString().Trim();

                if (pageName.Equals("CreateUpdateUser.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    AddHeaderButton("Employee Details", "btnEmpDts", BtnEmpDts_Click);
                }
                else if (pageName.Equals("Timesheet.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    AddHeaderButton("Employee Timesheet", "btnEmpTimSht", BtnEmpTimSht_Click);
                }
                else if (pageName.Equals("TeamTimesheet.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    AddHeaderButton("Team Timesheet", "btnTeamTimSht", BtnTeamTimSht_Click);
                }
                else if (pageName.Equals("SalaryModule.aspx", StringComparison.OrdinalIgnoreCase))
                {
                    AddHeaderButton("Employee Salary", "btnEmpSlry", btnEmpSlry_Click);
                }
            }
        }

        // 🔹 Add dynamic button to navbar
        private void AddHeaderButton(string text, string id, EventHandler clickHandler)
        {
            Button btn = new Button
            {
                Text = text,
                ID = id,
                CssClass = "nav-button",
                OnClientClick= "showLoader();",
                //UseSubmitBehavior = "false",

            };
            btn.Click += clickHandler;
            navContainer.Controls.Add(btn);
        }

        // 🔹 Click event handlers
        protected void BtnEmpDts_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            string username = lblUsername.Text.Trim();
            Response.Redirect($"~/CreateUpdateUser.aspx?UserID={Server.UrlEncode(userId)}&Username={Server.UrlEncode(username)}");
        }

        protected void BtnEmpTimSht_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            string username = lblUsername.Text.Trim();
            Response.Redirect($"~/Timesheet.aspx?UserID={Server.UrlEncode(userId)}&Username={Server.UrlEncode(username)}");
        }

        protected void BtnTeamTimSht_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            string username = lblUsername.Text.Trim();
            Response.Redirect($"~/TeamTimesheet.aspx?UserID={Server.UrlEncode(userId)}&Username={Server.UrlEncode(username)}");
        }
        protected void btnEmpSlry_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            string username = lblUsername.Text.Trim();
            Response.Redirect($"~/SalaryModule.aspx?UserID={Server.UrlEncode(userId)}&Username={Server.UrlEncode(username)}");
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.Remove("UserModules");
            Response.Redirect("~/Login.aspx");
        }

        protected void btnHome_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            string username = lblUsername.Text.Trim();
            Response.Redirect($"~/Notice.aspx?UserID={Server.UrlEncode(userId)}&Username={Server.UrlEncode(username)}");

        }
    }
}
