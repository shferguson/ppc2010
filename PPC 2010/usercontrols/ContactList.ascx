<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactList.ascx.cs" Inherits="PPC_2010.ContactList"  ViewStateMode="Disabled" %>

<table cellpadding="0" cellspacing="0">
<asp:Repeater runat="server" ID="contacts">
<ItemTemplate>
    <tr class="contact">
        <td>
            <a class="contactImage" style="background-image:url(<%# DataBinder.Eval(Container.DataItem, "ImageUrl") %>)" href="mailto:<%# DataBinder.Eval(Container.DataItem, "Email")%>" title="<%# DataBinder.Eval(Container.DataItem, "Name") %>"></a>
        </td>
        <td class="contactInfo">
            <h4><%# DataBinder.Eval(Container.DataItem, "Name") %></h4>
            <p />
            <%# DataBinder.Eval(Container.DataItem, "Position") %><br />
            Phone:&nbsp;<%# DataBinder.Eval(Container.DataItem, "Phone") %><br />
            Email:&nbsp;<a href="mailto:<%# DataBinder.Eval(Container.DataItem, "Email") %>"><%# DataBinder.Eval(Container.DataItem, "Email")%></a>
        </td>
    </tr>
</ItemTemplate>
</asp:Repeater>
</table>