<%@ Page Title="Notice" Language="C#" MasterPageFile="~/Site1.master"
    AutoEventWireup="true" CodeBehind="Notice.aspx.cs" Inherits="EmployeeTimesheet_Salary.Notice" %>

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

