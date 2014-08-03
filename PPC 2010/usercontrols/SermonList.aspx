<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SermonList.aspx.cs" Inherits="PPC_2010.SermonList" %>
<%--<%@ Register Namespace=PPC_2010.Controls" TagPrefix="controls" %>--%>



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

     <asp:DataPager ID="pager" runat="server" PageSize="8" PagedControlID="sermonGrid">
                                    <Fields>
                                        <asp:NextPreviousPagerField
                                            ButtonCssClass="command" 
                                            FirstPageText="«" PreviousPageText="‹"                                             
                                            RenderDisabledButtonsAsLabels="true" 
                                            ShowFirstPageButton="true" ShowPreviousPageButton="true" 
                                            ShowLastPageButton="false" ShowNextPageButton="false"
                                        /> 
                                        <asp:NumericPagerField
                                            ButtonCount="7" NumericButtonCssClass="command" 
                                            CurrentPageLabelCssClass="current" NextPreviousButtonCssClass="command"
                                        />
                                        <asp:NextPreviousPagerField 
                                            ButtonCssClass="command" 
                                            LastPageText="»" NextPageText="›" 
                                            RenderDisabledButtonsAsLabels="true" 
                                            ShowFirstPageButton="false" ShowPreviousPageButton="false" 
                                            ShowLastPageButton="true" ShowNextPageButton="true"
                                        />                                            
                                    </Fields>                            
                                </asp:DataPager>

    <br />

    <asp:HyperLink runat="server" Target="_blank" NavigateUrl="~/UserControls/SermonFeed.ashx">
        <asp:Image runat="server" ImageUrl="~/UserControls/images/rss.gif" />
    </asp:HyperLink>
</form>


