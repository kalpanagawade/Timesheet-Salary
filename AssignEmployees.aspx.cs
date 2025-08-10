using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Services;


namespace EmployeeTimesheet_Salary
{
    public partial class AssignEmployees : System.Web.UI.Page
    {
        public List<Employee> Employees = new List<Employee>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindEmployees();
                LoadEmployees();
            }

        }
        //Added by Hrutik 10082025

        protected void btnassign_Click(object sender, EventArgs e)
        {
            // Get selected employee IDs from the hidden field
            string selectedEmpIds = hfSelectedEmployees.Value?.Trim();
            string managerUserId = Request.QueryString["UserId"];

            if (!string.IsNullOrEmpty(selectedEmpIds) && !string.IsNullOrEmpty(managerUserId))
            {
                string[] empIds = selectedEmpIds
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => id.Trim())
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .ToArray();

                foreach (string empId in empIds)
                {
                    // Your method to assign employee to the manager
                    UpdateEmployeeManager(empId, managerUserId);
                }
            }

            // Close popup after assignment
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "CloseWindow",
                "window.close();",
                true
            );
        }

        //Added by Hrutik 10082025
        private void UpdateEmployeeManager(string employeeId, string managerUserId)
        {
            // Basic safety check
            if (string.IsNullOrEmpty(employeeId) || string.IsNullOrEmpty(managerUserId))
                return;

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = @"
            UPDATE iuser
 SET ManagerId = @ManagerId
 WHERE UserId = @EmployeeId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@ManagerId", managerUserId);
                    cmd.Parameters.AddWithValue("@EmployeeId", employeeId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }



        //private void BindEmployees()
        //{
        //    List<Employee> employees = new List<Employee>();
        //    string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        string query = "SELECT UserID, UserLoginName as FullName FROM IUser";  // Adjust query if needed
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

        //    gvEmployees.DataSource = employees;
        //    gvEmployees.DataBind();
        //}
        private void BindEmployees(string filter = "")
        {
            List<Employee> employees = new List<Employee>();
            string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserID, UserLoginName as FullName FROM IUser";

                if (!string.IsNullOrEmpty(filter))
                {
                    query += " WHERE UserID LIKE @Filter OR UserLoginName LIKE @Filter";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                if (!string.IsNullOrEmpty(filter))
                {
                    cmd.Parameters.AddWithValue("@Filter", "%" + filter + "%");
                }

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        ID = reader["UserID"].ToString(),
                        Name = reader["FullName"].ToString()
                    });
                }
            }

            gvEmployees.DataSource = employees;
            gvEmployees.DataBind();
        }

        private void LoadEmployees()
        {
            string connStr = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT UserID, UserLoginName AS FullName FROM IUser";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employees.Add(new Employee
                    {
                        ID = reader["UserID"].ToString(),
                        Name = reader["FullName"].ToString()
                    });
                }
            }
        }

        protected void txtSearchEmp_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtSearchEmp.Text.Trim();
            BindEmployees(searchValue);
        }


        public class Employee
        {
            public string ID { get; set; }
            public string Name { get; set; }
        }

        //[WebMethod]
        //public static List<Employee> GetAllEmployees()
        //{
        //    List<Employee> employees = new List<Employee>();
        //    string connString = ConfigurationManager.ConnectionStrings["MyDBConnection"].ConnectionString;

        //    using (SqlConnection conn = new SqlConnection(connString))
        //    {
        //        string query = "SELECT UserID, UserLoginName as FullName FROM IUser";
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

        //public class Employee
        //{
        //    public string ID { get; set; }
        //    public string Name { get; set; }
        //}
    }
}