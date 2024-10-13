<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Header1.ascx.cs" Inherits="TaskTrackr_Project.Header1" %>
<!DOCTYPE html>

<head runat="server">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-3.7.1.min.js"></script>  
    <script src="Scripts/bootstrap.min.js"></script>
</head>

<body>
<nav class="navbar navbar-expand-lg navbar-light bg-light">
    <div class="container-fluid">
        <a class="navbar-brand" href="Home.aspx">
            <div class="d-flex justify-content-start align-items-center">
                <asp:Image ID="img_logo" runat="server" ImageUrl="~/logo1.png" Style="max-width:45px" />
                <h2>TaskTrackr</h2>
            </div>
        </a>

        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse justify-content-end" id="navbarNav">
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a class="nav-link" href="home.aspx">About TaskTrackr</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="Login.aspx">Login</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="SignUp.aspx">Sign Up</a>
                </li>
            </ul>
        </div>
    </div>
</nav>
</body>
</html>