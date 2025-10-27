<%@ Page Title="Salary Module" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" CodeBehind="SalaryModule.aspx.cs" 
    Inherits="EmployeeTimesheet_Salary.SalaryModule" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css">

    <style>
        header {
            width: 98.5% !important;
        }
        th {
            text-align: center !important;
        }

        /* GridView table styling */
        .table thead th {
            background-color: #007BFF;
            color: white;
            text-align: center;
        }

        .table tbody td {
            text-align: center;
        }

        .link-btn {
            color: #007BFF;
            text-decoration: underline;
            cursor: pointer;
            background: none;
            border: none;
            padding: 0;
        }

        .search-box {
            margin-bottom: 20px;
        }

        .search-box input {
            width: 200px;
            display: inline-block;
            margin-right: 10px;
        }

        .salary-panel {
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 8px;
            border: 1px solid #dee2e6;
            margin-top: 20px;
        }

        .salary-panel h5 {
            margin-bottom: 15px;
        }

        .salary-panel table td {
            padding: 5px 10px;
        }

        .fw-bold {
            font-weight: 600;
        }

        /* Responsive small tables */
        @media (max-width: 768px) {
            .table-responsive {
                overflow-x: auto;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

  <div style="padding:1%">

    <div runat="server" id="SalMdlDiv" style="display:block;">
    <h2 class="mb-4">Salary Module</h2>
    <!-- Search Section -->
    <div class="search-box mb-3">
        <asp:Label ID="lblEmpId" runat="server" Text="Employee ID:" AssociatedControlID="txtSearchId" CssClass="me-2" />
        <asp:TextBox ID="txtSearchId" runat="server" CssClass="form-control d-inline w-auto me-3" />

        <asp:Label ID="lblEmpName" runat="server" Text="Employee Name:" AssociatedControlID="txtSearchName" CssClass="me-2" />
        <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control d-inline w-auto me-3" />

        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary me-2" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-secondary" OnClick="btnClear_Click" />
    </div>

    <!-- Employee Grid -->
    <div class="table-responsive">
        <asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False"
            OnRowCommand="gvEmployees_RowCommand" 
            DataKeyNames="UserId"
            AllowPaging="True" PageSize="10"
            OnPageIndexChanging="gvEmployees_PageIndexChanging"
            CssClass="table table-striped table-bordered">

            <Columns>
                <asp:TemplateField HeaderText="User ID">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkUserId" runat="server"
                            Text='<%# Eval("UserId") %>'
                            CommandName="ShowDetails"
                            CommandArgument='<%# Eval("UserId") + "|" + Eval("UserLoginName") %>'
                            CssClass="link-btn">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="UserLoginName" HeaderText="User Name" />
                <asp:BoundField DataField="UserEmailId" HeaderText="Email" />
            </Columns>
        </asp:GridView>
    </div>

    </div>

    <div runat="server" id="DtlSalDiv" style="display:none;padding:1%">

    <asp:Button ID="btnBack" runat="server" Text="← Back to Employee List" CssClass="btn btn-secondary mb-3" OnClick="btnBack_Click" />

    <!-- Details Panel -->
    <asp:Panel ID="pnlDetails" runat="server" Visible="false" CssClass="salary-panel">
        <h4><asp:Label ID="lblUser" runat="server" /></h4>

        <h5 class="mt-3">Approved Task Entries</h5>
        <div class="table-responsive">
            <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="true" CssClass="table table-sm table-bordered" />
        </div>

        <h5 class="mt-3">Salary Details</h5>
        <div style="display:flex;">
        <div style="width: 80%;margin-right: -38%;">
        <table class="table table-bordered w-50">
            <tr><td>Basic Salary:</td><td><asp:TextBox ID="txtBasic" runat="server" CssClass="form-control" /></td></tr>
            <tr><td>HRA:</td><td><asp:TextBox ID="txtHRA" runat="server" CssClass="form-control" /></td></tr>
            <tr><td>Allowance:</td><td><asp:TextBox ID="txtAllowance" runat="server" CssClass="form-control" /></td></tr>
            <tr><td>Deductions:</td><td><asp:TextBox ID="txtDeduction" runat="server" CssClass="form-control" /></td></tr>
            <tr><td>Bonus:</td><td><asp:TextBox ID="txtBonus" runat="server" CssClass="form-control"  /></td></tr>
        </table>
        </div>
        <div style="display:grid;">
        <asp:Label ID="lblAnnual" runat="server" CssClass="fw-bold h5" />
        <asp:Label ID="lblNetSalary" runat="server" CssClass="fw-bold h5" />
        <asp:Label ID="lblMonSalary" runat="server" CssClass="fw-bold h5" />
        <asp:Button ID="btnSaveSalary" runat="server" Text="Save Salary" CssClass="btn btn-success" OnClick="btnSaveSalary_Click" />
<asp:Label ID="lblMessage" runat="server" CssClass="text-success" />
        </div>
        </div>
    </asp:Panel>


    </div>

  </div>

</asp:Content>
