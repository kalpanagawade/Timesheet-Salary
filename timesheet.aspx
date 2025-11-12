<%@ Page Title="Employee Timesheet Calendar" Language="C#" MasterPageFile="~/Site1.master" AutoEventWireup="true"
    CodeBehind="timesheet.aspx.cs" Inherits="EmployeeTimesheet_Salary.timesheet" %>

<%--<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Employee Timesheet Calendar</title>--%>

    <asp:Content ID="ContentHead" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" />
    <link href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/flatpickr/dist/flatpickr.min.css"/>
    <script src="https://cdn.jsdelivr.net/npm/flatpickr"></script>
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />


    <style type="text/css">
        header {
            /*border-radius: 1rem;
            position fixed;*/
            width: 98.5% !important;
            /*height: 89px;
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
            transition: 0.5s;*/
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


        .header-section {
            background-color: antiquewhite;
            padding: 0px 15px;
            border: 1px solid #ccc;
            margin: 0px 5px 5px 5px;
            border-radius: 35px;
            line-height: 1.0;
        }

        .header-row {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            align-items: center;
        }

        .header-group {
            margin: 10px 10px 1px 10px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        .header-group2{
             margin: 1px 10px 10px 10px;
             display: flex;
             align-items: center;
             gap: 10px;
        }

            .header-group label,.header-group2 label {
                font-weight: bold;
            }

        .logout-btn {
            padding: 5px 15px;
            border: 1px solid #007bff;
            background-color: white;
            color: #007bff;
            border-radius: 4px;
            cursor: pointer;
        }

        .task-label {
            font-weight: bold;
            font-size: 16px;
            color: #007bff;
            margin-top: 2px;
        }

        .calendar {
            width: 100%;
            border-collapse: collapse;
            table-layout: fixed;
        }

            .calendar th, .calendar td {
                width: 14.28%; /* 100% / 7 days */
                height: 10vh;
                border: 1px solid #ccc;
                vertical-align: top;
                padding: 5px;
                box-sizing: border-box;
                text-align: left;
                font-size: 14px;
            }

            .calendar th {
                background-color: #f2f2f2;
                font-weight: bold;
                text-align: center;
            }

        .pending {
            background-color: #ffff99;
        }

        .weeklyoff, .compoff {
            background-color: #c2f0c2;
        }

        .bucket {
            background-color: #d9d9f3;
            font-style: italic;
        }

        .holiday, .leave {
            background-color: lightpink;
            color: white;
        }

        .approved {
            background-color: lightseagreen;
            color: white;
        }
        /* Approved = green */

        .NotApproved{
    background-color: lightcoral;    
}

        .nav-btn {
            margin: 5px;
            padding: 5px 10px;
        }

        .month-label {
            font-size: 20px;
            font-weight: bold;
            margin: 10px;
            display: inline-block;
        }

        .fancy-button {
            background-color: #28a745; /* Green */
            color: white;
            border: none;
            padding: 10px 20px;
            font-size: 16px;
            border-radius: 8px;
            transition: background-color 0.3s ease, transform 0.3s ease;
            cursor: pointer;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }

            .fancy-button:hover {
                background-color: #218838; /* Darker green */
                transform: scale(1.05); /* Slight zoom */
            }

            .fancy-button:disabled {
                background-color: #ccc;
                cursor: not-allowed;
                transform: none;
            }

        /*//Added ruby Hrutik 18072025*/
        .modalPopup {
            display: none;
            position: fixed; /* ensures it floats above the calendar */
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 9999; /* high enough to stay above all other elements */
            background-color: rgba(0, 0, 0, 0.5); /* dim background */
        }

        .modal-content {
            background-color: #fff;
            margin: 10% auto;
            padding: 20px;
            /*width: 50%;*/
            border-radius: 8px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.3);
        }
        #btnLogout{
            font-size: smaller;
            line-height: revert;
        }
        /*.modal {
            top: -6%
        }*/
      
    </style>

    <script type="text/javascript">


        //function updateClock() {
        //    const now = new Date();
        //    const timeStr = now.toTimeString().split(' ')[0];

        //    const inTimeDiv = document.getElementById('lblInTime');
        //    const outTimeDiv = document.getElementById('lblOutTime');

        //    if (inTimeDiv && inTimeDiv.innerText.trim() === "") {
        //        inTimeDiv.innerText = timeStr;
        //    }

        //    if (outTimeDiv && outTimeDiv.innerText.trim() === "") {
        //        outTimeDiv.innerText = timeStr;
        //    }
        //}

        //window.onload = function () {
        //    setInterval(updateClock, 1000);
        //};


        function validateTime(input) {
            var regex = /^([0-1]?[0-9]|2[0-3]):([0-5][0-9])$/;
            if (!regex.test(input.value)) {
                input.setCustomValidity("Enter valid time in HH:mm format (00–23:00–59)");
            } else {
                input.setCustomValidity("");
            }
        }

        function openTaskModal(dateStr, inTime, outTime) {
            console.clear();
            console.log("openTaskModal called:", { dateStr, inTime, outTime });

            // Format date (keep your existing code if you need it)
            const parts = dateStr.split('-'); // e.g. ['2025','09','21']
            const formatted = `${parts[2]}-${parts[1]}-${parts[0]}`;
            const modalDateEl = document.getElementById('<%= txtModalDate.ClientID %>');
            if (modalDateEl) modalDateEl.value = formatted;
            const modalText = document.getElementById('modalDateText');
            if (modalText) modalText.innerText = formatted;

            // --- get elements (ClientIDMode="Static" required) ---
            const inTimeBox = document.getElementById("inTimeText");
            const outTimeBox = document.getElementById("outTimeText");

            if (!inTimeBox || !outTimeBox) {
                console.error("Textboxes not found. Check ClientIDMode and element IDs (or typos).",
                    { inTimeBox, outTimeBox });
                return;
            }

            // helper: is real time value?
            function hasRealTime(val) {
                if (val === null || val === undefined) return false;
                const s = String(val).trim();
                if (s === "" || s.toLowerCase() === "null" || s === "00:00:00") return false;
                return true;
            }

            // Apply style class name you must define in CSS
            // .text-disabled { background-color:#f0f0f0 !important; color:#888 !important; cursor:not-allowed !important; }

            // In Time
            if (hasRealTime(inTime)) {
                inTimeBox.value = inTime;
                inTimeBox.setAttribute("disabled", "disabled");   // ensures attribute in DOM
                console.log("inTime locked:", inTime);
            } else {
                inTimeBox.value = "00:00:00";
                inTimeBox.removeAttribute("disabled");
                console.log("inTime editable");
            }

            // Out Time
            if (hasRealTime(outTime)) {
                outTimeBox.value = outTime;
                outTimeBox.setAttribute("disabled", "disabled");
                console.log("outTime locked:", outTime);
            } else {
                outTimeBox.value = "00:00:00";
                outTimeBox.removeAttribute("disabled");
                console.log("outTime editable");
            }

            // show modal AFTER setting fields
            $('#taskModal').modal('show');

            // IMPORTANT: DO NOT CALL __doPostBack HERE if you expect client changes to persist.
            // If you must call __doPostBack, move it to server-side and re-open the modal after postback.
            __doPostBack('<%= btnTriggerBindGrid.UniqueID %>', '');
        }



        document.addEventListener("DOMContentLoaded", function () {
            // In Time
            flatpickr("#inTimeText", {
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i:S", // 24-hour format (e.g., 13:45)
                time_24hr: true,
                enableSeconds: true
            });

            // Out Time
            flatpickr("#outTimeText", {
                enableTime: true,
                noCalendar: true,
                dateFormat: "H:i:S",
                time_24hr: true,
                enableSeconds: true
            });
        });


    </script>


    <script>

       <%-- function onTypeSelectionChanged(selectedValue) {
            const sendBtn = document.getElementById("<%= Btn_TAprov.ClientID %>");

            if (["Holiday", "Leave", "Weekly off", "Comp off"].includes(selectedValue)) {
                sendBtn.disabled = false;
            } else {
                sendBtn.disabled = true;
            }
        }--%>



        function validateBeforeSave() {
            var dayType = document.getElementById('<%= hfDayType.ClientID %>').value;
            var isSpecialDay = ["Holiday", "Leave", "Weekly", "Comp"].includes(dayType);

            if (isSpecialDay) {
                alert("✅ Saved successfully.");
                return true; // Skip validation
            }

            // Validate required fields
            var type = document.getElementById('<%= ddlModalType.ClientID %>').value;
            var workCompletion = document.getElementById('<%= ddlWorkCompletion.ClientID %>').value;
            var description = document.getElementById('<%= txtDescription.ClientID %>').value.trim();

            if (!type) {
                alert("Please select a Type.");
                return false;
            }
            if (!workCompletion) {
                alert("Please select Work Completion.");
                return false;
            }
            if (!description) {
                alert("Please enter Description.");
                return false;
            }
            var timeSpent = document.getElementById("<%= txtTimeSpent.ClientID %>").value;

            if (timeSpent === "" || timeSpent === "00:00") {
                alert("Please select Time Spent.");
                return false;
            }

            var intime = document.getElementById("<%= inTimeText.ClientID %>").value;

            if (intime === "" || intime === "00:00:00") {
                alert("Please select intime.");
                return false;
            }
            var outtime = document.getElementById("<%= outTimeText.ClientID %>").value;

            if (outtime === "" || outtime === "00:00:00") {
                alert("Please select  outtime.");
                return false;
            }
            <%--document.getElementById('<%= hfDayType.ClientID %>').value = "";--%>
            return true;
        }

        function onDayTypeSelected(selected) {
            document.getElementById('<%= hfDayType.ClientID %>').value = selected;

            var formDiv = document.getElementById('<%= allrows.ClientID %>');
            var inputs = formDiv.querySelectorAll("input, select, textarea");

            // Disable or enable form rows and inputs
            if (["Holiday", "Leave", "Weekly", "Comp"].includes(selected)) {
                formDiv.style.display = "none";
                inputs.forEach(el => el.disabled = true);
            } else {
                formDiv.style.display = "block";
                inputs.forEach(el => el.disabled = false);
            }

            // Toggle buttons visibility
            var buttonMap = {
                'Holiday': '<%= Btn_IsHoli.ClientID %>',
                'Leave': '<%= Btn_IsLev.ClientID %>',
                'Weekly': '<%= Btn_IsWek.ClientID %>',
                'Comp': '<%= Btn_IsComp.ClientID %>'
            };

            for (var key in buttonMap) {
                var btn = document.getElementById(buttonMap[key]);
                btn.style.display = (key === selected) ? "inline-block" : "none";
            }

        }




       <%-- function handleSelection(selectedType) {
            // Hide task entry fields
            document.getElementById('taskEntryFields').style.display = 'none';

            // Button IDs
            const btns = {
                'Holiday': '<%= Btn_IsHoli.ClientID %>',
            'Leave': '<%= Btn_IsLev.ClientID %>',
            'Weekly': '<%= Btn_IsWek.ClientID %>',
            'Comp': '<%= Btn_IsComp.ClientID %>'
            };

            // Loop through and hide all except selected
            for (let key in btns) {
                const btn = document.getElementById(btns[key]);
                if (key === selectedType) {
                    btn.disabled = false;
                    btn.style.display = 'inline-block'; // show selected
                } else {
                    btn.disabled = true;
                    btn.style.display = 'none'; // hide others
                }
            }
        }--%>

        // Optional reset function
        function resetButtonsAndForm() {
            document.getElementById('taskEntryFields').style.display = 'block';

            document.getElementById('<%= Btn_IsHoli.ClientID %>').disabled = false;
            document.getElementById('<%= Btn_IsLev.ClientID %>').disabled = false;
            document.getElementById('<%= Btn_IsWek.ClientID %>').disabled = false;
            document.getElementById('<%= Btn_IsComp.ClientID %>').disabled = false;

            document.getElementById('<%= Btn_IsHoli.ClientID %>').style.display = 'inline-block';
            document.getElementById('<%= Btn_IsLev.ClientID %>').style.display = 'inline-block';
            document.getElementById('<%= Btn_IsWek.ClientID %>').style.display = 'inline-block';
            document.getElementById('<%= Btn_IsComp.ClientID %>').style.display = 'inline-block';
        }



        function confirmDelete() {
            return confirm("Are you sure you want to delete this task?");
        }
        function confirmEdit(taskId) {
            return confirm("Do you want to edit task ID " + taskId + "?");
        }
        ///Added by hrutik 18072025
        function openModal() {
            document.getElementById("modalPopup").style.display = "block";
        }
        function closeModal() {
            document.getElementById("modalPopup").style.display = "none";
        }
        //function openModal() {
        //    $('#yourModalId').modal('show'); // Replace with your modal's actual ID
        //}
        $('#myModal').modal('show');



        function toggleLogoutDropdown() {
            var dropdown = document.getElementById("logoutDropdown");
            dropdown.style.display = (dropdown.style.display === "block") ? "none" : "block";
        }
        function logoutUser() {
            // Optional: Clear session or cookies
            window.location.href = 'Login.aspx'; // Adjust path if needed
        }


        let isLoggedIn = false;
        let inTimeTimer;
        let outTimeTimer;

        //Comment by Hrutik
        //function updateInTime() {
        //    const now = new Date();
        //    const timeStr = now.toTimeString().split(' ')[0]; // HH:mm:ss
        //    const inTimeBox = document.getElementById('txtInTime');
        //    if (inTimeBox) inTimeBox.value = timeStr;
        //}

        //Comment by Hrutik
        //function updateOutTime() {
        //    const now = new Date();
        //    const timeStr = now.toTimeString().split(' ')[0];
        //    const outTimeBox = document.getElementById('txtOutTime');
        //    if (outTimeBox) outTimeBox.value = timeStr;
        //}

        function updateInTime() {
            const inTimeBox = document.getElementById('txtInTime');
            if (inTimeBox && !inTimeBox.value) {  // only if DB did not fill it
                const now = new Date();
                inTimeBox.value = now.toTimeString().split(' ')[0];
            }
        }

        function updateOutTime() {
            const outTimeBox = document.getElementById('txtOutTime');
            if (outTimeBox && !outTimeBox.value) {  // only if DB did not fill it
                const now = new Date();
                outTimeBox.value = now.toTimeString().split(' ')[0];
            }
        }

        function startClock() {
            setInterval(function () {
                updateInTime();
                updateOutTime();
            }, 1000); // refresh every second
        }

        /*window.onload = startClock;*/

        //function startClocks() {
        //    updateInTime();
        //    updateOutTime();
        //    inTimeTimer = setInterval(updateInTime, 1000);
        //    outTimeTimer = setInterval(updateOutTime, 1000);
        //}

        //function stopInTimeClock() {
        //    clearInterval(inTimeTimer);
        //}

        //function stopOutTimeClock() {
        //    clearInterval(outTimeTimer);
        //}

        //function handleLoginLogout() {
        //    const btn = document.getElementById('btnLoginLogout');
        //    const now = new Date().toTimeString().split(' ')[0];

        //    if (!isLoggedIn) {
        //        // Login
        //        stopInTimeClock();
        //        document.getElementById('txtInTime').value = now;
        //        btn.value = 'Logout';  // 👈 This line sets the text
        //        console.log("Logged in: Button text set to Logout");
        //        isLoggedIn = true;
        //    } else {
        //        // Logout
        //        stopOutTimeClock();
        //        document.getElementById('txtOutTime').value = now;
        //        btn.disabled = true;
        //        console.log("Logged out: Button disabled");
        //    }

        //    return false; // 👈 This prevents postback!
        //}


        function displayCurrentDayDate() {
            const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
            const now = new Date();
            const day = days[now.getDay()];
            const date = now.getDate();
            const formatted = `${day}, ${date}`;
            document.getElementById('currentDateDisplay').textContent = formatted;
        }

        window.onload = function () {
            /*startClocks();*/
            startClock()
            displayCurrentDayDate();
        }

    </script>

