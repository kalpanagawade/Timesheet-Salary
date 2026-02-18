<%@ Page Title="Employee Details" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true"
    CodeBehind="CreateUpdateUser.aspx.cs" Inherits="EmployeeTimesheet_Salary.CreateUpdateUser"  %>

<%--<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" />
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>--%>


<asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" />
<link rel="stylesheet" href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css">
<script src="https://code.jquery.com/jquery-3.6.4.min.js"></script>
<script src="https://code.jquery.com/ui/1.13.2/jquery-ui.min.js"></script>

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" />

<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <style>
        .styled-grid {
            margin: 20px;
            width: 97.5%;
            border-collapse: collapse;
            border: 1px solid #ccc;
        }

            .styled-grid th {
                background-color: #007bff;
                color: white;
/*                padding: 8px;
                text-align:center;*/
            }

            .styled-grid td, .styled-grid th {
                padding: 10px;
            }

            header{
                width:98% !important;
            }

        header1 {
    border-radius: 1rem;
    /*position: fixed;*/
    width: 95.5%;
    height: 5%;
    margin: 11px 35px;
    top: 78px;
    /* right: 0; */
    z-index: 1000;
    display: flex;
    align-items: center;
    justify-content: space-between;
    background: silver;
    box-shadow: 0 8px 11px rgb(14 55 54 / 15%);
    padding: 5px 25px;
    transition: 0.5s;
        }

        .navbar .home-active {
            border-radius: 0.5rem;
            color: var(--bg-color);
        }

        .navbar {
            display: flex;
            column-gap: 0.5rem;
            flex-wrap: nowrap;
        }

            .navbar a {
                font-size: 1rem;
                font-weight: 500;
                color: var(--text-color);
                padding: 0.5rem 1rem;
            }

                .navbar a:hover,
                .navbar .home-active {
                    padding: 3% 14%;
                    background: var(--green-color);
                    border-radius: 5rem;
                    color: var(--bg-color);
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

        .Bigbox {
            background-color: snow;
            width: 100%;
            /*height: 840px;
            justify-items: anchor-center;
            align-content: space-around;*/
        }

        .Bigbox1 {
            background-color: snow;
            width: 100%;
            /*height: 840px;
            justify-items: anchor-center;
            align-content: space-around;*/
        }

        .CreateUser {
            border-style: solid;
            border-color: black;
            width: 98%;
            height: 70%;
            margin:1%;
        }

        .CreateUser1 {
            /*border-style: solid;
            border-color: blue*/;
            width: 100%;
            height: 8%;
            background-color: darkslateblue;
        }

        .CreateUser2 {
            /*border-style: solid;*/
            width: 100%;
            background-color: blanchedalmond;
            height: 10%;
            display: flex; /* Use Flexbox to arrange items in a row */
            justify-content: flex-start;
        }

        .smallbx {
            border-style: solid;
            border-color: black;
            border-radius: 21px;
            width: 16%; /* Adjust width to be in a row */
            height: 41px; /* You can adjust this height based on your preference */
            margin: 9px;
            /*padding: 19px;*/
            box-sizing: border-box; /* Ensures padding doesn't affect the box's size */
            display: flex; /* Use Flexbox to center text inside the box */
            justify-content: center; /* Center text horizontally */
            align-items: center; /* Center text vertically */
        }

            .smallbx:hover {
                background-color:  #198754;
                color:#fff;
                transition: 0.2s all linear;
            }

        .CreateUser3 {
            /*border-style: solid;*/
            width: 100%;
            /*border-color: red;*/
            height: 42%;
            justify-content: flex-start;
            text-align: left;
            display: inline-block; /* Align items in a row */
            margin-right: 20px;
        }

        .row {
            display: block; /* Align items in a row */
            /*margin-right: 20px;*/
            margin: 20px;
        }

        .col {
            display: inline-block; /* Align items in a row */
            margin-right: 20px;
            border-radius: 15px;
            width: 330px;
            height: 25px;
        }

        .BtnCls {
            display: inline-block; /* Align items in a row */
            margin-right: 20px;
            border-radius: 15px;
            width: 116px;
            height: 25px;
        }

        #UserEnquiry {
            pageinde border-style: solid;
            border-color: blueviolet;
            width: 99%;
            height: 45px;
            margin: 8px;
            padding: 6px;
            box-sizing: border-box;
            display: flex;
        }

        #CreateUser4 {
            border-style: solid;
            width: 98%;
            border-color: red;
            height: 24%;
            margin: 1%;
            display: flex;
            justify-content: center;
        }

        .row1 {
            display: block; /* Align items in a row */
            /*margin-right: 20px;*/
            margin: 20px;
        }

        .col1 {
            display: inline-block; /* Align items in a row */
            margin-right: 20px;
        }

        .no-Link-style {
            color: white;
            text-decoration: none;
            cursor: pointer;
        }
        @keyframes blinkBorder {
    0%   { border-color: red; }
    50%  { border-color: transparent; }
    100% { border-color: red; }
}

