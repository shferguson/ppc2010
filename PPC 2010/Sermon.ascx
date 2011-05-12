<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sermon.ascx.cs" Inherits="PPC_2010.Sermon" ViewStateMode="Disabled" %>
<p />
    <h1>Current Sermon</h1>
<p />
   <h3>"<asp:Label runat="server" ID="sermonTitle" />"</h3>
<p />
    <asp:Label runat="server" ID="speakerName" />&nbsp;|&nbsp;Recorded&nbsp;<asp:Label runat="server" ID="recordingDate" />&nbsp;|&nbsp;<asp:Label runat="server" ID="recordingSession" />
<p />
<p>
    <a class="download_sermonbutton" style="background-image:url(<%= DownloadImageUrl %>)" title="Download Sermon" href="<%= RecordingUrl %>" >Download Sermon</a>
</p>
    <h3>Play This Sermon</h3>
<p />
<div style="margin-left:10%;margin-right:10%">
    <embed style="background:green" type="application/x-mplayer2" src="<%= RecordingUrl %>" name="MediaPlayer"
		    width="500" height="<%= MediaPlayerHeight %>" showcontrols="1" showstatusbar="1" showdisplay="0" autostart="0" />
</div>
<p />
    <asp:Literal runat="server" ID="scriptureText" />
<p />
<%--<asp:HyperLink runat="server" Target="_blank" NavigateUrl="http://feeds.feedburner.com/ProvidencePcaSermons" >--%>
 <asp:HyperLink runat="server" Target="_blank" NavigateUrl="/usercontrols/sermonfeed.ashx" >
    <asp:Image runat="server" ImageUrl="~/UserControls/images/rss.gif" />
</asp:HyperLink>
<p />
<br />
<p>
    <span class="footerText">Scripture taken from The Holy Bible, English Standard Version. Copyright &copy;2001 by <a href="http://www.crosswaybibles.org">Crossway Bibles</a>, a publishing ministry of Good News Publishers. Used by permission. All rights reserved. Text provided by the <a href="http://www.gnpcb.org/esv/share/services/">Crossway Bibles Web Service</a>.</span>
</p>
