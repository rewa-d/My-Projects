<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="TaskTrackr_Project.Login" %>
<%@ Register TagPrefix="uc" TagName="Header" Src="~/header1.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Login Page</title>
    <link href="styles.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <uc:Header runat="server" />
    <h1 class="text-center mt-5">Welcome To TaskTrackr!</h1>
    <div class="login-body">
        <form id="loginForm" runat="server">
            <div>
                <asp:TextBox ID="txt_name" runat="server" style="margin-left:auto" Placeholder="Username"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_name" ErrorMessage="Please enter username!!" ForeColor="Red"></asp:RequiredFieldValidator>
               
            </div>
            <div>
                <asp:TextBox ID="txt_pass" runat="server" style="margin:auto" TextMode="Password" Placeholder="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_pass" ErrorMessage="Please enter password!!" ForeColor="Red"></asp:RequiredFieldValidator>
               
                <asp:Button ID="btn_login" runat="server" Text="Login" CssClass="btn btn-primary" OnClick="btn_login_Click" />

            </div>
        </form>
    </div>
    <h6 class="text-center my-3">Forgot Password?</h6>
    <h6 class="text-center my-3">New to TaskTrackr? <a href="SignUp.aspx" class="custom-link" >Sign Up</a> </h6>
    <asp:Label ID="lbl_res" runat="server"></asp:Label>
</body>
</html>

