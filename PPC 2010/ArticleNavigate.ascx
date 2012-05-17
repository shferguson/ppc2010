<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ArticleNavigate.ascx.cs" Inherits="PPC_2010.ArticleNavigate" %>

<div>
    <div style="width:300px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;float:left;text-align:left">
        <a runat="server" id="prevButton"></a>
    </div>
    <div style="width:300px;text-overflow:ellipsis;overflow:hidden;white-space: nowrap;float:right;text-align:right">
        <a runat="server" id="nextButton"></a>
    </div>
</div>
<div>
    <asp:Label ID="label" runat="server" />
</div>