using System;
using PPC_2010.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using PPC_2010.Data;
using System.ComponentModel;

namespace PPC_2010.UmbracoEvents
{
    public class ArticleEvents : ApplicationBase
    {
        public ArticleEvents()
        {
            Document.New += new Document.NewEventHandler(Document_New);
            Document.BeforeSave += new Document.SaveEventHandler(Document_BeforeSave);
            Document.AfterSave += new Document.SaveEventHandler(Document_AfterSave);
            Document.AfterDelete += new Document.DeleteEventHandler(Document_AfterDelete);
            Document.AfterMoveToTrash += new Document.MoveToTrashEventHandler(Document_AfterMoveToTrash);
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

        void Document_AfterSave(Document sender, SaveEventArgs e)
        {
            if (sender.ContentType.Alias == Constants.ArticleAlias)
            {
                ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticle(sender.Id, sender.IsTrashed);
            }
        }

        void Document_AfterDelete(Document sender, DeleteEventArgs e)
        {
            if (sender.ContentType.Alias == Constants.ArticleAlias)
            {
                ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticle(sender.Id, true);
            }
        }

        void Document_AfterMoveToTrash(Document sender, MoveToTrashEventArgs e)
        {
            if (sender.ContentType.Alias == Constants.ArticleAlias)
            {
                ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticle(sender.Id, true);
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
