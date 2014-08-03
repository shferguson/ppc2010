<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SermonList.ascx.cs" Inherits="PPC_2010.SermonList"  ViewStateMode="Disabled" %>

<form id="form" runat="server">

<h1>Sermon Archive</h1>

    <asp:GridView runat="Server" ID="sermonGrid" CssClass="sermonTable" AutoGenerateColumns="false"  CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None" OnRowDataBound="sermonGrid_RowDataBound">
        <Columns>
            <asp:BoundField HeaderText="Date" DataField="RecordingDate" DataFormatString="{0:d}" ItemStyle-CssClass="sermonDateColumn" />
            <asp:BoundField HeaderText="Speaker" DataField="SpeakerName"  ItemStyle-CssClass="sermonSpeakerColumn"  />
            <asp:HyperLinkField HeaderText="Title" DataTextField="Title" DataNavigateUrlFields="SermonUrl" ItemStyle-CssClass="sermonTitleColumn" />
            <asp:BoundField HeaderText="Scripture" DataField="ScriptureReference" ItemStyle-CssClass="sermonScriptureColumn" />
        </Columns>
        <RowStyle CssClass="row" />
    </asp:GridView>

    <asp:Button runat="server" Width="100" ID="previous" Text="« Previous" OnClick="previousClick" />
    <asp:Button runat="server" Width="100" ID="next" Text="Next »" OnClick="nextClick" />
    <br />
    <br />
    <asp:HyperLink runat="server" Target="_blank" NavigateUrl="http://feeds.feedburner.com/ProvidencePcaSermons" >
    <%--<asp:HyperLink runat="server" Target="_blank" NavigateUrl="/usercontrols/sermonfeed.ashx" >--%>
        <asp:Image ID="Image1" runat="server" ImageUrl="~/UserControls/images/rss.gif" />
    </asp:HyperLink>

</form>