.blink {
    animation: blinkBorder 0.8s linear infinite;
}
    </style>

            <script type="text/javascript">

                function validateDesgition() {
                    var ddlRole = document.getElementById('<%= ddlRole.ClientID %>').value;
                    if (ddlRole === "") {
                        alert("Please select a role.");
                        return false;
                    }

                    return true;
                }
                function validateEmail() {
                    var email = document.getElementById('<%= txtEmail.ClientID %>').value;
                    var userNameTxt = document.getElementById('<%= txtUserName.ClientID %>');
                    var userName = userNameTxt.value;
                    userNameTxt.addEventListener("input", function () {
                        this.style.border = "";
                        this.classList.remove("blink");
                    });
                    var passwordTxt = document.getElementById('<%= txtPassword.ClientID %>');
                    var password = passwordTxt.value;
                    passwordTxt.addEventListener("input", function () {
                        this.style.border = "";
                        this.classList.remove("blink");
                    });
                    var confirmPasswordTxt = document.getElementById('<%= txtConfirmPassword.ClientID %>');
                    var confirmPassword = confirmPasswordTxt.value;
                    confirmPasswordTxt.addEventListener("input", function () {
                        this.style.border = "";
                        this.classList.remove("blink");
                    });
                var ddlStatus = document.getElementById('<%= ddlStatus.ClientID %>').value;
                <%--var ddlRole = document.getElementById('<%= ddlRole.ClientID %>').value;--%>

                var radioInternal = document.getElementById('<%= RDOIUsrTyp.ClientID %>');
                var radioExternal = document.getElementById('<%= RDOEUsrTyp.ClientID %>');

                var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

                if (!radioInternal.checked && !radioExternal.checked) {
                    alert("Please select a user type.");
                    return false;
                }
                if (userName.trim() === "") {
                    alert("User Name is required.");
                    userNameTxt.style.border = "2px solid red";
                    userNameTxt.classList.add("blink");
                    /*userNameTxt.focus();*/
                    return false;
                }
                if (password.trim() === "" || confirmPassword.trim() === "") {
                    alert("Password and Confirm Password are required.");
                    confirmPasswordTxt.style.border = "2px solid red";
                    confirmPasswordTxt.classList.add("blink");
                    passwordTxt.style.border = "2px solid red";
                    passwordTxt.classList.add("blink");
                    return false;
                }
                if (password !== confirmPassword) {
                    alert("Passwords do not match.");
                    confirmPasswordTxt.style.backgroundColor = "Pink";
                    confirmPasswordTxt.classList.add("blink");
                    passwordTxt.style.backgroundColor = "lightgreen";
                    passwordTxt.classList.add("blink");
                    return false;
                }
                if (email.trim() === "") {
                    alert("Email ID is required.");
                    return false;
                }
                if (!emailPattern.test(email)) {
                    alert("Please enter a valid Email ID.");
                    return false;
                }
                if (ddlStatus === "") {
                    alert("Please select a status.");
                    return false;
                }
                //if (ddlRole === "") {
                //    alert("Please select a role.");
                //    return false;
                //}

                return true; // ✅ Allow server-side method to run
            }
                function toggleDivs(hideDivId, showDivId) {
                    debugger
                // Hide the div with the ID hideDivId
                //document.getElementById(hideDivId).style.display = 'none';

                //// Show the div with the ID showDivId
                    //document.getElementById(showDivId).style.display = 'block';
                    var hideDiv = document.getElementById(hideDivId);
                    var showDiv = document.getElementById(showDivId);

                    if (hideDiv && showDiv) {
                        hideDiv.style.display = 'none';
                        showDiv.style.display = 'block';
                    } else {
                        console.error("Could not find divs:", hideDivId, showDivId);
                    }
            }
            function logoutUser() {
                // Optional: Clear session or perform logout logic here

                // Redirect to login page
                window.location.href = 'Login.aspx'; // Use the correct path to your login page
            }
            function toggleLogoutDropdown() {
                var dropdown = document.getElementById("logoutDropdown");
                dropdown.style.display = (dropdown.style.display === "block") ? "none" : "block";
            }

            //function logoutUser() {
            //    // Optional: Clear session or cookies
            //    window.location.href = 'Login.aspx'; // Adjust path if needed
            //}

            // Optional: Close dropdown when clicking outside
            window.addEventListener("click", function (e) {
                if (!e.target.closest(".profile-container")) {
                    document.getElementById("logoutDropdown").style.display = "none";
                }
            });

            $(function () {
                $('#<%= txtDOBED.ClientID %>').datepicker({
                    dateFormat: 'd M yy', // or 'd M yy' for 1 Jan 1900
                    changeMonth: true,
                    changeYear: true,
                    yearRange: '1900:2050'
                });
            });



            function confirmEdit() {
                return confirm("Are you sure you want to update the iUser table?");
            }

            $(document).ready(function () {
                /* debugger*/
                $('body').on('change', 'input[type="checkbox"]', function () {
                    var isChecked = $(this).is(':checked');

                    // Select/Deselect all children
                    $(this).closest('li').find('input[type="checkbox"]').prop('checked', isChecked);

                    // Select all parents if checked
                    if (isChecked) {
                        $(this).parents('li').children('input[type="checkbox"]').prop('checked', true);
                    } else {
                        // Uncheck parent only if no siblings are checked
                        $(this).parents('ul').each(function () {
                            var anyChecked = $(this).find('> li > input[type="checkbox"]:checked').length > 0;
                            if (!anyChecked) {
                                $(this).prev('input[type="checkbox"]').prop('checked', false);
                            }
                        });
                    }
                });
            });

            function gatherSelectedModules() {
                var selected = [];
                document.querySelectorAll("input[type='checkbox'][id^='chk_']:checked").forEach(function (cb) {
                    var id = cb.id.replace("chk_", ""); // extract ModuleID from checkbox id
                    selected.push(id);
                });

                // Set hidden field value as comma-separated IDs
                document.getElementById('<%= hfSelectedModules.ClientID %>').value = selected.join(',');
                //document.getElementById('<%= HiddenField1.ClientID %>').value = selected.join(',');
                
                return true; // allow form submission
            }
            //Added bY hrutik
            <%--function onRoleChange() {
                var ddl = document.getElementById('<%= ddlRole.ClientID %>');
                var selectedText = ddl.options[ddl.selectedIndex].text;

                if (selectedText.toLowerCase().includes('manager')) {
                    window.open('AssignEmployees.aspx', '_blank', 'width=900,height=600'); // opens new popup
                }
            }--%>

            function onRoleChange() {
                var ddl = document.getElementById('<%= ddlRole.ClientID %>');
        var selectedText = ddl.options[ddl.selectedIndex].text;

        if (selectedText.toLowerCase().includes('manager')) {
                     var userId = document.getElementById('<%= txtUserid.ClientID %>').value;
                        window.open('AssignEmployees.aspx?UserId=' + encodeURIComponent(userId), '_blank', 'width=900,height=600');
                    }
                }

            function onRoleChangeEdit() {
                    var ddl = document.getElementById('<%= DropDownList4.ClientID %>');
                var selectedText = ddl.options[ddl.selectedIndex].text;

                    if (selectedText.toLowerCase().includes('manager')) {
                        var userId = document.getElementById('<%= txtUseridED.ClientID %>').value;
                        window.open('AssignEmployees.aspx?UserId=' + encodeURIComponent(userId), '_blank', 'width=900,height=600');
                    }
                }



            </script>
