<%@ Page Title="Team Timesheet" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true" 
    CodeBehind="TeamTimesheet.aspx.cs" Inherits="EmployeeTimesheet_Salary.TeamTimesheet" %>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Team Timesheet</title>--%>

<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
     <style>
        body {
            font-family: Arial, sans-serif;
            background: #f8f9fa;
            margin: 0;
            padding: 20px;
        }
        h2 {
            color: #333;
            margin-bottom: 15px;
        }
        .grid-container {
            background: #fff;
            padding: 15px;
            border-radius: 8px;
            box-shadow: 0 0 8px rgba(0,0,0,0.1);
            margin-bottom: 20px;
        }
        .btn {
            background: #007bff;
            color: white;
            border: none;
            padding: 6px 12px;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn:hover {
            background: #0056b3;
        }
        .btn-danger {
            background: #dc3545;
        }
        .btn-danger:hover {
            background: #a71d2a;
        }
        .month-nav {
            margin: 10px 0;
        }
        .month-label {
            font-weight: bold;
            margin: 0 10px;
        }
        table {
            width: 100%;
        }
        th {
            background: #007bff;
            color: white;
            padding: 8px;
        }
        td {
            padding: 6px;
            border-bottom: 1px solid #ddd;
        }
        .not-filled {
            color: red;
            font-style: italic;
        }
    </style>

</asp:Content>
<%--</head>
<body>
    <form id="form1" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <div class="grid-container">
            <h2>Team Members</h2>
            <asp:GridView ID="gvTeamMembers" runat="server" AutoGenerateColumns="False"
                DataKeyNames="UserId"
                OnRowCommand="gvTeamMembers_RowCommand">
                <Columns>
                    <asp:BoundField DataField="UserId" HeaderText="User ID" />
                    <asp:BoundField DataField="UserLoginName" HeaderText="Name" />
                    <asp:ButtonField CommandName="SelectMember" Text="View Timesheet" />
                </Columns>
            </asp:GridView>
        </div>

        <asp:Button ID="btnBack" runat="server" Text="Back to Team List" CssClass="btn btn-danger"
            OnClick="btnBack_Click" Visible="false" />

        <div class="grid-container" runat="server" id="timesheetSection" visible="false">
            <h2>Timesheet</h2>

            <div class="month-nav">
                <asp:Button ID="btnPrevMonth" runat="server" Text="&lt; Prev" CssClass="btn"
                    OnClick="btnPrevMonth_Click" />
                <span class="month-label" id="lblMonth" runat="server"></span>
                <asp:Button ID="btnNextMonth" runat="server" Text="Next &gt;" CssClass="btn"
                    OnClick="btnNextMonth_Click" />
            </div>

<asp:GridView ID="gvTimesheet" runat="server" AutoGenerateColumns="False" OnRowCommand="gvTimesheet_RowCommand">
    <Columns>
        <asp:BoundField DataField="TaskDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}">
        <ItemStyle Width="9%" />
            <HeaderStyle Width="9%" />
        </asp:BoundField>
        <asp:BoundField DataField="Description" HeaderText="Description" />
        <asp:BoundField DataField="TimeSpent" HeaderText="Hours" >
        <ItemStyle Width="9%" />
            <HeaderStyle Width="9%" />
        </asp:BoundField>
        <asp:BoundField DataField="Type" HeaderText="Type" >
        <ItemStyle Width="9%" />
            <HeaderStyle Width="9%" />
        </asp:BoundField>


        <asp:TemplateField HeaderText="Approval">
            <ItemStyle Width="15%" />
            <ItemTemplate>
                <asp:Button ID="btnApprove" runat="server" Text="Approve" CommandName="Approve" CommandArgument='<%# Eval("TaskID") %>' CssClass="btn btn-success btn-sm" Style="background-color:green" />
                <asp:Button ID="btnNotApprove" runat="server" Text="Not Approve" CommandName="NotApprove" CommandArgument='<%# Eval("TaskID") %>' CssClass="btn btn-danger btn-sm" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

        </div>
        
</asp:Content>
   <%-- </form>
</body>
</html>--%>
