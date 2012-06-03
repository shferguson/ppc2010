<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleNavigate.ascx.cs" Inherits="PPC_2010.ArticleNavigate" %>

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
                <% if (!LatestArticles.Any(l => l.Id == CurrentArticle.Id)) %>
                <option style="display:none;" selected="" value="<%= umbraco.library.NiceUrl(CurrentArticle.Id) %>"></option>
            </select>
        </div>
    </div>
</form>
