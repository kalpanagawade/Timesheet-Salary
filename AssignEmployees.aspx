<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AssignEmployees.aspx.cs" Inherits="EmployeeTimesheet_Salary.AssignEmployees" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assign Employees</title>
     <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    <!-- Bootstrap CSS (already included) -->
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />

<!-- Bootstrap Icons -->
<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css" />


    <style>
         body { font-family: Arial; padding: 20px; }
         .grid-style { border: 1px solid #ccc; padding: 10px; }
         .header { background-color: #f5f5f5; font-weight: bold; }
        .container-custom {
            max-width: 900px;
            margin: 40px auto;
        }
       /* #txtSearchEmp {
            margin-bottom: 10px;
        }*/
        .table-wrapper {
            max-height: 300px;
            overflow-y: auto;
        }
    </style>
    
      
  
</head>
<body>
    <form id="form1" runat="server">
       
        <div class="container-custom">

            <div class="modal-content">

                <!-- Modal Header -->
                <div class="modal-header">
                    <h5 class="modal-title">Assign Employees under Manager</h5>
                </div>

                <!-- Modal Body -->
                <div class="modal-body">
    <!-- Optional: Search box, not functional for server-side unless handled in code -->
    <%--<asp:TextBox ID="txtSearchEmp" runat="server" CssClass="form-control" placeholder="Search by name or ID..." AutoPostBack="true" OnTextChanged="txtSearchEmp_TextChanged" />
     <span class="input-group-text"><i class="bi bi-search"></i></span>--%>

    <div class="input-group mb-2">
    <asp:TextBox ID="txtSearchEmp" runat="server"
        CssClass="form-control border-end-0"
        placeholder="Search by name or ID..."
        AutoPostBack="true"
        OnTextChanged="txtSearchEmp_TextChanged" />
    
    <span class="input-group-text bg-white border-start-0">
        <i class="bi bi-search"></i>
    </span>
</div>


    <div style="max-height: 300px; overflow-y: auto; margin-top: 10px;">
        <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="false"
            CssClass="table table-bordered table-striped" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ID" HeaderText="Employee ID" />
                <asp:BoundField DataField="Name" HeaderText="Employee Name" />
            </Columns>
        </asp:GridView>
    </div>
</div>


                <!-- Modal Footer -->
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" onclick="assignEmployees()">Assign</button>
                    <button type="button" class="btn btn-secondary" onclick="window.close()">Close</button>
                </div>

            </div>

        </div>
    </form>
    <!-- Bootstrap JS (optional for styling) -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        function filterEmployees() {
            var input = document.getElementById("txtSearchEmp").value.toLowerCase();
            var rows = document.querySelectorAll("#employeeTable tbody tr");

            rows.forEach(function (row) {
                var id = row.cells[1].innerText.toLowerCase();
                var name = row.cells[2].innerText.toLowerCase();

                row.style.display = (id.includes(input) || name.includes(input)) ? "" : "none";
            });
        }

        function assignEmployees() {
            var selected = [];
            document.querySelectorAll(".emp-checkbox:checked").forEach(function (cb) {
                selected.push(cb.value);
            });

            if (selected.length > 0) {
                alert("Assigned to: " + selected.join(", "));
                window.close(); // or redirect back if needed
            } else {
                alert("Please select at least one employee.");
            }
        }
    </script>

    <script>
        window.onload = function () {
            loadEmployeeList();
        };

        function loadEmployeeList() {
            var tbody = document.getElementById("employeeTableBody");
            tbody.innerHTML = "<tr><td colspan='3'>Loading...</td></tr>";

            PageMethods.GetAllEmployees(
                function (response) {
                    tbody.innerHTML = "";
                    response.forEach(function (emp, index) {
                        var row = document.createElement("tr");

                        row.innerHTML = `
                            <td><input type="checkbox" class="emp-checkbox" value="${emp.ID}" /></td>
                            <td>${emp.ID}</td>
                            <td>${emp.Name}</td>
                        `;
                        tbody.appendChild(row);
                    });
                },
                function (err) {
                    tbody.innerHTML = "<tr><td colspan='3'>Error loading data</td></tr>";
                    console.error(err);
                }
            );
        }

        function filterEmployees() {
            var input = document.getElementById("txtSearchEmp").value.toLowerCase();
            var rows = document.querySelectorAll("#employeeTable tbody tr");

            rows.forEach(function (row) {
                var id = row.cells[1].innerText.toLowerCase();
                var name = row.cells[2].innerText.toLowerCase();

                row.style.display = id.includes(input) || name.includes(input) ? "" : "none";
            });
        }

        function assignEmployees() {
            var selected = [];
            var checkboxes = document.querySelectorAll(".emp-checkbox:checked");

            checkboxes.forEach(cb => selected.push(cb.value));

            if (selected.length > 0) {
                alert("Assigned to: " + selected.join(", "));
                window.close();
            } else {
                alert("Please select at least one employee.");
            }
        }
    </script>
</body>
</html>
