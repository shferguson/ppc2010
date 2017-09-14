<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleNavigate.ascx.cs" Inherits="PPC_2010.ArticleNavigate" ViewStateMode="Disabled" %>

<form runat="server">
    <div>
        <div style="width:300px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;float:left;text-align:left">
            <a runat="server" id="prevButton"></a>
        </div>
        <div style="width:300px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;float:right;text-align:right">
            <a runat="server" id="nextButton"></a>
        </div>
        <br />
        <div style="margin: 0px auto;text-align:center;">
            Latest Articles:
            <br />
            <select onchange="window.location=$(this).val()">
                <% foreach (var article in LatestArticles) { %>
                       <%=  string.Format("<option {2}value=\"{0}\">{1}</option>",
                           umbraco.library.NiceUrl(article.Id),
                           article.Date.Value.ToShortDateString() + " - " + article.Title,
                           article.Id == CurrentArticle.Id ? "selected=\"\" " : "") %>
                <% } %> 
                <% if (!LatestArticles.Any(l => l.Id == CurrentArticle.Id)) { %>
                <option style="display:none;" selected="" value="<%= umbraco.library.NiceUrl(CurrentArticle.Id) %>"></option>
                <% } %>
            </select>
        </div>
    </div>
</form>

<div style="display: flex; flex-direction: row; align-items: center; margin: 20px 0px">
    <div>
        <div class="fb-share-button" data-href="<%=ShareUrl%>" data-layout="button" data-size="small" data-mobile-iframe="true"></div>
    </div>
    <div style="margin-left: 10px;">
        <a class="twitter-share-button" href="https://twitter.com/intent/tweet?url=<%=Uri.EscapeUriString(ShareUrl)%>"></a>
    </div>
    <a style="margin-left:auto" href="http://feeds.feedburner.com/ProvidencePcaArticles" target="_blank"><img src="/UserControls/images/rss.gif"></img></a>
</div>
