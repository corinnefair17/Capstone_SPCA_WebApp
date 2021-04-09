<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="SPCA_Capstone.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>About this Application</h2>
    <br />
    <p style="font-size:large">This application was created by four students at Christopher Newport University as a capstone project for our senior year. The goal of this project is to more rapidly identify diseases common in shelter dogs. This is accomplished via a camera recording the dogs and then periodically sending the raw footage through a machine learning algorithm that identifies abnormal behavior that may potentially indicate an illness. Additionally, this application allows staff and volunteers to manually enter data, allowing this application to serve as a single source of information regarding dogs in the shelters.</p>
    <p style="font-size:large">For more information about our team, please visit the <a runat="server" href="~/Contact">Contact page</a>.</p>
</asp:Content>
