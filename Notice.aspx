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
    <h2 style="margin: 20px;">Tomorrow Is Holiday</h2>
</asp:Content>

<%--    </form>
</body>
</html>--%>