</asp:Content>

<%--</head>
<body>
    <form id="form1" runat="server">--%>

     <asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server" />
       <%-- <header>
            <img src="../img/logo.png" alt="Logo" style="width: 50px; height: 50px; margin: 0px 0px 0px 8px;" />
            <div class="navbar">
                <asp:Button runat="server" ID="btnEmpTimSht" class="home-active" Text="Employee Timesheet" Style="padding: 18px;" OnClick="btnEmpTimSht_Click" />
            </div>
            
            <div class="navbar">
               
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



        </header>--%>

        <div style="height: 2rem;"></div>
        <!-- for spacing -->
        <div class="header-section">
            <div class="header-row">
                <div class="header-group">
                    <label>Work Place *</label>
                    <asp:DropDownList ID="ddlWorkPlace" runat="server">
                        <asp:ListItem Text="Work From Home" Value="WFH" />
                        <asp:ListItem Text="Office" Value="Office" />
                    </asp:DropDownList>
                </div>

                <div class="header-group">
                    <label>In Time *</label>
                    <asp:TextBox ID="txtInTime" runat="server" ReadOnly="true" ClientIDMode="Static" />
                </div>

                <div class="header-group">
                    <label>Out Time *</label>
                    <asp:TextBox ID="txtOutTime" runat="server" ReadOnly="true" ClientIDMode="Static" />
                </div>

                <div class="header-group">
                    <%--<asp:Button ID="btnLoginLogout" runat="server" Text="Login" CssClass="logout-btn"
                        OnClientClick="return handleLoginLogout();" UseSubmitBehavior="false" ClientIDMode="Static" />--%>
                    <asp:Button ID="btnLoginLogout" runat="server" Text="Login" CssClass="logout-btn"
    OnClick="btnLoginLogout_Click"  UseSubmitBehavior="false" ClientIDMode="Static" />
                </div>

            </div>

            <div class="task-label">Daily Task Sheet</div>

            <div class="header-row">
                <div class="header-group2">
                    <label>From Date *</label>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="date-input" placeholder="dd-mm-yyyy" />
                </div>

                <div class="header-group2">
                    <label>To Date *</label>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="date-input" placeholder="dd-mm-yyyy" />
                </div>

                <div class="header-group2">
                    <asp:Button ID="btnFetch" runat="server" Text="Fetch Timesheet" Enabled="false" />
                </div>

                <div class="header-group2">
                    <span id="currentDateDisplay"></span>
                    <label>Status</label>
                    <%--<asp:Image ID="imgStatusHelp" runat="server" ImageUrl="~/images/info.png" ToolTip="Status info" />--%>
                </div>
            </div>
        </div>

        <div style="text-align: center; margin-bottom: 10px;">
            <!-- Navigation Buttons -->
            <asp:Button ID="btnPrev" runat="server" Text="&lt;" OnClick="btnPrev_Click" CssClass="nav-btn" />
            <asp:Button ID="btnNext" runat="server" Text="&gt;" OnClick="btnNext_Click" CssClass="nav-btn" />

            <!-- Hidden Field to keep track of current month -->
            <asp:HiddenField ID="hfMonthYear" runat="server" />

            <!-- Month-Year Display -->
            <div class="month-label">
                <asp:Label ID="lblMonthYear" runat="server" />
            </div>
        </div>

        <div style="margin: 0px 7px;">
            <!-- Calendar Table -->
            <table class="calendar">
                <tr>
                    <th style="background-color: lightpink;">Sun</th>
                    <th>Mon</th>
                    <th>Tue</th>
                    <th>Wed</th>
                    <th>Thu</th>
                    <th>Fri</th>
                    <th style="background-color: lightpink;">Sat</th>
                </tr>
                <asp:Literal ID="litCalendar" runat="server" />
            </table>
        </div>


        <!-- Modal -->
        <div class="modal fade" id="taskModal" tabindex="-1" role="dialog" aria-labelledby="taskModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-xl" role="document">
                <div class="modal-content">
                    <div class="modal-header bg-primary text-white">
                        <h5 class="modal-title" id="taskModalLabel">Tasksheet - <span id="modalDateText">13 / 02 / 2025</span></h5>

                        <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close" onclick="location.reload();">
                            <span aria-hidden="true">&times;</span>
                        </button>


                        <%-- <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>--%>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanelModal" runat="server">
                            <ContentTemplate>
                                <asp:HiddenField ID="hfSelectedTaskID" runat="server" />
                                <div class="form-row mb-3">

                                    <%--<asp:Button ID="Btn_IsHoli" runat="server" CssClass="fancy-button" Text="Holiday"
                                        OnClientClick="onDayTypeSelected('Holiday'); return false;"" />

                                    <asp:Button ID="Btn_IsLev" runat="server" CssClass="fancy-button" Text="Leave"
                                        OnClientClick="onDayTypeSelected('Leave'); return false;" />

                                    <asp:Button ID="Btn_IsWek" runat="server" CssClass="fancy-button" Text="Weekly Off"
                                        OnClientClick="onDayTypeSelected('Weekly'); return false;" />

                                    <asp:Button ID="Btn_IsComp" runat="server" CssClass="fancy-button" Text="Comp Off"
                                        OnClientClick="onDayTypeSelected('Comp'); return false;"/>--%>
                                    <asp:Button ID="Btn_IsHoli" runat="server" ClientIDMode="Static" CssClass="fancy-button" Text="Holiday" OnClientClick="onDayTypeSelected('Holiday'); return false;" Style="margin: 0px  11px;" />
                                    <asp:Button ID="Btn_IsLev" runat="server" ClientIDMode="Static" CssClass="fancy-button" Text="Leave" OnClientClick="onDayTypeSelected('Leave'); return false;" Style="margin: 0px  11px;" />
                                    <asp:Button ID="Btn_IsWek" runat="server" CssClass="fancy-button" Text="Weekly Off" OnClientClick="onDayTypeSelected('Weekly'); return false;" Style="margin: 0px  11px;" />
                                    <asp:Button ID="Btn_IsComp" runat="server" ClientIDMode="Static" CssClass="fancy-button" Text="Comp Off" OnClientClick="onDayTypeSelected('Comp'); return false;" Style="margin: 0px  11px;" />

                                </div>


                                <div runat="server" id="allrows">

                                    <asp:HiddenField ID="hfDayType" runat="server" />
                                    <asp:Button ID="btnTriggerBindGrid" runat="server" Text="TriggerBindGrid"
                                        Style="display: none;" OnClick="btnTriggerBindGrid_Click" />

                                    <!-- First Row: In Time / Out Time -->
