<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SermonSearch.ascx.cs" Inherits="PPC_2010.SermonSearch" %>
<form runat="server">
    <table>
        <tr>
            <td align="right">Year</td>
            <td><asp:DropDownList runat="server" ID="year" Width="200" /></td>
        </tr>
        <tr>
            <td align="right">Month</td>
            <td><asp:DropDownList runat="server" ID="month" Width="200" /></td>
        </tr>
        <tr>
            <td align="right">Speaker</td>
            <td><asp:DropDownList runat="server" ID="speaker" Width="200" /></td>
        </tr>
         <tr>
            <td align="right">Audio Type</td>
            <td><asp:DropDownList runat="server" ID="audioType" Width="200" /></td>
        </tr>
        <tr>
            <td align="right">Series</td>
            <td><asp:DropDownList runat="server" ID="audioSeries" Width="200" /></td>
        </tr>
        <tr>
            <td align="right">Title</td>
            <td><asp:TextBox runat="server" ID="title" Width="195" /></td>
        </tr>
    </table>

    <br />
    <asp:Button runat="server" ID="search" Text="Search" OnClick="searchClick" />

</form>

