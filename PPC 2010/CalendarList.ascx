<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CalendarList.ascx.cs" Inherits="PPC_2010.CalendarList"  ViewStateMode="Disabled" %>

<asp:Repeater ID="calendarList" runat="server">
    <ItemTemplate>
        <span class="emph2"><%# DataBinder.Eval(Container.DataItem, "Date") %></span><br />
        <asp:Repeater ID="dayItems" runat="server" DataSource="<%#  ((CalendarItemDayGroup)Container.DataItem).Items  %>"  >
            <ItemTemplate>
                <span class="emph1"><%# DataBinder.Eval(Container.DataItem, "Start") %></span><%# DataBinder.Eval(Container.DataItem, "Title") %><br />
            </ItemTemplate>
        </asp:Repeater>
        <p />
    </ItemTemplate>
</asp:Repeater>
