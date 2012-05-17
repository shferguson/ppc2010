using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using PPC_2010.Data;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;
using umbraco;


namespace PPC_2010
{
    public partial class ArticleNavigate : System.Web.UI.UserControl
    {
        private readonly IArticleRepository _articleReposiotry;

        public ArticleNavigate()
        {
            _articleReposiotry = ServiceLocator.Instance.Locate<IArticleRepository>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var currentNode = Node.GetCurrent();

                var nextArticle = _articleReposiotry.NextArticle(currentNode.Id);
                SetButtonText(nextButton, nextArticle);
                
                var prevArticle = _articleReposiotry.PrevArticle(currentNode.Id);
                SetButtonText(prevButton, prevArticle);
            }
        }

        private void SetButtonText(HtmlAnchor link, IArticle article)
        {
            if (article == null)
            {
                link.Visible = false;
            }
            else
            {
                link.InnerText = article.Title;
                link.HRef = library.NiceUrl(article.Id);
            }
        }
    }
}