</asp:Content>

<%--</head>
<body>
    <form id="form1" runat="server">--%>
        <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div>
            <header1>
                <%--<img src="../img/logo.png" alt="Logo" style="width: 50px; height: 50px; margin: 0px 0px 0px 8px;" />
                <div class="navbar">
                    <asp:Button
                        runat="server"
                        ID="btnEmpDts" class="home-active" Text="Employee Details" Style="padding: 18px;"
                        OnClick="btnEmpDts_Click" />
                </div>--%>
                
                <div class="navbar">


                    <asp:Button runat="server" ID="btnsrh" class="home-active" Text="Search Employee"
                        OnClientClick="document.getElementById('MainContent_Bigbox').style.display='none';
                     document.getElementById('MainContent_Bigbox1').style.display='block'; return false;" />
                    <asp:Button runat="server" ID="btncre" class="home-active" Text="Create Employee"
                        OnClientClick="document.getElementById('MainContent_Bigbox1').style.display='none';
                     document.getElementById('MainContent_Bigbox').style.display='block'; return false;" />

                </div>
                 <%--              <div class="profile-container">
                    <div class="profile-icon" onclick="toggleLogoutDropdown()">
                        <asp:Label ID="txtCreatedBy" runat="server"> </asp:Label>
                        <i class="bx bx-log-out"></i>
                    </div>
                    <div class="logout-dropdown" id="logoutDropdown" onclick="logoutUser()">
                        Logout
                    </div>
                </div>--%>



            </header1>

            <div id="Bigbox" class="Bigbox" runat="server">
                <div id="cr" class="CreateUser">
                    <div id="Cr1" class="CreateUser1" style="padding: 11px -1px 0px 12px; color: white;height: 26px;">
                        Create Employee
                    </div>
                    <div id="cr2" class="CreateUser2">
                        <!-- Employee Details button (always enabled) -->
                        <asp:Button runat="server" ID="btnem" Text="Employee Details" CssClass="smallbx"
                            OnClientClick="toggleDivs('MainContent_cr3B', 'MainContent_cr3A'); return false;" />

                        <asp:Button runat="server" ID="btnsersac" Text="Service Sanctioning" CssClass="smallbx"
                            OnClientClick="toggleDivs('MainContent_cr3A', 'MainContent_cr3B'); return false;" />



                        <asp:Label ID="Lbl1" runat="server" CssClass="text-end d-block" Text="Employee Id :-"
                            Style="padding-left: 814px;"></asp:Label>
                        <asp:Label ID="Lblid" runat="server" CssClass="text-end d-block"></asp:Label>
                    </div>
                    <div id="cr3A" class="CreateUser3" runat="server" style="display: block;">
                        <div class="row" id="rowno1">
                            <asp:RadioButton ID="RDOIUsrTyp" runat="server" CssClass="col" GroupName="Options" Text="Internal Employee Type" />
                            <asp:RadioButton ID="RDOEUsrTyp" runat="server" CssClass="col" GroupName="Options" Text="External  Employee Type" />

                        </div>
                        <div class="row" id="rowno2">
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="col" placeholder=" Employee Name" ></asp:TextBox>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="col">
                                <asp:ListItem Value="">-- Select Status --</asp:ListItem>
                                <asp:ListItem>Active</asp:ListItem>
                                <asp:ListItem>Inactive</asp:ListItem>
                            </asp:DropDownList>

                            <asp:Label ID="lblUserid" runat="server" CssClass="col" Text=" Employee ID :-"></asp:Label>
                            <asp:TextBox ID="txtUserid" runat="server" CssClass="col" Text=" Employee Id" Enabled="false"></asp:TextBox>
                        </div>
                        <div class="row" id="rowno3">
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="col" placeholder="Password" TextMode="Password"></asp:TextBox>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="col" placeholder="Confirm Password" TextMode="Password"></asp:TextBox>


                            <asp:Label ID="lblDOB" runat="server" CssClass="col" Text="DOB :-"></asp:Label>
                            <%-- <asp:TextBox runat="server" CssClass="col" ID="txtDOB"></asp:TextBox>--%>
                            <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

                            <asp:ScriptManager ID="ScriptManager1" runat="server" />

                            <asp:TextBox runat="server" CssClass="col" ID="txtDOB" Text="DOB"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender
                                ID="CalendarExtender1"
                                runat="server"
                                TargetControlID="txtDOB"
                                Format="d MMM yyyy" />
                        </div>
                        <div class="row" id="rowno4">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="col" placeholder="Enter Email Id"></asp:TextBox>
                            <%--<asp:Label ID="lblmobno" runat="server" CssClass="col" Text="Mobile Number :-"></asp:Label>--%>
                            <asp:TextBox runat="server" CssClass="col" ID="txtMobile" placeholder="Mobile Number"></asp:TextBox>
                            <%--comment & added by Hrutik--%>
                            
                        </div>
                        <div class="row" id="rowno5">
                            <div class="col" style="width:100%; text-align: center;" >
                            <asp:Button ID="btnSave" runat="server" CssClass="BtnCls" Text="Save" OnClientClick="return validateEmail();document.getElementById('MainContent_Bigbox1').style.display='none';
  document.getElementById('MainContent_Bigbox').style.display='block'; return false;" OnClick="btnSave_Click" />
                            <asp:Button ID="btnCanl" runat="server" CssClass="BtnCls" Text="Cancel" OnClientClick="document.getElementById('MainContent_Bigbox1').style.display='none';
  document.getElementById('MainContent_Bigbox').style.display='block'; return false;" />
                                </div>

                        </div>

                        <div class="row" id="rowno6"  runat="server">
                            <div class="col" style="width:100%; text-align: center;" >
                           <asp:Label ID="lblDesi" runat="server" CssClass="col" style="width: 109px;" Text="Designation :-"></asp:Label>
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="col" style="width: 200px;" AutoPostBack="false" onchange="onRoleChange()" />
                            
                            
                            <%--<asp:Button ID="btnOpenEmployeePopup" runat="server" Style="display: none;" OnClientClick="showEmployeePopup(); return false;" />
