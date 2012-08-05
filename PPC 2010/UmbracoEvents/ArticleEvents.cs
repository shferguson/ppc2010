using System;
using System.ComponentModel;
using PPC_2010.Data;
using PPC_2010.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace PPC_2010.UmbracoEvents
{
    public class ArticleEvents : ApplicationBase
    {
        public ArticleEvents()
        {
            Document.New += new Document.NewEventHandler(Document_New);
            Document.BeforeSave += new Document.SaveEventHandler(Document_BeforeSave);
            Document.AfterSave += new Document.SaveEventHandler(Document_Refresh);
            Document.AfterDelete += new Document.DeleteEventHandler(Document_Refresh);
            Document.AfterMoveToTrash += new Document.MoveToTrashEventHandler(Document_Refresh);
            Document.AfterUnPublish += new Document.UnPublishEventHandler(Document_Refresh);
            Document.AfterRollBack += new Document.RollBackEventHandler(Document_Refresh);
            Document.AfterPublish += new Document.PublishEventHandler(Document_Refresh);
        }

        void Document_New(Document sender, NewEventArgs e)
        {
            if (sender.ContentType.Alias == Constants.ArticleAlias)
            {
                if (CheckForRefresh(sender, e))
                    return;

                var article = new Data.Media.Article(sender);
                article.Title = sender.Text;
                article.Date = DateTime.Today.GetDateOfNext(DayOfWeek.Sunday);

                CheckForRefresh(sender, e);
            }
        }

        void Document_BeforeSave(Document sender, SaveEventArgs e)
        {
            if (sender.ContentType.Alias == Constants.ArticleAlias)
            {
                if (CheckForRefresh(sender, e))
                    return;

                var article = new Data.Media.Article(sender);
                if (article.Date.HasValue)
                    sender.Text = "Article-" + article.Date.Value.ToString("MM/dd/yyyy");
            }
           
        }

        void Document_Refresh(Document sender, EventArgs e)
        {
            if (sender.ContentType.Alias == Constants.ArticleAlias)
            {
                ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticle(sender.Id, sender.IsTrashed);
            }
        }

        static bool CheckForRefresh(Document sender, CancelEventArgs e)
        {
            if (sender.Text == Constants.RefreshIndicatorTitle)
            {
                ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticles();
                e.Cancel = true;
            }

            return e.Cancel;
        }
    }
}
