<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calendar.ascx.cs" Inherits="PPC_2010.Calendar"  ViewStateMode="Disabled" %>
<%@ Register Assembly="BaseControls" Namespace="BaseControls" TagPrefix="bc" %>

  <bc:BaseCalendar runat="server"  ID="calendar"
    NavPrevFormat='<a href="calendar.aspx?d={0:d}">{0:MMM}</a>'
    NavNextFormat='<a href="calendar.aspx?d={0:d}">{0:MMM}</a>'
    UrlVisibleDateAttribute="d"  
    
    CssClass="calendar"  
    
    CssHeaderNavigationPrevious="calendar_navprev"
    CssHeaderNavigationCurrent="calendar_navcurrent"
    CssHeaderNavigationNext="calendar_navnext"    
    OnRenderBodyDay="CalendarBodyDay"
  />