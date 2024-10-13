<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="TaskTrackr_Project.Home" %>
<%@ Register TagPrefix="uc" TagName="Header" Src="~/header1.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>TaskTrackr</title>
    <meta name="viewport" content="width=device-width,initial-scale=1" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-3.7.1.min.js"></script>  
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="style.css" rel="stylesheet" />
</head>
<body>
    <uc:Header runat="server" />
    <form runat="server">
        
        <div class="my-5">
            <h1 align="center"><b>Organize your work</b> </h1>
            <h1 align="center"><b>and life, finally.</b></h1>
            <div class="mt-5">
                <h5 align="center"><i>Become focused, organized, and calm with TaskTrackr.</i></h5> 
                <h5 align="center"><i>The world’s #1 task manager and to-do list app.</i></h5>
            </div>
            
         
        </div>
        <div class="text-center">
            <asp:Button ID="btn_signup" runat="server" Text="Sign Up" CssClass="btn btn-primary" ClientIDMode="AutoID" OnClick="btn_signup_Click1" />
        </div>
        <div class="text-center my-5">
             <asp:AdRotator ID="AdRotator1" runat="server" AdvertisementFile="~/AdRotator.xml" CssClass="advertisement"/>
        </div>
       
    </form>
</body>
</html>
