<%@ Page Title="Salary Module" Language="C#"
 MasterPageFile="~/Site1.master"
 AutoEventWireup="true"
 CodeBehind="SalaryModule.aspx.cs"
 Inherits="EmployeeTimesheet_Salary.SalaryModule" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-3">

<h4>Employee List</h4>

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

<!-- EMPLOYEE GRID (UNCHANGED) -->
<div class="table-responsive">
<asp:GridView ID="gvEmployees" runat="server" AutoGenerateColumns="False"
 OnRowCommand="gvEmployees_RowCommand"
 AllowPaging="true" PageSize="10"
 OnPageIndexChanging="gvEmployees_PageIndexChanging"
 CssClass="table table-bordered table-striped">
<Columns>
 <asp:TemplateField HeaderText="User ID">
  <ItemTemplate>
   <asp:LinkButton ID="lnkUserId" runat="server"
    Text='<%# Eval("UserId") %>'
    CommandName="ShowDetails"
    CommandArgument='<%# Eval("UserId")+"|"+Eval("UserLoginName") %>' />
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


<hr />

<!-- SALARY PANEL -->
<asp:Panel ID="pnlSalary" runat="server" Visible="false">

<h5><asp:Label ID="lblUser" runat="server" /></h5>

<asp:DropDownList ID="ddlYear" runat="server"
 AutoPostBack="true"
 OnSelectedIndexChanged="FilterChanged"
 CssClass="form-control w-25 mb-2" />

<div style="display:flex;">
<div style="width: 80%;margin-right: -38%;">
<table class="table w-50">
<tr><td>Basic</td><td><asp:TextBox ID="txtBasic" runat="server" /></td></tr>
<tr><td>HRA</td><td><asp:TextBox ID="txtHRA" runat="server" /></td></tr>
<tr><td>Allowance</td><td><asp:TextBox ID="txtAllowance" runat="server" /></td></tr>
<tr><td>Bonus</td><td><asp:TextBox ID="txtBonus" runat="server" /></td></tr>
</table>
        </div>
        <div style="display:grid;">
        <asp:Label ID="lblAnnual" runat="server" CssClass="fw-bold h5" />
        <asp:Label ID="lblNetSalary" runat="server" CssClass="fw-bold h5" />
        <asp:Label ID="lblMonSalary" runat="server" CssClass="fw-bold h5" />
        <%--<asp:Button ID="btnSaveSalary" runat="server" Text="Save Salary" CssClass="btn btn-success" OnClick="btnSaveSalary_Click" />--%>
<asp:Label ID="Label1" runat="server" CssClass="text-success" />
        </div>
        </div>
<asp:Button ID="btnSaveSalary" runat="server"
 Text="Save Yearly Salary"
 CssClass="btn btn-success"
 OnClick="btnSaveSalary_Click" />

<hr />

    <h5 class="mt-3">Approved Task Entries</h5>

<asp:DropDownList ID="ddlMonth" runat="server"
 AutoPostBack="true"
 OnSelectedIndexChanged="FilterChanged"
 CssClass="form-control w-25 mb-2" />

<asp:TextBox ID="txtDeduction" runat="server"
 ReadOnly="true" CssClass="form-control w-25 mb-2" />

<asp:Button ID="btnReleaseSalary" runat="server"
 Text="Release Salary"
 CssClass="btn btn-danger"
 OnClick="btnReleaseSalary_Click" />

<br />
<asp:Label ID="lblMessage" runat="server" CssClass="fw-bold text-success" />


            <div class="table-responsive">
          <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="true" 
    OnRowDataBound="gvTasks_RowDataBound" CssClass="table table-sm table-bordered" />

        </div>
</asp:Panel>

</div>
    </div>
</asp:Content>
