using System;
using System.Web.UI;
using PPC_2010.Data;
using umbraco;

namespace PPC_2010
{
    public partial class LatestArticle : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var articleRepo = ServiceLocator.Instance.Locate<IArticleRepository>();
                var article = articleRepo.LoadLatestArticle();
                if (article == null)
                {
                    articleLink.Visible = false;
                }
                else
                {
                    articleLink.HRef = library.NiceUrl(article.Id);
                }
            }
        }
    }
}