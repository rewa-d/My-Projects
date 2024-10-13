<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Task_Add.aspx.cs" Inherits="TaskTrackr_Project.Task_Add" %>
<%@ Register TagPrefix="uc" TagName="Header" Src="~/header.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="styles.css" rel="stylesheet" />
</head>
<body>
    <uc:Header runat="server" />
        <form id="form1" runat="server">
        <div class="container">
            <div class="big-box">
                <div class="row mt-3">
                <div class="col-12 text-center">
                    <h3>Add Task</h3>
                </div>
            </div>
            <div class="row mt-5">
                <div class="col-3 text-right">
                    <asp:Label ID="lbl_title" runat="server" Text="Title"></asp:Label>
                </div>
                <div class="col-6">
                     <asp:TextBox ID="txt_title" runat="server" class="w-100" style="margin-left: 18px"></asp:TextBox>
                </div>
            </div>
        

        <div class="row mt-3">
                <div class="col-3 text-right">
                     <asp:Label ID="lbl_desc" runat="server" Text="Description"></asp:Label>
                </div>
                <div class="col-6">
                     <asp:TextBox ID="txt_desc" runat="server" class="w-100" style="margin-left: 18px" TextMode="MultiLine"></asp:TextBox>
                </div>
        </div>

        <div class="row mt-3">
            <div class="col-3 text-right">
                 <asp:Label ID="lbl_start" runat="server" Text="Start Date"></asp:Label>
            </div>
            <div class="col-6">
                  <asp:TextBox ID="txt_start" runat="server" class="w-100" style="margin-left: 18px" TextMode="Date"></asp:TextBox>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-3 text-right">
                 <asp:Label ID="lbl_end" runat="server" Text="End Date"></asp:Label>
            </div>
            <div class="col-6">
                   <asp:TextBox ID="txt_end" runat="server" class="w-100" style="margin-left: 18px" TextMode="Date"></asp:TextBox>
            </div>

        </div>

  <div class="row mt-3">
      <div class="col-3 text-right">
            <asp:Label ID="lbl_due" runat="server" Text="Due Date"></asp:Label>
      </div>
      <div class="col-6">
             <asp:TextBox ID="txt_due" runat="server" class="w-100" style="margin-left: 18px" TextMode="Date"></asp:TextBox>
      </div>

  </div>


<div class="row mt-5">
<div class="col-4 offset-4">
    <asp:Button ID="btn_add"  runat="server" Text="Add Task" CssClass="btn btn-primary w-100" OnClick="btn_add_Click" />
</div>
  </div>  
            </div>
            
  <div class="row mt-3">
<div class="col-12">
    <asp:Label ID="lbl_res" runat="server"></asp:Label>
</div>
  </div>  
    
</div>

    </form>
</body>
</html>
