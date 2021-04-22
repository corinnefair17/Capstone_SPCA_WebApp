<%@ Page Title="Dog Listing" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dogs.aspx.cs" Inherits="SPCA_Capstone.WebForm1" %>
<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div id="headerTable">
        <table id="headerTableArea" class="nav-justified">
        <tr>
            <td id="titleArea" style="text-align: left">
                <h1 id="titleText" runat="server">Dog Listing</h1>
            </td>
            <td id="selectDogArea" runat="server" style="text-align:right; font-size: large">
                <asp:DropDownList class="btn btn-primary dropdown-toggle" ID="DogDropdown" AppendDataBoundItems="true" runat="server" DataSourceID="DogsData" DataTextField="Dog_Name" DataValueField="Dog_Name" AutoPostBack="true" OnSelectedIndexChanged="Load_Dog_Click">
                    <asp:ListItem Text="Select Dog" Value="Select Dog" />
                </asp:DropDownList>
                <asp:SqlDataSource ID="DogsData" runat="server" ConnectionString="<%$ ConnectionStrings:SPCAConnectionString %>" SelectCommand="SELECT * FROM [Dogs]"></asp:SqlDataSource>
            </td>
        </tr>
        </table>
    </div>

    <hr />

    <div id="mainTable">
        <table id="singleDogInfo" style="table-layout:fixed; margin-left:auto; margin-right:auto; width:850px">
        <tbody>
            <tr style="text-align:center; font-size:large">
                <td>
                    <asp:RequiredFieldValidator ID="nameRequiredValidator" CssClass="text-danger" Display="Static" ControlToValidate="dogName" ErrorMessage="Dog Name Required" runat="server">*</asp:RequiredFieldValidator>
                    Name:&nbsp;
                    <textarea id="dogName" runat="server" rows="1" placeholder="No Data" disabled style="resize:none; background-color:#FFF"></textarea>
                </td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td rowspan="4" style="text-align:center">
                    <asp:Image ID="dogPic" ImageURL="/Content/paw_print.png" runat="server" Width="350px"/>
                </td>
                <td style="text-align:right; font-size:large">
                    <asp:CompareValidator ID="kennelIntValidator" CssClass="text-danger" ControlToValidate="dogKennelID" Operator="DataTypeCheck" ErrorMessage="Kennel ID must be an integer" runat="server" Type="Integer">*</asp:CompareValidator>
                    <asp:RequiredFieldValidator ID="kennelRequiredValidator" class="text-danger" ControlToValidate="dogkennelID" ErrorMessage="Kennel ID Required" runat="server">*</asp:RequiredFieldValidator>
                    Kennel ID:&nbsp;       
                    <textarea id="dogKennelID" runat="server" rows="1" placeholder="No Data" disabled style="resize:none; background-color:#FFF; width:250px"></textarea>             
                </td>
            </tr>
            <tr>
                <td style="text-align:right; font-size:large">
                    Breed:&nbsp;
                    <textarea id="dogBreed" runat="server" rows="1" placeholder="No Data" MaxLength="255" disabled style="resize:none; background-color:#FFF; width:250px"></textarea>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; font-size:large">
                    <asp:CompareValidator ID="ageDoubleValidator" CssClass="text-danger" ControlToValidate="dogAge" Operator="DataTypeCheck" ErrorMessage="Age must be an integer or decimal number" runat="server" Type="Double">*</asp:CompareValidator>
                    Age:&nbsp;
                    <textarea id="dogAge" runat="server" rows="1" placeholder="No Data" disabled style="resize:none; background-color:#FFF; width:250px"></textarea>
                </td>
            </tr>
            <tr>
                <td style="text-align:right; font-size:large">
                    <asp:CompareValidator ID="weightDoubleValidator" CssClass="text-danger" ControlToValidate="dogWeight" Operator="DataTypeCheck" ErrorMessage="Weight must be an integer or decimal number" runat="server" Type="Double">*</asp:CompareValidator>
                    Weight:&nbsp;
                    <textarea id="dogWeight" runat="server" rows="1" placeholder="No Data" disabled style="resize:none; background-color:#FFF; width:250px"></textarea>
                </td>
            </tr>
            <tr>
                <td style="text-align:left; padding-left:100px">
                    <br />
                    <asp:FileUpload ID="imageUpload" runat="server" visible="false" /><br />
                </td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2" style="font-size:large; white-space:pre">
                    Notes:&nbsp;
                    <textarea id="notes" runat="server" rows="5" disabled style="font-size:large; resize:vertical; width:100%; max-width:88%; background-color:#FFF"></textarea>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="font-size:large; white-space:pre">
                    <div id="chart" style="width:850px; height:500px; text-align:center"></div>
                </td>
            </tr>
        </tbody>
        </table>
        <script type="text/javascript" src="https://code.highcharts.com/highcharts.js"></script>
        <script>
            $('#chart').highcharts({
                chart: {
                    type: 'spline'
                },
                title: {
                    text: 'Dog Activity Data'
                },
                xAxis: {
                    title: {
                        text: 'Time'
                    },
                    type: 'datetime',
                    labels: {
                        format: '{value:%e %b %H:%M:%S}'
                    }
                },
                yAxis: {
                    title: {
                        text: 'Meters/Minute'
                    }
                },
                series: [{
                    type: 'spline',
                    name: 'Movement Data',
                    data: <%=chartData%>
                }]
            });
        </script>
    </div>
    <hr />
    <div id="optionsArea" style="text-align:right" runat="server">
        <asp:Button ID="addDogButton" class="btn btn-primary" runat="server" Text="Add New Dog" Width="140px" OnClick="Add_Dog_Click" CausesValidation="false" />&nbsp;
        <asp:Button ID="editDogButton" class="btn btn-primary" runat="server" Text="Edit Current Dog" Width="140px" OnClick="Edit_Dog_Click" CausesValidation="false" />&nbsp;
        <asp:Button ID="deleteDogButton" class="btn btn-primary" runat="server" Text="Delete Current Dog" Width="140px" OnClick="Delete_Dog_Click" OnClientClick="return confirm('Are you sure you want to delete this dog?')" CausesValidation="false" />
    </div>
    <div id="validationArea" style="text-align:right" runat="server">
        <asp:ValidationSummary ID="validationSummary" CssClass="text-danger" DisplayMode="List" runat="server" ShowSummary="true" HeaderText="One or more fields are invalid" />

    </div>
    <br />
    <div id="saveCancelArea" style="text-align:right" runat="server" visible="false">
        <asp:Button ID="saveButton" class="btn btn-primary" runat="server" Text="Save" Width="70px" OnClick="Save_Dog_Click" CausesValidation="true" />&nbsp;
        <asp:Button ID="cancelButton" class="btn btn-danger" runat="server" Text="Cancel" Width="70px" OnClick="Cancel_Button_Click" OnClientClick="return confirm('Are you sure you want to cancel?')" CausesValidation="false" />
    </div>
</asp:Content>