<%--                                    <div class="form-row mb-3">
                                        <div class="col-md-2 font-weight-bold">In Time *</div>
                                        <div class="col-md-2">11:01:44</div>
                                        <div class="col-md-2 font-weight-bold">Out Time *</div>
                                        <div class="col-md-2">20:05:00</div>
                                    </div>--%>

                                    <div class="form-row mb-3">
    <div class="col-md-2 font-weight-bold">In Time *</div>
    <div class="col-md-4">
        <asp:TextBox ID="inTimeText" runat="server" CssClass="form-control" ClientIDMode="Static"  TextMode="SingleLine"/>
    </div>
    <div class="col-md-2 font-weight-bold">Out Time *</div>
    <div class="col-md-4">
        <asp:TextBox ID="outTimeText" runat="server" CssClass="form-control" ClientIDMode="Static"  TextMode="SingleLine"/>
    </div>
</div>




                                    <!-- Second Row -->
                                    <div class="form-row mb-3">
                                        <div class="col-md-2 font-weight-bold">Date *</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtModalDate" runat="server" CssClass="form-control" Text="21-03-2004"   Enabled="false" />
                                        </div>

                                        <div class="col-md-2 font-weight-bold">Type *</div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlModalType" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="- Select Type -" Value="" />
                                                <asp:ListItem Text="Development" Value="Development" />
                                                <asp:ListItem Text="Testing" Value="Testing" />
                                                <asp:ListItem Text="Support" Value="Support" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <!-- Third Row -->
                                    <%--      <div class="form-row mb-3">
                            <div class="col-md-2 font-weight-bold">Ticket No/Bug Id</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtTicketNo" runat="server" CssClass="form-control" />
                            </div>

                            <div class="col-md-2 font-weight-bold">PMO</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlPMO" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select..." Value="" />

                                </asp:DropDownList>
                            </div>
                        </div>--%>



                                    <%-- <!-- Fifth Row -->
                        <div class="form-row mb-3">
                            <div class="col-md-2 font-weight-bold">Activity Source *</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlActivitySource" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select Source" Value="" />

                                </asp:DropDownList>
                            </div>

                            <div class="col-md-2 font-weight-bold">Application</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtApplication" runat="server" CssClass="form-control" ReadOnly="true" />
                            </div>
                        </div>--%>

                                    <!-- Sixth Row -->
                                    <div class="form-row mb-3">
                                        <div class="col-md-2 font-weight-bold">Work Completion *</div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ddlWorkCompletion" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Select value" Value="" />
                                                <asp:ListItem Value="10%">10%</asp:ListItem>
                                                <asp:ListItem Value="20%">20%</asp:ListItem>
                                                <asp:ListItem Value="30%">30%</asp:ListItem>
                                                <asp:ListItem Value="40%">40%</asp:ListItem>
                                                <asp:ListItem Value="50%">50%</asp:ListItem>
                                                <asp:ListItem Value="60%">60%</asp:ListItem>
                                                <asp:ListItem Value="70%">70%</asp:ListItem>
                                                <asp:ListItem Value="80%">80%</asp:ListItem>
                                                <asp:ListItem Value="90%">90%</asp:ListItem>
                                                <asp:ListItem Value="100%">100%</asp:ListItem>

                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 font-weight-bold">Approx. Time Spent (Hrs) *</div>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtTimeSpent" runat="server" CssClass="form-control" oninput="validateTime(this)"/>
                                        </div>

                                        <label for="txtDescription" class="form-label">Description</label>

                                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="form-control" Rows="4" Style="width: 580%" />





                                    </div>

                                    <!-- Seventh Row -->
                                    <%--<div class="form-row mb-3">
                            <div class="col-md-2 font-weight-bold">Category/Group</div>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ddlCategoryGroup" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Select Value" Value="" />

                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 font-weight-bold">Description *</div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" />
                            </div>
                        </div>--%>
                                </div>

                                <div class="form-row mb-3">
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4"></div>
                                    <div class="col-md-4">
                                        <%-- <asp:Button ID="Btn_Tsave" runat="server" CssClass="fancy-button" Text="Save"
                                            OnClick="Btn_Tsave_Click" OnClientClick="return validateBeforeSave();" />--%>
                                        <asp:Button ID="Btn_Tsave" runat="server" CssClass="fancy-button" Text="Save"
                                            OnClick="Btn_Tsave_Click" OnClientClick="return validateBeforeSave();" />



                                        <asp:Button ID="Btn_TBack" runat="server" CssClass="fancy-button" Text="Back" Enabled="false" />

                                        <asp:Button ID="Btn_TAprov" runat="server" CssClass="fancy-button" Text="Send for Approval" Enabled="false"
                                            OnClick="Btn_SndFrApprov_Click" />
                                    </div>
                                </div>


                                <asp:Label ID="lblItemStatus" runat="server" CssClass="text-info" Style="padding-right: 260px" />
                                <%--OnRowEditing="gvTasks_RowEditing"--%>
                                <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False"
                                    AllowPaging="true" PageSize="5"
                                    OnPageIndexChanging="gvTasks_PageIndexChanging"
                                    CssClass="table table-bordered"
                                    DataKeyNames="TaskID"
                                    OnRowUpdating="gvTasks_RowUpdating"
                                    OnRowCancelingEdit="gvTasks_RowCancelingEdit"
                                    OnRowDeleting="gvTasks_RowDeleting"
                                    OnRowCommand="gvTasks_RowCommand"
                                    OnDataBound="gvTasks_DataBound"
                                    OnRowDataBound="gvTasks_RowDataBound">

                                    <EmptyDataTemplate>
                                        <div style="padding: 10px; text-align: center; color: red;">
                                            No productivity item to show.
                                        </div>
                                    </EmptyDataTemplate>

                                    <Columns>


                                        <%--gvTasks_RowEditing call method if I unccommet this (Hrutik)--%>
                                        <%--<asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server"
                                            CommandName="Edit"
                                            CommandArgument='<%# Eval("TaskID") %>'
                                            OnClientClick='<%# "return confirmEdit(" + Eval("TaskID") + ");" %>'
                                            CssClass="btn btn-sm btn-warning" ToolTip="Edit Task">
                                            <i class="fas fa-pen"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                        <%--gvTasks_RowCommand call method if I use this (Hrutik)--%>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server"
                                                    CommandName="EditRow"
                                                    CommandArgument='<%# Eval("TaskID") %>'
                                                    OnClientClick='<%# "return confirmEdit(" + Eval("TaskID") + ");" %>'
                                                    CssClass="btn btn-sm btn-warning" ToolTip="Edit Task">
            <i class="fas fa-pen"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>




                                        <asp:TemplateField HeaderText="Date">
                                            <ItemTemplate>
                                                <%# Eval("TaskDate", "{0:yyyy-MM-dd}") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtDate" runat="server"
                                                    Text='<%# Bind("TaskDate", "{0:yyyy-MM-dd}") %>'
                                                    TextMode="Date" CssClass="form-control" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                                <%# Eval("Description") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox1" runat="server"
                                                    Text='<%# Bind("Description") %>'
                                                    CssClass="form-control" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Approx Time Spent">
                                            <ItemTemplate>
                                                <%# Eval("TimeSpent") %>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="TextBox2" runat="server"
                                                    Text='<%# Bind("TimeSpent") %>'
                                                    TextMode="Time" CssClass="form-control" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Delete">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" OnClientClick="return confirmDelete();"
                                                    CssClass="btn btn-sm btn-danger" ToolTip="Delete">
                    <i class="fas fa-trash-alt"></i> 
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Columns>
                                </asp:GridView>

                            </ContentTemplate>
                        </asp:UpdatePanel>


                    </div>
                </div>
            </div>
        </div>

     </asp:Content>

    <%--</form>
</body>
</html>--%>
