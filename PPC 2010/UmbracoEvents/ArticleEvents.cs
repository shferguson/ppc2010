using System;
using System.Collections.Generic;
using System.Linq;
using PPC_2010.Data;
using PPC_2010.Extensions;
using umbraco.businesslogic;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;
using Umbraco.Core;

namespace PPC_2010.UmbracoEvents
{
    public class ArticleEvents : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Created += new TypedEventHandler<IContentService, NewEventArgs<IContent>>(ContentService_Created);
            ContentService.Saving += new TypedEventHandler<IContentService, SaveEventArgs<IContent>>(ContentService_Saving);
            ContentService.Deleted += new TypedEventHandler<IContentService, DeleteEventArgs<IContent>>(ContentService_Deleted);
            ContentService.Saved += new TypedEventHandler<IContentService, SaveEventArgs<IContent>>(ContentService_Saved);
            ContentService.RolledBack += new TypedEventHandler<IContentService, RollbackEventArgs<IContent>>(ContentService_RolledBack);
            ContentService.SentToPublish += new TypedEventHandler<IContentService, SendToPublishEventArgs<IContent>>(ContentService_SentToPublish);
            ContentService.Trashed += new TypedEventHandler<IContentService, MoveEventArgs<IContent>>(ContentService_Trashed);
            ContentService.UnPublished += new TypedEventHandler<IPublishingStrategy, PublishEventArgs<IContent>>(ContentService_UnPublished);
            ContentService.Published += new TypedEventHandler<IPublishingStrategy, PublishEventArgs<IContent>>(ContentService_Published);
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext) { }

        void ContentService_Created(IContentService sender, NewEventArgs<IContent> e)
        {
            if (e.Entity.ContentType.Alias == PPC_2010.Data.Constants.ArticleAlias)
            {
                if (CheckForRefresh(e.Entity, e))
                    return;

                var article = new Data.Media.Article(e.Entity);
                article.Title = e.Entity.Name;
                article.Date = DateTime.Today.GetDateOfNext(DayOfWeek.Sunday);
            }
        }

        void ContentService_Saving(IContentService sender, SaveEventArgs<IContent> e)
        {
            foreach (IContent content in e.SavedEntities)
            {
                if (content.ContentType.Alias == PPC_2010.Data.Constants.ArticleAlias)
                {
                    if (CheckForRefresh(content, e))
                        return;

                    var article = new Data.Media.Article(content);
                    if (article.Date.HasValue)
                        content.Name = "Article-" + article.Date.Value.ToString("MM/dd/yyyy");
                }
            }
        }

        void ContentService_Published(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            RefreshArticles(e.PublishedEntities);
        }

        void ContentService_UnPublished(IPublishingStrategy sender, PublishEventArgs<IContent> e)
        {
            RefreshArticles(e.PublishedEntities);
        }

        void ContentService_Trashed(IContentService sender, MoveEventArgs<IContent> e)
        {
            foreach (var entity in e.MoveInfoCollection.Select(m => m.Entity))
                RefreshArticle(entity);
        }

        void ContentService_SentToPublish(IContentService sender, SendToPublishEventArgs<IContent> e)
        {
            RefreshArticle(e.Entity);
        }

        void ContentService_RolledBack(IContentService sender, RollbackEventArgs<IContent> e)
        {
            RefreshArticle(e.Entity);
        }

        void ContentService_Saved(IContentService sender, SaveEventArgs<IContent> e)
        {
            RefreshArticles(e.SavedEntities);
        }

        void ContentService_Deleted(IContentService sender, DeleteEventArgs<IContent> e)
        {
            RefreshArticles(e.DeletedEntities);
        }

        void RefreshArticle(IContent content)
        {
            ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticle(content.Id, content.Status == ContentStatus.Trashed);
        }

        void RefreshArticles(IEnumerable<IContent> contents)
        {
            foreach (IContent content in contents.Where(c => c.ContentType.Alias == PPC_2010.Data.Constants.ArticleAlias))
            {
                RefreshArticle(content);
            }
        }

        // As of Umbraco 6.2.4 if you try to access e.Cancel for an event that isn't cancellable you'll get an exception
        static bool CheckForRefresh(IContent content, CancellableEventArgs e)
        {
            if (content.Name == PPC_2010.Data.Constants.RefreshIndicatorTitle)
            {
                ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticles();
                return true;
            }

            return false;
        }
    }
}
