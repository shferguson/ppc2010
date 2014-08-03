<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdminLogin.ascx.cs" Inherits="PPC_2010.AdminLogin" %>
<form id="form1" runat="server">
    <div>
        <asp:Login ID="Login1" runat="server" BackColor="#F7F7DE" BorderColor="#CCCC99" BorderStyle="Solid"
            BorderWidth="1px" 
            DestinationPageUrl="/" Font-Names="Verdana" Font-Size="10pt" >
            <TitleTextStyle BackColor="#6B696B" Font-Bold="True" ForeColor="#FFFFFF" />
        </asp:Login>
     </div>
</form>
