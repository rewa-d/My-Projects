<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="TaskTrackr_Project.Dashboard" %>
<%@ Register TagPrefix="uc" TagName="Header" Src="~/header.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="styles.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <uc:Header runat="server" />
    <form id="form1" runat="server">
        <div class="container">
    <div class="row mt-5">
        <div class="col-2 offset-10">
            <asp:Button class="btn btn-info w-100 text-white" ID="btn_add" runat="server" Text="Add Task" OnClick="btn_add_Click1" />
        </div>
    </div>
    <div class="row mt-5">
        <div class="col-12">
            <asp:Table ID="table_view" runat="server" CssClass="table border">
                
            </asp:Table>
        </div>
    </div>
</div>

        <div>
           
        </div>
        <div>
            <asp:Label ID="lbl_res" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
