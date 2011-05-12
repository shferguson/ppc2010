<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllSermons.ascx.cs" Inherits="PPC_2010.AllSermons" %>



<form id="form" runat="server">

<h1>All Sermons</h1>



Title:&nbsp;
    <asp:Label ID="sermonTitle" runat="Server" />

    <asp:GridView runat="Server" ID="sermonGrid" AutoGenerateColumns="true">
        <Columns>
            <asp:BoundField Visible="false" DataField="Id" />
            <asp:HyperLinkField HeaderText="Audio Url" Text="RecordingUrl" datanavigateurlfields="RecordingUrl" />
            <asp:HyperLinkField HeaderText="Listen" Text="SermonUrl" datanavigateurlfields="SermonUrl" />
        </Columns>
    </asp:GridView>

    <asp:Button runat="server" Width="100" ID="previous" Text="Previous" OnClick="previousClick" />
    <asp:Button runat="server" Width="100" ID="next" Text="Next" OnClick="nextClick" />
    <br />
    <asp:HyperLink runat="server" Target="_blank" NavigateUrl="~/UserControls/SermonFeed.ashx">
        <asp:Image runat="server" ImageUrl="~/UserControls/images/rss.gif" />
    </asp:HyperLink>
</form>


