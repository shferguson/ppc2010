﻿using System;
using PPC_2010.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;
using PPC_2010.Data;

namespace PPC_2010.UmbracoEvents
{
    public class ArticleEvents : ApplicationBase
    {
        static ArticleEvents()
        {
            Document.New += new Document.NewEventHandler(Document_New);
            Document.BeforeSave += new Document.SaveEventHandler(Document_BeforeSave);
            Document.AfterSave += new Document.SaveEventHandler(Document_AfterSave);
        }

        static void Document_New(Document sender, NewEventArgs e)
        {
            if (sender.ContentType.Alias == "Article")
            {
                var article = new Data.Media.Article(sender);
                article.Title = sender.Text;
                article.Date = DateTime.Today.GetDateOfNext(DayOfWeek.Sunday);
            }
        }

        static void Document_BeforeSave(Document sender, SaveEventArgs e)
        {
            if (sender.ContentType.Alias == "Article")
            {
                var article = new Data.Media.Article(sender);
                if (article.Date.HasValue)
                    sender.Text = "Article-" + article.Date.Value.ToString("MM/dd/yyyy");
            }
           
        }

        static void Document_AfterSave(Document sender, SaveEventArgs e)
        {
            ServiceLocator.Instance.Locate<IArticleRepository>().RefreshArticles();
        }
    }
}
