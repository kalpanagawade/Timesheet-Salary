<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EmployeeTimesheet_Salary.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Form </title>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>


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
                padding: 13px 80px;
                background-color: white;
                border-radius: 0 0 99px 99px;
                height:15%;
            }

        .FromShift {
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
    transform: translate(0px, -20px);
    padding: 2px 5px;
    width:40%;
    height:30%;
    font-size: 14px;
    text-align:center;
    border-radius: 25px;
}

.form_wrapper i {
    position: absolute;
    left: 265px;
    color: black;
    font-size: 20px;
}


        .remember_box {
            display: flex;
            align-items: center;
            justify-content: space-between;
            
        }

        /*button {
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
#togglePassword {
    position: absolute;
    right: 15px;               /* 👈 Place inside right */
    top: 50%;
    transform: translateY(-50%);
    font-size: 22px;
    cursor: pointer;
    color: white;
}
.Login-btn {
    background-color: #0d6efd;
    color: white;
    padding: 10px 20px;
    border: solid;
    border-radius: 25px;
    font-size: 16px;
    cursor: pointer;
    border-color:black;
}
.popup {
    display: none;
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background: rgba(0,0,0,0.5);
    z-index: 9999;
}

.popup-content {
    width: 350px;
    margin: 10% auto;
    background: #fff;
    padding: 20px;
    border-radius: 12px;
    box-shadow: 0 0 10px #000;
    text-align: center;
}


.linkCls{
        background-color: honeydew;
    cursor: pointer;
}

.inputBox {
    width: 95%;
    padding: 8px;
    margin-bottom: 10px;
}
.close {
    float: right;
    cursor: pointer;
    font-size: 25px;
}
.hourglass {
  width: 70px;
  height: 70px;
  border: 10px solid Black;
  border-color: red transparent yellow transparent;
  border-radius: 50%;
  animation: hourglass 1.2s infinite;
  margin: auto;
}

