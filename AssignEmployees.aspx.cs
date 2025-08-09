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