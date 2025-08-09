<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EmployeeTimesheet_Salary.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form </title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" />
    <style>
        body {
            display: flex;
            align-items: center;
            justify-content: center;
            min-height: 100vh;
            /* background-color: green;*/
            
              background-image: url('../img/loginImg.PNG');
              background-size: cover;           /* Optional: makes image cover the whole screen */
              /*background-position: center;  */    /* Optional: centers the image */
              /*background-repeat: no-repeat;*/ 
         }

        main {
            width: 300px;
            padding: 20px;
            border-radius: 10px;
            border: 2px solid white;
            background-color: transparent;
            backdrop-filter: blur(25px);
            position: relative;
        }

            main header {
                position: absolute;
                top: 0;
                left: 50%;
                transform: translateX(-50%);
                padding: 10px 20px;
                background-color: white;
                border-radius: 0 0 10px 10px;
            }

        form {
            margin-top: 80px;
            display: flex;
            flex-direction: column;
            gap: 20px;
        }
        /*.from_wrapper { 
     position: relative;
     display: flex;
     align-items: center;
 }

 input[type="text"], input[type="password"] {
     padding: 10px;
     border: 2px solid white;
     width: 100%;
     border-radius: 25px;
     outline: none;
     background-color: transparent;
     color: white;
 }

 .labelcls {
     position: absolute;
     left: 10px;
     color: white;
     transition: .2s;
 }*/
.form_wrapper {
    position: relative;
    display: flex;
    align-items: center;
    width: 100%;
    margin-bottom: 20px;
}

.input {
    padding: 12px 12px 12px 40px;
    border: 2px solid white;
    width: 100%;
    border-radius: 25px;
    outline: none;
    background-color: transparent;
    color: white;
    font-size: 16px;
}

.input:focus {
    border-color: #00bfff;
}

.labelcls {
    position: absolute;
    left: 40px;
    top: 12px;
    color: white;
    font-size: 16px;
    pointer-events: none;
    transition: 0.2s ease all;
}

.input:focus + .labelcls,
.input:not(:placeholder-shown) + .labelcls {
    background-color: white;
    color: black;
    transform: translate(10px, -20px);
    padding: 2px 5px;
    font-size: 8px;
    border-radius: 4px;
}

.form_wrapper i {
    position: absolute;
    left: 12px;
    color: white;
    font-size: 20px;
}


       /* .remember_box {
            display: flex;
            align-items: center;
            justify-content: space-between;
            font-size: 12px;
        }

        button {
            border: none;
            padding: 10px;
            border-radius: 25px;
            cursor: pointer;*/
        /*}*/
        .remember {
    display: flex;
    align-items: center;
    gap: 5px; /* space between checkbox and text */
    color: white; /* optional */
}

input[type="checkbox"] {
    width: 16px;
    height: 16px;
    accent-color: #00bfff; /* Optional: modern browsers */
}

        .new_account {
            text-align: center;
            color: white;
        }
     
    </style>
</head>
<body>
    <main>
        <header>
            
            <h4>Login</h4>
        </header>
        <form id="form2" runat="server">

            <%-- <div class="from_wrapper"> 
     <asp:TextBox ID="TextBox1" runat="server" CssClass="input" required="required" />
     <label for="txtUsername">Username</label>
     <i class='bx bxs-user bx-burst'></i>

 </div> --%>
             <!-- Username -->
        <div class="form_wrapper">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="input" placeholder=" " required="required" />
            <label  class="labelcls" for="txtUsername">Username</label>
            <i class='bx bxs-user bx-burst'></i>
        </div>

        <!-- Password -->
          <%--  <div class="from_wrapper">
    <asp:TextBox ID="TextBox1" runat="server" CssClass="input" required="required" />
    <label for="password">Password</label>
    <i class='bx bxs-lock bx-tada'></i>
</div>--%>
        <div class="form_wrapper">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input" TextMode="Password" placeholder=" " required="required" />
            <label  class="labelcls" for="txtPassword">Password</label>
            <i class='bx bxs-lock bx-tada'></i>
        </div>


            <div class="remember_box">
                <div class="remember">
                    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" />
                </div>

                <asp:HyperLink ID="lnkForgotPassword" runat="server" NavigateUrl="ForgotPassword.aspx">
                  Forgot Password?
                </asp:HyperLink>
            </div>
            <asp:Button ID="btnLogin" runat="server" Class="Login-btn" Text="Login" OnClick="btnlogin_Click" />
            <%--OnClick="btnlogin_Click" --%>

            <div class="new_account">
                Don't have account ?
                <asp:LinkButton ID="lnkSignUp" runat="server" OnClientClick="Sign(); return false;">
                    Sign up
                </asp:LinkButton>

            </div>
        </form>
    </main>



</body>
</html>
