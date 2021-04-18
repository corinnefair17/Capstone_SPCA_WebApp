<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SPCA_Capstone.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Title %></h2>
    <h3>For more information, please contact the Watch Dog team </h3>

    <style>
        div.gallery {
            margin: 5px;
            border: 1px solid #ccc;
            width: 180px;
        }

            div.gallery:hover {
                border: 1px solid #777;
            }

            div.gallery img {
                width: 100%;
                height: auto;
            }

        div.desc {
            padding: 15px;
            text-align: center;
        }
    </style>
    <table>
        <tr>
            <td>


                <div class="gallery">
                    <a target="_blank" href="img_5terre.jpg">
                        <img src="/Content/hannah.jpg" alt="Hannah Allen" width="600" height="400">
                    </a>
                    <div class="desc">Hannah Allen
                        <br />
                        Hannah.Allen.17@cnu.edu</div>
                </div>
            </td>
            <td>
                <div class="gallery">
                    <a target="_blank" href="/Content/corinne.jpg">
                        <img src="/Content/corinne.jpg" alt="Corinne Fair" width="600" height="400">
                    </a>
                    <div class="desc">Corinne Fair
                        <br />
                        Corinne.Fair.17@cnu.edu</div>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="gallery">
                    <a target="_blank" href="/Content/danny.jpg">
                        <img src="/Content/danny.jpg" alt="Danny Williams" width="600" height="400">
                    </a>
                    <div class="desc">Danny Williams
                        <br />
                        Danny.Williams.17@cnu.edu</div>
                </div>
            </td>
            <td>
                <div class="gallery">
                    <a target="_blank" href="/Content/henry.jpg">
                        <img src="/Content/henry.jpg" alt="Henry Wilson" width="600" height="400">
                    </a>
                    <div class="desc">Henry Wilson
                        <br />
                        Henry.Wilson.17@cnu.edu</div>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>


