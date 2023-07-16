using System;
using System.Web.UI;
using PPC_2010.Data;
using umbraco;

namespace PPC_2010
{
    public partial class LatestArticleLink : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var articleRepo = ServiceLocater.Instance.Locate<IArticleRepository>();
                var article = articleRepo.LoadLatestArticle();
                if (article != null)
                {
                    articleLink.NavigateUrl = library.NiceUrl(article.Id);
                    articleLink.Text = article.Title;
                }
            }
        }
    }
}