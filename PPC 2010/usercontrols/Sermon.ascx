<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Sermon.ascx.cs" Inherits="PPC_2010.Sermon" ViewStateMode="Disabled" %>
<p>
    <h1>Current Sermon</h1>
<p />
<p>
   <h3>"<asp:Label runat="server" ID="sermonTitle" />"</h3>
<p />
    <asp:Label runat="server" ID="speakerName" />&nbsp;|&nbsp;Recorded&nbsp;<asp:Label runat="server" ID="recordingDate" />&nbsp;|&nbsp;<asp:Label runat="server" ID="recordingSession" />
<p />
<p>
    <a class="download_sermonbutton" style="background-image:url(<%= DownloadImageUrl %>)" title="Download Sermon" href="<%= RecordingUrl %>">Download Sermon</a>
</p>

<div id="playSermon">
<h3>Play This Sermon</h3>

 <div style="margin-left:10%;margin-right:10%">
    <br />
    <p id="audioplayer">Sermon player</p>  
    <script type="text/javascript"> <% // The below uses a weird hack to work around a limitation in the AudioPlayer control, it uses comma's to separate title names, so a title can't have a comma in it, so we replace it with U+201A: Single Low-9 Quotation Mark %> 
        AudioPlayer.embed("audioplayer", { "soundFile": <%= System.Web.Helpers.Json.Encode(RecordingUrl) %>, "titles": <%= System.Web.Helpers.Json.Encode(sermonTitle.Text).Replace(",", "\u201A") %>, "artists": <%= System.Web.Helpers.Json.Encode(speakerName.Text) %> });  
    </script>
 </div>
  </div>
<p>
    <asp:Literal runat="server" ID="scriptureText" />
<p />
<asp:HyperLink runat="server" Target="_blank" NavigateUrl="http://feeds.feedburner.com/ProvidencePcaSermons" >
 <%--<asp:HyperLink runat="server" Target="_blank" NavigateUrl="/usercontrols/sermonfeed.ashx" > --%>
    <asp:Image runat="server" ImageUrl="~/UserControls/images/rss.gif" />
</asp:HyperLink>
<p />
<br />
<p>
    <span class="footerText">Scripture taken from The Holy Bible, English Standard Version. Copyright &copy;2001 by <a href="http://www.crosswaybibles.org">Crossway Bibles</a>, a publishing ministry of Good News Publishers. Used by permission. All rights reserved. Text provided by the <a href="http://www.gnpcb.org/esv/share/services/">Crossway Bibles Web Service</a>.</span>
</p>
