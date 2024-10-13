<%@Page Language="C#" AutoEventWireup="true" CodeFile="~/SignUp.aspx.cs" Inherits="TaskTrackr_Project.SignUp" %>

<%@ Register TagPrefix="uc" TagName="Header" Src="~/header1.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Sign Up Page</title>
    <link href="styles.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <uc:Header runat="server" />
    <h1 class="text-center mt-5">Welcome To TaskTrackr!</h1>
    <div class="signUp-body">
        <form id="signUpForm" runat="server">

            <div class="my-3">
                <asp:TextBox ID="txt_fname" runat="server" style="margin-left:auto" Placeholder="First Name"></asp:TextBox>
            </div>
            <div class="my-3">
                <asp:TextBox ID="txt_lname" runat="server" style="margin-left:auto" Placeholder="Last Name"></asp:TextBox>
            </div>
                       
            <div class="my-3">
                <asp:TextBox ID="txt_email" runat="server" style="margin-left:auto" Placeholder="Email Address" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_email" ErrorMessage="Please enter email address!!" ForeColor="Red"></asp:RequiredFieldValidator>
            </div>
           
            <div class="my-3">
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter valid e-mail address!! " ControlToValidate="txt_email" EnableClientScript="False" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                <asp:TextBox ID="txt_user" runat="server" style="margin-left:auto" Placeholder="Username"></asp:TextBox>
            </div>

            <div class="my-3">
                <asp:TextBox ID="txt_pass" runat="server" style="margin:auto" TextMode="Password" Placeholder="Password"></asp:TextBox>  
           </div>
            <div class="my-3">
               <asp:TextBox ID="txt_repass" runat="server" style="margin-left:auto" Placeholder="Re-enter password" TextMode="Password"></asp:TextBox>
               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_pass" ErrorMessage="Please enter password!!" ForeColor="Red"></asp:RequiredFieldValidator>
               <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Passwords do not match!!" ControlToCompare="txt_pass" ControlToValidate="txt_repass" EnableClientScript="False" ForeColor="Red"></asp:CompareValidator>
            </div>
            
            <div>
                <asp:Button ID="btn_signup" runat="server" Text="Sign Up" CssClass="btn btn-primary" OnClick="btn_signup_Click" />
            </div>
        </form>
    </div>
    <p>
       
        <asp:Label ID="lbl_res" runat="server"></asp:Label>
    </p>
</body>
</html>