<asp:Label ID="lblDesi" runat="server" CssClass="col" Text="Designation :-"></asp:Label>
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="col"  AutoPostBack="false" onchange="onRoleChange()">
                                </asp:DropDownList>--%>
                            <%--<asp:ListItem Value="">-- Select Role --</asp:ListItem>
                                <asp:ListItem>Developer</asp:ListItem>
                                <asp:ListItem>Support</asp:ListItem>
                                <asp:ListItem>Tester</asp:ListItem>--%>
                            <asp:Button ID="BtnDesAssign" runat="server" CssClass="BtnCls" Text="SaveRole" OnClientClick="return validateDesgition();document.getElementById('MainContent_Bigbox1').style.display='none';
  document.getElementById('MainContent_Bigbox').style.display='block'; return false;" OnClick="btnSaveRole_Click" />
                            </div>
                            
                         </div>

                        <%--Added by hrutik--%>
                        <!-- Added by Hrutik -->
                        <!-- Added by Hrutik -->

<%--<div class="modal fade" id="employeeModal" tabindex="-1" role="dialog" aria-labelledby="employeeModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg" role="document">
        <div class="modal-content">

            <!-- Modal Header -->
            <div class="modal-header">
                <h5 class="modal-title" id="employeeModalLabel">Assign Employees under Manager</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>

            <!-- Modal Body -->
            <div class="modal-body">
    <!-- Search box -->
    <input type="text" id="txtSearchEmp" class="form-control" placeholder="Search by name or ID..." onkeyup="filterEmployees()" />

    <!-- Employee table -->
    <div style="max-height: 300px; overflow-y: auto; margin-top: 10px;">
        <table class="table table-bordered table-striped" id="employeeTable">
            <thead class="table-light">
                <tr>
                    <th>Select</th>
                    <th>Employee ID</th>
                    <th>Employee Name</th>
                </tr>
            </thead>
            <tbody id="employeeTableBody">
                <!-- Dynamic rows go here -->
            </tbody>
        </table>
    </div>
