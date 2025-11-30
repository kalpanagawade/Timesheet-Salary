<%@ Page Title="Notice" Language="C#" MasterPageFile="~/Site1.master"
    AutoEventWireup="true" CodeBehind="Notice.aspx.cs" Inherits="EmployeeTimesheet_Salary.Notice" %>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <style>
        .styled-grid {
            margin: 20px;
            width: 97.5%;
            border-collapse: collapse;
            border: 1px solid #ccc;
        }

            .styled-grid th {
                background-color: coral;
            }

            .styled-grid td, .styled-grid th {
                padding: 10px;
            }

        header {
            border-radius: 1rem;
            position: fixed;
            width: 97%;
            height: 89px;
            margin: 11px;
            top: 0;
            right: 0;
            z-index: 1000;
            display: flex;
            align-items: center;
            justify-content: space-between;
            background: black;
            box-shadow: 0 8px 11px rgb(14 55 54 / 15%);
            padding: 11px 11px;
            transition: 0.5s;
        }

        .navbar .home-active {
            border-radius: 0.5rem;
            color: var(--bg-color);
        }

        .navbar {
            display: flex;
            column-gap: 0.5rem;
        }

            .navbar a {
                font-size: 1rem;
                font-weight: 500;
                color: var(--text-color);
                padding: 0.5rem 1rem;
            }

                .navbar a:hover,
                .navbar .home-active {
                    padding: 18px;
                    background: var(--green-color);
                    border-radius: 5rem;
                    color: var(--bg-color);
                    transition: background 0.5s;
                }

            .navbar:hover .home-active:hover {
                padding: 14px;
                background: var(--light-green-color);
                border-radius: 1rem;
                color: black;
                transition: background 0.5s;
            }

        .profile {
            display: flex;
            align-items: center;
            column-gap: 0.5rem;
            cursor: pointer;
            color: white;
        }

        .profile-container {
            position: relative;
            display: inline-block;
        }

        .profile-icon {
            display: flex;
            align-items: center;
            cursor: pointer;
            color: white;
        }

        .logout-dropdown {
            display: none;
            position: absolute;
            top: 100%; /* Below the icon */
            left: 0;
            background-color: #333;
            color: white;
            padding: 0.5rem 1rem;
            border-radius: 5px;
            cursor: pointer;
            white-space: nowrap;
            z-index: 10;
        }

            .logout-dropdown:hover {
                background-color: #444;
            }


        :root {
            --green-color: #3cb815;
            --light-green-color: #c0eb7b;
            --orange-color: #ff7e00;
            --light-orange-color: #f75f1d;
            --text-color: #1a2428;
            --bg-color: #fff;
        }

        .logo {
            display: flex;
            align-items: center;
            font-size: 1.1rem;
            font-weight: 600;
            color: var(--text-color);
            column-gap: 5.5rem;
        }

            .logo .bx {
                font-size: 24px;
                color: var(--orange-color)
            }
    </style>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">--%>
        <%--<div>
            <header>
                <img src="../img/logo.png" alt="Logo" style="width: 50px; height: 50px; margin: 0px 0px 0px 8px;" />
                 <div class="navbar">
                    <asp:PlaceHolder ID="phDynamicButtons" runat="server"></asp:PlaceHolder>
                </div>
                <div class="profile-container">
                    <div class="profile-icon" onclick="toggleLogoutDropdown()">
                        <asp:Label ID="txtCreatedBy" runat="server"> </asp:Label>
                        <i class="bx bx-log-out"></i>
                    </div>
                    <div class="logout-dropdown" id="logoutDropdown" onclick="logoutUser()">
                        Logout
                    </div>
                </div>
            </header>
        </div>
        <script type="text/javascript">


            function logoutUser() {
                // Optional: Clear session or perform logout logic here

                // Redirect to login page
                window.location.href = 'Login.aspx'; // Use the correct path to your login page
            }
            function toggleLogoutDropdown() {
                var dropdown = document.getElementById("logoutDropdown");
                dropdown.style.display = (dropdown.style.display === "block") ? "none" : "block";
            }
        </script>--%>
 <asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
<link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
</asp:Content>

        <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.3.0/css/bootstrap.min.css" rel="stylesheet" />