@keyframes hourglass {
  0% {
    transform: rotate(0deg);
  }
  50% {
    transform: rotate(180deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

        .loader-box {
            display: none;
            text-align: center;
            margin-top: 10px;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 9999;
            padding-top:20%
        }
    </style>
  


</head>
<body>
 <div id="hrPopup" class="popup">
    <div class="popup-content">
        <span class="close" onclick="closeHRPopup()">&times;</span>

        <h3>HR Contact Details</h3>
        <p><strong>Name:</strong>Kalpana Gawade.</p>
        <p><strong>Email:</strong> gawadekalpana648@gmaiil.com</p>
        <p><strong>Phone:</strong> +91 9307769947</p>
    </div>
</div>

    <div id="frgPopup" class="popup">
    <div class="popup-content">
        <span class="close" onclick="closeFrgPopup()">&times;</span>

        <h3>Forgot Password</h3>

        <input type="text" id="txtUserInput" placeholder="Enter Email or Phone" class="inputBox" />
        <button type="button" onclick="sendOTP()">Send OTP</button>

        <div id="otpSection" style="display:none; margin-top:15px;">
            <input type="text" id="txtOTP" placeholder="Enter OTP" class="inputBox" />
            <button type="button" onclick="verifyOTP()">Verify OTP</button>
        </div>

        <div id="newPassSection" style="display:none; margin-top:15px;">
            <input type="password" id="txtNewPass" placeholder="New Password" class="inputBox" />
            <button type="button" onclick="updatePassword()">Update Password</button>
        </div>
    </div>
</div>

<%--    <div id="loader" style="display:block; text-align:center; margin-top:15px;padding-top: 18%;" class="popup">
<img src="/img/Time.gif" style="width:80px; height:70px;" />
<p>Please wait...</p>
</div>--%>


    <div id="loader" class="loader-box">
    <div class="hourglass"></div>
    <p>Loading...</p>
</div>

    <main>
        <header>
            
            <h4>Login</h4>
        </header>
        <form id="form2" runat="server">


        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true" />
        <div class="FromShift">
             <!-- Username -->
        <div class="form_wrapper">
            <asp:TextBox ID="txtUsername" runat="server" CssClass="input" placeholder=" " required="required" />
            <label  class="labelcls" for="txtUsername">Username</label>
            <i class='bx bxs-user bx-burst'></i>
        </div>


        <div class="form_wrapper">
            <asp:TextBox ID="txtPassword" runat="server" CssClass="input" TextMode="Password" placeholder=" " required="required" />
            <label  class="labelcls" for="txtPassword">Password</label>
               <i class='bx bx-show' id="togglePassword"></i>
        </div>


            <div class="remember_box">
                <div class="remember">
                    <asp:CheckBox ID="chkRememberMe" runat="server" Text="Remember me" />
                </div>

                 <asp:LinkButton ID="lnkfrgpass" runat="server" class="linkCls" OnClientClick="showFrgPopup(); return false;">
                Forgot Password?
                 </asp:LinkButton>
            </div>
            <asp:Button ID="btnLogin" runat="server" Class="Login-btn" Text="Login" OnClick="btnlogin_Click" OnClientClick="showLoader();" UseSubmitBehavior="false"/>
            <%--OnClick="btnlogin_Click" --%>

            <div class="new_account">
               Don't have an account?
                <asp:LinkButton ID="lnkSignUp" runat="server" class="linkCls" OnClientClick="showHRPopup(); return false;">
                Please contact HR.
                </asp:LinkButton>

            </div>
            </div>

            <script type="text/javascript">
                function showLoader() {
                    debugger
                    document.getElementById("loader").style.display = "block";

                    setTimeout(function () {
                        // auto hide after 3 seconds
                        document.getElementById("loader").style.display = "none";
                    }, 100000);
                }
                document.getElementById("togglePassword").onclick = function () {

                    var pwd = document.getElementById("<%= txtPassword.ClientID %>");

                    if (pwd.type === "password") {
                        // Show password
                        pwd.type = "text";
                        this.classList.remove("bx-show");
                        this.classList.add("bx-hide");
                    } else {
                        // Hide password
                        pwd.type = "password";
                        this.classList.remove("bx-hide");
                        this.classList.add("bx-show");
                    }
                };

                function showHRPopup() {
                    document.getElementById("hrPopup").style.display = "block";
                }

                function closeHRPopup() {
                    document.getElementById("hrPopup").style.display = "none";
                }

                function showFrgPopup() {
                    document.getElementById("frgPopup").style.display = "block";
                }

                function closeFrgPopup() {
                    document.getElementById("frgPopup").style.display = "none";
                }



                // STEP 1: SEND OTP
                function sendOTP() {
                    debugger
                    var userInput = document.getElementById("txtUserInput").value;

                    $.ajax({
                        type: "POST",
                        url: "Login.aspx/SendOTP",
                        data: JSON.stringify({ emailOrPhone: userInput }),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        headers: { "X-Requested-With": "XMLHttpRequest" },
                        success: function (response) {
                            alert("OTP sent successfully!");
                            $("#otpSection").show();
                        },
                        error: function (xhr) {
                            console.log(xhr.responseText);
                            /*alert("Error sending OTP!");*/
                            $("#otpSection").show();  //This is temprary solustion
                        }
                    });
                }

                // STEP 2: VERIFY OTP
                function verifyOTP() {
                    var otp = document.getElementById("txtOTP").value;

                    $.ajax({
                        type: "POST",
                        url: "Login.aspx/VerifyOTP",
                        data: JSON.stringify({ otp: otp }),
                        contentType: "application/json; charset=utf-8",
                        success: function (response) {
                            if (response.d === "OK") {
                                alert("OTP verified!");
                                document.getElementById("newPassSection").style.display = "block";
                            } else {
                                alert("Incorrect OTP!");
                            }
                        },
                        error: function (xhr) {
                            console.log(xhr.responseText);
                            /*alert("Error sending OTP!");*/
                            alert("OTP verified!");
                            document.getElementById("newPassSection").style.display = "block"; //This is temprary solustion
                        }
                    });
                }

                // STEP 3: UPDATE PASSWORD
                function updatePassword() {
                    var newPass = document.getElementById("txtNewPass").value;

                    $.ajax({
                        type: "POST",
                        url: "Login.aspx/UpdatePassword",
                        data: JSON.stringify({ newPassword: newPass }),
                        contentType: "application/json; charset=utf-8",
                        success: function () {
                            alert("Password updated successfully!");
                            closeFrgPopup();
                        }
                    });
                }

            </script>
            
        </form>
    </main>



</body>
</html>
