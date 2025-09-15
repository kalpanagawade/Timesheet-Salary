using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace EmployeeTimesheet_Salary
{
    public partial class CreateUpdateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtCreatedBy.Text = Request.QueryString["Username"];
            if (!IsPostBack) // Ensure it runs only on the first page load
            {
                Bigbox.Attributes["style"] = "display:block;";
                Bigbox1.Attributes["style"] = "display:none;";
                Smallbox.Attributes["style"] = "display:none;";
                DropDownList1.SelectedValue = "10";
                GridView1.PageSize = Convert.ToInt32(DropDownList1.SelectedValue);
                BindGrid();
                GetNextUserId();
                GridUser();
                DateTime dobDefault = DateTime.Today.AddYears(-18);
                txtDOB.Text = dobDefault.ToString("d MMM yyyy");
                btnsersac.Attributes["disabled"] = "true";
                //LoadUserDetails();
                BindDesignationDropdown();
                BuildModuleTree();
                rowno6.Visible = false;
            }


        }

        private void BuildModuleTree()
        {
            // Use the connection string from Web.config
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            try
            {

                DataTable dtModules = new DataTable();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM iModule WHERE ModuleStatus = 0", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtModules);
                }

                // Build the nested tree HTML
                StringBuilder sb = new StringBuilder();
                sb.Append("<ul style='list-style-type:none;'>");

                foreach (DataRow row in dtModules.Select("ParentModuleID IS NULL"))
                {
                    BuildTreeNode(row, dtModules, sb);
                }

                sb.Append("</ul>");

                // Render the HTML into the Literal control
                litModuleTree.Text = sb.ToString();
                litModuleTreeE.Text= sb.ToString();
            }
            catch (Exception ex)
            {
                // Log the error using a SqlConnection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BuildModuleTree");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        conn.Open();
                        logCmd.ExecuteNonQuery();
                    }
                }
            }

        }

        private void BuildTreeNode(DataRow row, DataTable allModules, StringBuilder sb)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            try
            {
                int moduleId = Convert.ToInt32(row["ModuleID"]);
                string moduleName = row["ModuleCode"].ToString();

                sb.Append("<li>");
                sb.AppendFormat("<input type='checkbox' id='chk_{0}' /> {1}", moduleId, moduleName);

                DataRow[] childRows = allModules.Select("ParentModuleID = " + moduleId);
                if (childRows.Length > 0)
                {
                    sb.Append("<ul style='list-style-type:none; margin-left: 20px;'>");
                    foreach (DataRow child in childRows)
                    {
                        BuildTreeNode(child, allModules, sb);
                    }
                    sb.Append("</ul>");
                }

                sb.Append("</li>");
            }
            catch (Exception ex)
            {
                // Log the error using a SqlConnection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BuildTreeNode");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        conn.Open();
                        logCmd.ExecuteNonQuery();
                    }
                }
            }

        }


        private void GetNextUserId()
        {
            // Retrieve connection string from web.config
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ISNULL(MAX(userId), 0) + 1 FROM iUser";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        txtUserid.Text = result.ToString();
                        Lblid.Text = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    txtUserid.Text = "Error: " + ex.Message;
                    
                        using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                        {
                            logCmd.CommandType = CommandType.StoredProcedure;
                            logCmd.Parameters.AddWithValue("@MethodName", "GetNextUserId");
                            logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                            logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                            //conn.Open();
                            logCmd.ExecuteNonQuery();
                        }


                }
            }
        }
        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("Ins_Map_designation_UserID_Data", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserId", txtUserid.Text);
                        cmd.Parameters.AddWithValue("@Des_Name", ddlRole.SelectedValue);
                       
                        cmd.ExecuteNonQuery();
                    }

                    // JavaScript to enable the service sanctioning button
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "enableBtn", $@"
                alert('Successfully Saved!');
                document.getElementById('{btnsersac.ClientID}').removeAttribute('disabled');
            ", true);
                    //btnSave.Attributes["disabled"] = "true";
                }
                catch (Exception ex)
                {


                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnSaveRole_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }



                }
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("Ins_Iuser_Data", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserId", txtUserid.Text);
                        cmd.Parameters.AddWithValue("@UserLoginName", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@UserPIN", txtPassword.Text);
                        cmd.Parameters.AddWithValue("@UserEmailId", txtEmail.Text);
                        cmd.Parameters.AddWithValue("@UserMobileNo1", txtMobile.Text);
                        cmd.Parameters.AddWithValue("@DOB", Convert.ToDateTime(txtDOB.Text));
                        cmd.Parameters.AddWithValue("@CreatedBy", txtCreatedBy.Text);
                        cmd.Parameters.AddWithValue("@Des_Name", ddlRole.SelectedValue);
                        if (RDOIUsrTyp.Checked)
                        {
                            cmd.Parameters.AddWithValue("@User_type", "I");
                        }
                        else if (RDOEUsrTyp.Checked)
                        {
                            cmd.Parameters.AddWithValue("@User_type", "E");
                        }
                        cmd.ExecuteNonQuery();
                    }

                    // JavaScript to enable the service sanctioning button
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "enableBtn", $@"
                alert('Successfully Saved!');
                document.getElementById('{btnsersac.ClientID}').removeAttribute('disabled');
            ", true);
                    btnSave.Attributes["disabled"] = "true";
                    rowno6.Visible = true;
                }
                catch (Exception ex)
                {
                    
                    
                        using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                        {
                            logCmd.CommandType = CommandType.StoredProcedure;
                            logCmd.Parameters.AddWithValue("@MethodName", "btnSave_Click");
                            logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                            logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                           
                            logCmd.ExecuteNonQuery();
                        }
                    

                    
                }
            }
        }


        protected void btnSen_Click(object sender, EventArgs e)
        {
            cr3A.Attributes["style"] = "display:none;";
            cr3B.Attributes["style"] = "display:block;";
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            // Get comma-separated Module IDs from hidden field
            string selectedModuleIds = hfSelectedModules.Value;

            // Split into array
            string[] moduleIds = selectedModuleIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    foreach (string moduleId in moduleIds)
                    {
                        using (SqlCommand cmd = new SqlCommand("PRC_INS_iUserGrpAcs", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            Button clickedButton = sender as Button;
                            if (clickedButton != null)
                            {
                                if (clickedButton.ID == "BtnSen")
                                {
                                    cmd.Parameters.AddWithValue("@UserId", Lblid.Text);
                                }
                                else if (clickedButton.ID == "BtnSenE")
                                {
                                    cmd.Parameters.AddWithValue("@UserId", Label6.Text);
                                }

                                // Execute the command or your logic here
                                // Example:
                                // cmd.ExecuteNonQuery();
                            }


                            //cmd.Parameters.AddWithValue("@UserId", Label6.Text);
                            cmd.Parameters.AddWithValue("@CrecatedBy", txtCreatedBy.Text);
                            cmd.Parameters.AddWithValue("@Module_Id", moduleId); // Single module at a time

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {

                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnSen_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }

                  
                }
            }
        }



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Bigbox.Attributes["style"] = "display:none;";
            Bigbox1.Attributes["style"] = "display:block;";
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Define stored procedure name, not SQL text
                    using (SqlCommand cmd = new SqlCommand("search_userdetail", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; // Tell it it's a stored procedure

                        // Check if textUserid is not null or blank
                        if (!string.IsNullOrWhiteSpace(txtuidsrh.Text))
                        {
                            cmd.Parameters.AddWithValue("@UserId", txtuidsrh.Text.Trim());
                        }

                        // Check if textUserLoginName is not null or blank
                        if (!string.IsNullOrWhiteSpace(txtunamsrh.Text))
                        {
                            cmd.Parameters.AddWithValue("@UserLoginName", txtunamsrh.Text.Trim());
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                        else
                        {
                            GridView1.DataSource = null;
                            GridView1.DataBind();

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('No record found.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {



                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnSearch_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }
                    
                }
            }
        }

        protected void CancelED_Click(object sender, EventArgs e)
        {
            Bigbox.Attributes["style"] = "display:none;";
            Bigbox1.Attributes["style"] = "display:block;";
            Smallbox.Attributes["style"] = "display:none;";
            Smallboxcr.Attributes["style"] = "display:block;";
            lbEd.Text="";
        }
        private void GridUser()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // Define SQL query, no need for stored procedure here
                    string query = "SELECT UserId, UserLoginName,CASE WHEN LogonFailureCount < 3  AND LastPINChangeDate >= DATEADD(MONTH, -1, CAST(GETDATE() AS DATE)) AND LastAccessDate >= DATEADD(MONTH, -1, CAST(GETDATE() AS DATE)) AND (IneffectiveDate IS NULL OR IneffectiveDate > CAST(GETDATE() AS DATE)) THEN 'Active' ELSE 'Inactive' END AS Status FROM iUser";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // Execute the query and fill the data into a DataTable
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        // Bind the data to the GridView
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    // Show error message in case of an exception
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "GridUser()");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            Bigbox.Attributes["style"] = "display:none;";
            Bigbox1.Attributes["style"] = "display:block;";
            Smallbox.Attributes["style"] = "display:block;";
            Smallboxcr.Attributes["style"] = "display:none;";
            lbEd.Text = " > Edit";

            if (e.CommandName == "CustomClick")
            {
                string userId = e.CommandArgument.ToString();

                // Debug alert (optional)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('UserID clicked: {userId}');", true);

                string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    try
                    {
                        conn.Open();

                        string query = "SELECT UserId, UserLoginName, DOB, UserEmailId, UserMobileNo1 FROM iUser WHERE UserId = @UserId";

                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);

                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                txtUseridED.Text = dt.Rows[0]["UserId"].ToString();
                                Label6.Text = dt.Rows[0]["UserId"].ToString();
                                txtUserLoginNameED.Text = dt.Rows[0]["UserLoginName"].ToString();

                                DateTime dob;
                                if (DateTime.TryParse(dt.Rows[0]["DOB"].ToString(), out dob))
                                {
                                    txtDOBED.Text = dob.ToString("d MMM yyyy").ToUpper();
                                }

                                txtemED.Text = dt.Rows[0]["UserEmailId"].ToString();
                                txtumnED.Text = dt.Rows[0]["UserMobileNo1"].ToString();

                                // Populate dropdowns, radio buttons, and checkboxes
                                BindDesignationDropdownEdit(userId);
                                BindStatusDropdownEdit(userId);
                                BindUserTypeRadioBtn(userId);
                                BindServicesCheckbox(userId);   // ✅ THIS binds checkboxes as needed
                            }
                            else
                            {
                                txtUseridED.Text = "User not found";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                        {
                            logCmd.CommandType = CommandType.StoredProcedure;
                            logCmd.Parameters.AddWithValue("@MethodName", "GridView1_RowCommand");
                            logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                            logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                            logCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }


        //protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    Bigbox.Attributes["style"] = "display:none;";
        //    Bigbox1.Attributes["style"] = "display:block;";
        //    Smallbox.Attributes["style"] = "display:block;";
        //    Smallboxcr.Attributes["style"] = "display:none;";
        //    lbEd.Text=" > Edit";
        //    if (e.CommandName == "CustomClick")
        //    {
        //        string userId = e.CommandArgument.ToString();                
        //        // Do something with the userId
        //        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('UserID clicked: {userId}');", true);
        //        string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        //        using (SqlConnection conn = new SqlConnection(connString))
        //        {
        //            try
        //            {
        //                conn.Open();

        //                string query = "SELECT UserId,UserLoginName,DOB,UserEmailId,UserMobileNo1 FROM iUser where UserId="+userId;

        //                using (SqlCommand cmd = new SqlCommand(query, conn))
        //                {
        //                    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                    DataTable dt = new DataTable();
        //                    da.Fill(dt);

        //                    if (dt.Rows.Count > 0)
        //                    {
        //                        txtUseridED.Text = dt.Rows[0]["UserId"].ToString();
        //                     Label6.Text = dt.Rows[0]["UserId"].ToString();
        //                        txtUserLoginNameED.Text = dt.Rows[0]["UserLoginName"].ToString();
        //                        DateTime dob;
        //                        if (DateTime.TryParse(dt.Rows[0]["DOB"].ToString(), out dob))
        //                        {
        //                            txtDOBED.Text = dob.ToString("d MMM yyyy").ToUpper(); // Output: 1 JAN 1900
        //                        }
        //                        txtemED.Text=dt.Rows[0]["UserEmailId"].ToString();// display login name
        //                        txtumnED.Text=dt.Rows[0]["UserMobileNo1"].ToString();// display login name
        //                        BindDesignationDropdownEdit(userId);
        //                        BindStatusDropdownEdit(userId);
        //                        BindUserTypeRadioBtn(userId);
        //                        BindServicesCheckbox(userId);

        //                    }
        //                    else
        //                    {
        //                        txtUseridED.Text = "User not found";
        //                        //txtUserLoginNameED.Text = "";
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {

        //                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
        //                {
        //                    logCmd.CommandType = CommandType.StoredProcedure;
        //                    logCmd.Parameters.AddWithValue("@MethodName", "GridView1_RowCommand");
        //                    logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
        //                    logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


        //                    logCmd.ExecuteNonQuery();
        //                }

        //            }
        //        }
        //    }
        //}
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bigbox.Attributes["style"] = "display:none;";
            Bigbox1.Attributes["style"] = "display:block;";
            GridView1.PageSize = Convert.ToInt32(DropDownList1.SelectedValue);
            GridView1.PageIndex = 0; // reset to first page
            BindGrid();
        }
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Bigbox.Attributes["style"] = "display:none;";
            Bigbox1.Attributes["style"] = "display:block;";
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.PageSize = Convert.ToInt32(DropDownList1.SelectedValue);
            BindGrid();
        }

        private void BindGrid()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT UserId, UserLoginName, CASE WHEN LogonFailureCount < 3 AND LastPINChangeDate >= DATEADD(MONTH, -1, CAST(GETDATE() AS DATE)) AND LastAccessDate >= DATEADD(MONTH, -1, CAST(GETDATE() AS DATE)) AND (IneffectiveDate IS NULL OR IneffectiveDate > CAST(GETDATE() AS DATE)) THEN 'Active' ELSE 'Inactive' END AS Status FROM iUser";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BindGrid");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string userId = txtUseridED.Text.Trim();
            string UserLoginName = txtUserLoginNameED.Text.Trim();
            string DOB = txtDOBED.Text.Trim();
            string EmID = txtemED.Text.Trim();
            string MobNo = txtumnED.Text.Trim();
            string UserType = string.Empty;
            if (RadioButton1.Checked)
            {
                UserType = RadioButton1.Text.Trim();  // "Internal User Type"
            }
            else if (RadioButton2.Checked)
            {
                UserType = RadioButton2.Text.Trim();  // "External User Type"
            }

            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    // 1. Get current values
                    string selectQuery = @"SELECT UserLoginName, DOB, UserEmailId, UserMobileNo1,UserType 
                                   FROM iUser 
                                   WHERE UserId = @UserId";

                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@UserId", userId);
                        SqlDataReader reader = selectCmd.ExecuteReader();

                        if (reader.Read())
                        {
                            string currentName = reader["UserLoginName"].ToString().Trim();
                            string currentDOB = Convert.ToDateTime(reader["DOB"]).ToString("M/d/yyyy h:mm:ss tt");
                            string currentEmail = reader["UserEmailId"].ToString().Trim();
                            string currentMobile = reader["UserMobileNo1"].ToString().Trim();
                            string currentUserType = reader["UserType"].ToString().Trim();

                            reader.Close();

                            // 2. Compare values
                            if (currentName == UserLoginName &&
                                currentDOB == DOB &&
                                currentEmail == EmID &&
                                currentMobile == MobNo &&
                                currentUserType == UserType)
                            {
                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No changes were made.');", true);
                                return;
                            }

                            // 3. Proceed to update if changed
                            string updateQuery = @"UPDATE iUser 
                                           SET UserLoginName = @UserLoginName, 
                                               DOB = @DOB, 
                                               UserEmailId = @UserEmailId, 
                                               UserMobileNo1 = @UserMobileNo1, 
                                               UserType=@UserType
                                           WHERE UserId = @UserId";

                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@UserLoginName", UserLoginName);
                                updateCmd.Parameters.AddWithValue("@DOB", DOB);
                                updateCmd.Parameters.AddWithValue("@UserEmailId", EmID);
                                updateCmd.Parameters.AddWithValue("@UserMobileNo1", MobNo);
                                updateCmd.Parameters.AddWithValue("@UserId", userId);
                                if (RadioButton1.Checked)
                                {
                                    updateCmd.Parameters.AddWithValue("@UserType", "I");
                                }
                                else if (RadioButton2.Checked)
                                {
                                    updateCmd.Parameters.AddWithValue("@UserType", "E");
                                }

                                int rowsAffected = updateCmd.ExecuteNonQuery();

                                string message = rowsAffected > 0 ?
                                    "User details updated successfully." :
                                    "Update failed.";

                                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('User not found.');", true);
                        }
                    }
                }
                catch (Exception ex)
                {

                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "btnEdit_Click");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }

                   
                }
            }
        }
        protected void btnEmpDts_Click(object sender, EventArgs e)
        {
            string userId = Request.QueryString["UserID"];
            
            Response.Redirect("Notice.aspx?UserID=" + Server.UrlEncode(userId) + "&Username=" + Server.UrlEncode(txtCreatedBy.Text.Trim()));
        }

        //added By Hrutik
        //[System.Web.Services.WebMethod]
        //public static List<Employee> GetAllEmployees()
        //{
        //    List<Employee> employees = new List<Employee>();
        //    string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        string query = "SELECT UserID, UserLoginName as FullName FROM IUser where UserID='100000'"; // adjust as needed
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        conn.Open();
        //        SqlDataReader reader = cmd.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            employees.Add(new Employee
        //            {
        //                ID = reader["UserID"].ToString(),
        //                Name = reader["FullName"].ToString()
        //            });
        //        }
        //    }

        //    return employees;
        //}

        //// Employee model
        //public class Employee
        //{
        //    public string ID { get; set; }
        //    public string Name { get; set; }
        //}


        private void BindDesignationDropdown()
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = "SELECT Des_id, Des_Name FROM MST_Designation";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    ddlRole.DataSource = reader;
                    ddlRole.DataTextField = "Des_Name";
                    ddlRole.DataValueField = "Des_id";
                    ddlRole.DataBind();

                    ddlRole.Items.Insert(0, new ListItem("-- Select Role --", "0"));
                }
            }
            catch (Exception ex)
            {
                // Log the error using a SqlConnection
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BindDesignationDropdown");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);
                        conn.Open();

                        logCmd.ExecuteNonQuery();
                    }
                }
            }

        }


        private void BindDesignationDropdownEdit(string userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            int selectedDesId = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    {
                        conn.Open();

                        // 1️⃣ Get currently mapped designation (Des_ID) for the user using INNER JOIN
                        string desIdQuery = @"
            SELECT M.Des_ID
            FROM MAP_Designation_UserId M
            INNER JOIN IUser U ON M.UserId = U.UserId
            WHERE M.UserId = @UserId";

                        using (SqlCommand cmd = new SqlCommand(desIdQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            object result = cmd.ExecuteScalar();
                            if (result != null)
                                selectedDesId = Convert.ToInt32(result);
                        }

                        // 2️⃣ Get all designations from MST_Designation
                        string ddlQuery = "SELECT Des_ID, Des_Name FROM MST_Designation";

                        using (SqlCommand cmd2 = new SqlCommand(ddlQuery, conn))
                        using (SqlDataReader reader = cmd2.ExecuteReader())
                        {
                            DropDownList4.DataSource = reader;
                            DropDownList4.DataTextField = "Des_Name";
                            DropDownList4.DataValueField = "Des_ID";
                            DropDownList4.DataBind();
                        }

                        // 3️⃣ Set selected designation
                        if (selectedDesId != 0)
                        {
                            ListItem item = DropDownList4.Items.FindByValue(selectedDesId.ToString());
                            if (item != null)
                                DropDownList4.ClearSelection();
                            item.Selected = true;
                        }
                    }
                }

                catch (Exception ex)
                {



                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BindDesignationDropdownEdit");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);


                        logCmd.ExecuteNonQuery();
                    }

                }

            } 
        }

        private void BindStatusDropdownEdit(string userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string statusQuery = @"
                SELECT 
                    CASE 
                        WHEN LogonFailureCount < 3
                         AND LastPINChangeDate >= DATEADD(MONTH, -1, CAST(GETDATE() AS DATE))
                         AND LastAccessDate >= DATEADD(MONTH, -1, CAST(GETDATE() AS DATE))
                         AND (IneffectiveDate IS NULL OR IneffectiveDate > CAST(GETDATE() AS DATE)) 
                        THEN 'Active'
                        ELSE 'Inactive'
                    END AS Status
                FROM iUser
                WHERE UserId = @UserId";

                    string userStatus = "Unknown";

                    using (SqlCommand statusCmd = new SqlCommand(statusQuery, conn))
                    {
                        statusCmd.Parameters.AddWithValue("@UserId", userId);
                        object statusResult = statusCmd.ExecuteScalar();
                        if (statusResult != null)
                        {
                            userStatus = statusResult.ToString();
                        }
                    }

                    // Bind Status to DropDownList3
                    DropDownList3.Items.Clear();
                    DropDownList3.Items.Add(new ListItem("Active", "Active"));
                    DropDownList3.Items.Add(new ListItem("Inactive", "Inactive"));

                    // Set selected status
                    ListItem selectedItem = DropDownList3.Items.FindByValue(userStatus);
                    if (selectedItem != null)
                        selectedItem.Selected = true;
                }
                catch (Exception ex)
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BindStatusDropdownEdit");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void BindUserTypeRadioBtn(string userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string query = "SELECT UserType FROM iUser WHERE UserId = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            string userType = result.ToString().Trim();

                            if (userType == "I")
                            {
                                RadioButton1.Checked = true;
                                RadioButton2.Checked = false;
                            }
                            else if (userType == "E")
                            {
                                RadioButton1.Checked = false;
                                RadioButton2.Checked = true;
                            }
                            else
                            {
                                // If neither, uncheck both (optional)
                                RadioButton1.Checked = false;
                                RadioButton2.Checked = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", conn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BindUserTypeRadioBtn");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        private void BindServicesCheckbox(string userId)
        {
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // Step 1: Get selected modules for the user
                    string userModulesQuery = "SELECT Module_Id FROM iUserGrpAcs WHERE UserId = @UserId";
                    List<string> selectedModuleIds = new List<string>();

                    using (SqlCommand cmd = new SqlCommand(userModulesQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                selectedModuleIds.Add(reader["Module_Id"].ToString());
                            }
                        }
                    }

                    // Step 2: Get all available modules from iModule
                    string allModulesQuery = "SELECT ModuleId, ModuleName FROM iModule ORDER BY ModuleId";

                    using (SqlCommand cmdAll = new SqlCommand(allModulesQuery, conn))
                    using (SqlDataReader readerAll = cmdAll.ExecuteReader())
                    {
                        StringBuilder sb = new StringBuilder();

                        while (readerAll.Read())
                        {
                            string moduleId = readerAll["ModuleId"].ToString();
                            string moduleName = readerAll["ModuleName"].ToString();
                            bool isChecked = selectedModuleIds.Contains(moduleId);

                            sb.AppendFormat("<input type='checkbox' id='chkModule{0}' name='chkModules' value='{0}' {1} /> {2}<br/>",
                                moduleId,
                                isChecked ? "checked='checked'" : "",
                                moduleName);
                        }

                        litModuleTree.Text = sb.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error into your error log table
                string connStringLog = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

                using (SqlConnection logConn = new SqlConnection(connStringLog))
                {
                    logConn.Open();

                    using (SqlCommand logCmd = new SqlCommand("PRC_InsertErrorLog", logConn))
                    {
                        logCmd.CommandType = CommandType.StoredProcedure;
                        logCmd.Parameters.AddWithValue("@MethodName", "BindServicesCheckbox");
                        logCmd.Parameters.AddWithValue("@ErrorMessage", ex.Message);
                        logCmd.Parameters.AddWithValue("@ErrorDateTime", DateTime.Now);

                        logCmd.ExecuteNonQuery();
                    }
                }
            }
        }









    }
}