<div class="container mt-4">

    <h2 class="text-center mb-4">Employee Timesheet Dashboard</h2>

    <div class="row">

        <!-- Total Employees -->
        <div class="col-md-3">
            <div class="card shadow p-3 text-center" style="border-left: 5px solid #0d6efd;">
                <i class="fa-solid fa-users fa-2x text-primary"></i>
                <h4 class="mt-2">Employees</h4>
                <asp:Label ID="lblTotalEmployees" runat="server" CssClass="fw-bold fs-4"></asp:Label>
            </div>
        </div>

        <!-- Pending Timesheets -->
        <div class="col-md-3">
            <div class="card shadow p-3 text-center" style="border-left: 5px solid #ffc107;">
                <i class="fa-solid fa-hourglass-half fa-2x text-warning"></i>
                <h4 class="mt-2">Pending Timesheets</h4>
                <asp:Label ID="lblPendingTimesheets" runat="server" CssClass="fw-bold fs-4"></asp:Label>
            </div>
        </div>

        <!-- Manager Approvals -->
        <div class="col-md-3">
            <div class="card shadow p-3 text-center" style="border-left: 5px solid #198754;">
                <i class="fa-solid fa-check-circle fa-2x text-success"></i>
                <h4 class="mt-2">Approvals</h4>
                <asp:Label ID="lblApprovals" runat="server" CssClass="fw-bold fs-4"></asp:Label>
            </div>
        </div>

        <!-- Payroll Pending -->
        <div class="col-md-3">
            <div class="card shadow p-3 text-center" style="border-left: 5px solid #dc3545;">
                <i class="fa-solid fa-money-check-dollar fa-2x text-danger"></i>
                <h4 class="mt-2">Payroll Tasks</h4>
                <asp:Label ID="lblPayrollItems" runat="server" CssClass="fw-bold fs-4"></asp:Label>
            </div>
        </div>

    </div>

</div>
<div class="container mt-4">

    <h2 class="text-center mb-4">Employee Notice</h2>

    <div class="row">

        <!-- Total Employees -->
       <div class="col-md-6">
    <div class="card shadow p-3 text-center" style="border-left: 5px solid #FF5733;">
        <i class="fa-solid fa-users fa-2x text-info"></i>
        <h4 class="mt-2">Employees Holiday</h4>

        <div class="d-flex justify-content-between align-items-center">
            <asp:Button ID="btnPrev" runat="server" Text="<<"
                CssClass="btn btn-outline-primary btn-sm"
                OnClick="btnPrev_Click" />

            <asp:Label ID="LblMonth" runat="server" CssClass="fw-bold fs-4"></asp:Label>

            <asp:Button ID="btnNext" runat="server" Text=">>"
                CssClass="btn btn-outline-primary btn-sm"
                OnClick="btnNext_Click" />
        </div>

        <asp:GridView ID="gvHoliday" runat="server" CssClass="table table-bordered mt-3" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="HolidayDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:BoundField DataField="HolidayName" HeaderText="Holiday Name" />
            </Columns>
        </asp:GridView>
    </div>
</div>

        <div class="col-md-6">
    <div class="card shadow p-3" style="border-left: 5px solid #FF5733;">
        <div class="d-flex justify-content-between align-items-center">
            <i class="fa-solid fa-bell fa-2x text-info"></i>
            <h4 class="mt-2 fw-bold">Notice Board</h4>
        </div>

        <hr />

        <asp:BulletedList 
            ID="lstNotice" 
            runat="server" 
            CssClass="text-start fw-semibold"
            BulletStyle="Disc"
            DisplayMode="Text" 
            Font-Size="Medium">
        </asp:BulletedList>

        <!-- If Admin want to add notice -->
        <div class="mt-3">
            <asp:TextBox ID="txtNotice" runat="server" CssClass="form-control" 
                         Placeholder="Write new notice..."></asp:TextBox>
            <asp:Button ID="btnAddNotice" runat="server" CssClass="btn btn-primary mt-2"
                        Text="Add Notice" OnClick="btnAddNotice_Click" />
        </div>
    </div>
</div>

    </div>
    
</div>


</asp:Content>


<%--    </form>
</body>
</html>--%>
