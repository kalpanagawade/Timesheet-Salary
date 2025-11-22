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

   <script type="text/javascript">
       function toggleSelectAll(source) {
           debugger
           // Get all row checkboxes
           var grid = document.getElementById('<%= gvTimesheet.ClientID %>');
           var checkBoxes = grid.querySelectorAll('input[type=checkbox][id*="chkSelect"]');

           // Loop and check/uncheck only enabled checkboxes
           checkBoxes.forEach(function (cb) {
               if (!cb.disabled) {
                   cb.checked = source.checked;
               }
           });
       }

       function updateHeaderState() {
           var header = document.getElementById('<%= gvTimesheet.ClientID %>').querySelector('input[id*="chkSelectAll"]');
    var checkBoxes = document.getElementById('<%= gvTimesheet.ClientID %>').querySelectorAll('input[type=checkbox][id*="chkSelect"]');

    var enabledBoxes = Array.from(checkBoxes).filter(cb => !cb.disabled);
    var checkedCount = enabledBoxes.filter(cb => cb.checked).length;

    if (checkedCount === 0) {
        header.checked = false;
        header.indeterminate = false;
    } else if (checkedCount === enabledBoxes.length) {
        header.checked = true;
        header.indeterminate = false;
    } else {
        header.checked = false;
        header.indeterminate = true;
    }
}

function attachHandlers() {
    var grid = document.getElementById('<%= gvTimesheet.ClientID %>');
           if (!grid) return;

           var header = grid.querySelector('input[id*="chkSelectAll"]');
           var checkBoxes = grid.querySelectorAll('input[type=checkbox][id*="chkSelect"]');

           if (header) {
               header.onclick = function () { toggleSelectAll(header); };
           }

           checkBoxes.forEach(function (cb) {
               cb.onchange = updateHeaderState;
           });

           updateHeaderState();
       }

       // Run when page fully loads
       document.addEventListener('DOMContentLoaded', attachHandlers);

       // Handle UpdatePanel refresh (if exists)
       if (window.Sys && Sys.WebForms && Sys.WebForms.PageRequestManager) {
           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(attachHandlers);
       }
   </script>


</asp:Content>
<%--</head>
<body>
    <form id="form1" runat="server">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <div class="grid-container">
            <asp:Label runat="server" ID="id_name"><h2>Team Members</h2></asp:Label>
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

          <%-- <asp:GridView ID="gvTimesheet" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvTimesheet_RowDataBound">
    <Columns>
        <asp:TemplateField HeaderText="Select">
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" />
            </HeaderTemplate>
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
                <asp:HiddenField ID="hfTaskID" runat="server" Value='<%# Eval("TaskID") %>' />
                <asp:HiddenField ID="hfDescription" runat="server" Value='<%# Eval("Description") %>' />
            </ItemTemplate>
            <ItemStyle Width="5%" />
        </asp:TemplateField>

        <asp:BoundField DataField="TaskDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
        <asp:BoundField DataField="Description" HeaderText="Description" />
        <asp:BoundField DataField="TimeSpent" HeaderText="Hours" />
        <asp:BoundField DataField="Type" HeaderText="Type" />

    </Columns>
</asp:GridView>--%>

        <asp:GridView ID="gvTimesheet" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvTimesheet_RowDataBound">
<Columns>
    <asp:TemplateField HeaderText="Select">
        <HeaderTemplate>
            <asp:CheckBox ID="chkSelectAll" runat="server" />
        </HeaderTemplate>
        <ItemTemplate>
            <asp:CheckBox ID="chkSelect" runat="server" />
            <asp:HiddenField ID="hfTaskID" runat="server" Value='<%# Eval("TaskID") %>' />
            <asp:HiddenField ID="hfDescription" runat="server" Value='<%# Eval("Description") %>' />
            <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("Status") %>' />
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Date">
        <ItemTemplate>
            <%# Eval("TaskDate", "{0:dd-MMM-yyyy}") %>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Description">
        <ItemTemplate>
            <%# Eval("Description") %>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Hours">
        <ItemTemplate>
            <%# Eval("TimeSpent") %>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Type">
        <ItemTemplate>
            <%# Eval("Type") %>
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="Status">
        <ItemTemplate>
            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>

</Columns>
</asp:GridView>


<div style="margin-top:10px;">
    <asp:Button ID="btnApproveSelected" runat="server" Text="Approve Selected" CssClass="btn btn-success"
        OnClick="btnApproveSelected_Click"  style="background-color:green" />
    <asp:Button ID="btnRejectSelected" runat="server" Text="Reject Selected" CssClass="btn btn-danger"
        OnClick="btnRejectSelected_Click" />
</div>


        </div>
        
</asp:Content>
   <%-- </form>
</body>
</html>--%>