</div>


            <!-- Modal Footer -->
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" onclick="assignEmployees()">Assign</button>
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
            </div>

        </div>
    </div>
</div>--%>


                    </div>

                    <div id="cr3B" class="CreateUser3" runat="server" style="display: none">
                        Service Sanctioning
                        <asp:Literal ID="litModuleTree" runat="server"></asp:Literal>
                        <div class="col" style="width: 39%;"></div>
                        <asp:Button ID="BtnSen" runat="server" CssClass="BtnCls" Text="Save" OnClick="btnSen_Click" OnClientClick="return gatherSelectedModules();" />
                        <asp:HiddenField ID="hfSelectedModules" runat="server" />

                    </div>


                </div>

            </div>
            <div id="Bigbox1" class="Bigbox1" runat="server">
                <%--style="display: none"--%>
                <div id="crb1" class="CreateUser">
                    <div id="cr1b1" class="CreateUser1" style="padding: 11px -1px 0px 12px; color: white;height: 26px;background-color: brown;">
                        <asp:LinkButton runat="server" ID="Label4" Text="Search Employee" OnClick="CancelED_Click" CssClass="no-Link-style"> </asp:LinkButton>
                        <asp:Label runat="server" ID="lbEd" Text=""></asp:Label>



                        <%-- <asp:Button ID="Button1" runat="server" Text="Edit"
    OnClientClick="return confirmEdit();" OnClick="btnEdit_Click" />--%>
                    </div>

                    <div id="Smallboxcr" runat="server">
                        <div id="cr2b1" class="CreateUser2" style="display: flex; justify-content: space-between; align-items: center; width: 100%;">
