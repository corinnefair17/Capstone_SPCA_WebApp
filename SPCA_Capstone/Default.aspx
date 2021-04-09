<%@ Page Title="PSPCA Dog Tracker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SPCA_Capstone._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron" style="text-align:center">
        <h1>Peninsula SPCA Dog Tracker</h1>
        <br />
        <p class="lead">This application allows for a convenient single information source to keep track of the status of shelter dogs</p>
        <br />
        <p><a runat="server" href="~/Dogs" class="btn btn-primary btn-lg">View dogs &raquo;</a></p>
    </div>

    </asp:Content>
