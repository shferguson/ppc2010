<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sermon.ascx.cs" Inherits="PPC_2010.Sermon" ViewStateMode="Disabled" %>
<h1>Current Sermon</h1>
<h3>"<asp:Label runat="server" ID="sermonTitle" />"</h3>
<p>
    <asp:Label runat="server" ID="speakerName" />&nbsp;|&nbsp;Recorded&nbsp;<asp:Label runat="server" ID="recordingDate" />&nbsp;|&nbsp;<asp:Label runat="server" ID="recordingSession" />
<p />
<p>
    <a class="download_sermonbutton" style="background-image:url(<%= DownloadImageUrl %>)" title="Download Sermon" href="<%= RecordingUrl %>">Download Sermon</a>
</p>

<div id="playSermon">
<h3>Play This Sermon</h3>

 <div style="margin-left:10%;margin-right:10%">
    <br />
    <audio src="<%= RecordingUrl %>"></audio>
 </div>
  </div>
<p>
    <asp:Literal runat="server" ID="scriptureText" />
<p />
<div style="display: flex; flex-direction: row; align-items: center; margin: 20px 0px">
    <div>
        <div class="fb-share-button" data-href="<%=ShareUrl%>" data-layout="button" data-size="small" data-mobile-iframe="true"></div>
    </div>
    <div style="margin-left: 10px;">
        <a class="twitter-share-button" href="https://twitter.com/intent/tweet?url=<%=Uri.EscapeUriString(ShareUrl)%>"></a>
    </div>
    <a style="margin-left:auto" href="http://feeds.feedburner.com/ProvidencePcaSermons" target="_blank"><img src="/UserControls/images/rss.gif"></img></a>
</div>
<p>
    <span class="footerText">Scripture taken from The Holy Bible, English Standard Version. Copyright &copy;2001 by <a href="http://www.crosswaybibles.org">Crossway Bibles</a>, a publishing ministry of Good News Publishers. Used by permission. All rights reserved. Text provided by the <a href="http://www.gnpcb.org/esv/share/services/">Crossway Bibles Web Service</a>.</span>
</p>