<div class="row1" id="Udetail">
    <asp:TextBox runat="server" Class="col" ID="txtuidsrh" placeholder="Employee ID"></asp:TextBox>
    <asp:TextBox runat="server" Class="col" ID="txtunamsrh" placeholder="Employee Name"></asp:TextBox>
    <asp:DropDownList ID="DropDownList2" runat="server" CssClass="col">
        <asp:ListItem Value="">-- All (Status) --</asp:ListItem>
        <asp:ListItem Value="Active">Active</asp:ListItem>
        <asp:ListItem Value="Inactive">Inactive</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        <asp:ListItem Text="5" Value="5" />
        <asp:ListItem Text="10" Value="10" />
        <asp:ListItem Text="20" Value="20" />
        <asp:ListItem Text="50" Value="50" />
    </asp:DropDownList>


</div>
<div class="col" style="width:100%; text-align: center;">

<asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" class="BtnCls" />
</div>
                        </div>

                        <div id="cr3b1" class="CreateUser3">
                            
                            <div class="row" id="Udetail1">
                                

                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                    OnRowCommand="GridView1_RowCommand"
                                    CssClass="styled-grid" OnPageIndexChanging="GridView1_PageIndexChanging">
                                    <Columns>

                                        <asp:BoundField DataField="UserId" HeaderText="Employee ID" />
                                        <asp:BoundField DataField="UserLoginName" HeaderText="Login Name" />
                                        <asp:BoundField DataField="Status" HeaderText=" Active " />
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <%--<asp:Button ID="btnAction" runat="server" Text="Edit"
                                                    CommandName="CustomClick"
                                                    CommandArgument='<%# Eval("UserId") %>' />--%>
                                                <asp:LinkButton ID="btnAction" runat="server"
                                                    CssClass="btn btn-sm btn-outline-primary"
                                                    CommandName="CustomClick"
                                                    CommandArgument='<%# Eval("UserId") %>'
                                                    ToolTip="Edit">
                                                    <i class="bi bi-pencil-square"></i>
                                                </asp:LinkButton>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>



                            </div>
                        </div>
                    </div>
                    <div id="Smallbox" runat="server">
                        <div id="ED2" class="CreateUser2" style="background-color:khaki;">

                            <asp:Button runat="server" Text="Employee Details" ID="EDuserdetail" CssClass="smallbx"
                                OnClientClick="toggleDivs('MainContent_ED3B', 'MainContent_ED3A'); return false;"></asp:Button>

                            <asp:Button runat="server" Text="Service Sanctioning" ID="EDsersec" CssClass="smallbx"
                                OnClientClick="toggleDivs('MainContent_ED3A', 'MainContent_ED3B'); return false;"></asp:Button>

                            <asp:Label ID="Label5" runat="server" CssClass="text-end d-block" Text="Employee Id :-"
                                Style="padding-left: 814px;"></asp:Label>
                            <asp:Label ID="Label6" runat="server" CssClass="text-end d-block"></asp:Label>

                        </div>
                        <div id="ED3A" runat="server" class="CreateUser3" style="display: block;">
                            <div class="row" id="EDrowno1">
                                <asp:RadioButton ID="RadioButton1" runat="server" CssClass="col" GroupName="Options" Text="Internal User Type" />
                                <asp:RadioButton ID="RadioButton2" runat="server" CssClass="col" GroupName="Options" Text="External User Type" />

                            </div>
                            <div class="row" id="EDrowno2">
                                <asp:TextBox ID="txtUserLoginNameED" runat="server" CssClass="col" placeholder="User Name"></asp:TextBox>
                                <asp:DropDownList ID="DropDownList3" runat="server" CssClass="col" Enabled="true">
                                    <asp:ListItem Value="">-- Select Status -- </asp:ListItem>
                                    <asp:ListItem>Active</asp:ListItem>
                                    <asp:ListItem>Inactive</asp:ListItem>
                                </asp:DropDownList>
                                <asp:Label ID="Label1" runat="server" CssClass="col" Text="Employee Name :-"></asp:Label>
                                <asp:TextBox ID="txtUseridED" runat="server" CssClass="col" Text="User Id" Enabled="false"></asp:TextBox>

                            </div>
                            <div class="row" id="EDrowno3">
                                <asp:TextBox ID="TextBox3" runat="server" CssClass="col" placeholder="Password" TextMode="Password" Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="TextBox4" runat="server" CssClass="col" placeholder="Confirm Password" TextMode="Password" Enabled="false"></asp:TextBox>
                                <asp:Label ID="Label2" runat="server" CssClass="col" Text="DOB :-"></asp:Label>
                                <asp:TextBox runat="server" CssClass="col" ID="txtDOBED"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender
                                    ID="CalendarExtender2"
                                    runat="server"
                                    TargetControlID="txtDOBED"
                                    Format="d MMM yyyy" />
                            </div>
                            <div class="row" id="EDrowno4">
                                <asp:TextBox ID="txtemED" runat="server" CssClass="col" placeholder="Enter Email Id"></asp:TextBox>
                                <%--<asp:Label ID="lblmobno" runat="server" CssClass="col" Text="Mobile Number :-"></asp:Label>--%>
                                <asp:TextBox runat="server" CssClass="col" ID="txtumnED" placeholder="Mobile Number"></asp:TextBox>
                                <asp:Label ID="Label3" runat="server" CssClass="col" Text="Designation :-"></asp:Label>
                                <asp:DropDownList ID="DropDownList4" runat="server" CssClass="col" AutoPostBack="false" onchange="onRoleChangeEdit()" >
                                    <%--<asp:ListItem Value="">-- Select Role --</asp:ListItem>
                                    <asp:ListItem>Developer</asp:ListItem>
                                    <asp:ListItem>Support</asp:ListItem>
                                    <asp:ListItem>Tester</asp:ListItem>--%>

                                </asp:DropDownList>

                            </div>
                            <div class="row" id="EDrowno5">
                                <div class="col" style="width: 39%;"></div>
                                <asp:Button ID="btnEdit" runat="server" CssClass="BtnCls" Text="Edit"
                                    OnClientClick="return confirmEdit();" OnClick="btnEdit_Click" />

                                <asp:Button ID="btnEditCanl" runat="server" CssClass="BtnCls" Text="Cancel" OnClick="CancelED_Click" />

                            </div>
                        </div>

                        <div id="ED3B" class="CreateUser3" runat="server" style="display: none">
                            Service Sanctioning
                        <asp:Literal ID="litModuleTreeE" runat="server"></asp:Literal>
                            <div class="col" style="width: 39%;"></div>
                            <asp:Button ID="BtnSenE" runat="server" CssClass="BtnCls" Text="Save" OnClick="btnSen_Click" OnClientClick="return gatherSelectedModules();" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />

                        </div>


                    </div>
                </div>
            </div>
        </div>
        




        </asp:Content>
   <%-- </form>
</body>
</html>--%>
