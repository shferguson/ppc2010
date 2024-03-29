﻿using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using PPC_2010.Data;
using umbraco.NodeFactory;
using umbraco;
using System.Linq;
using System.Collections.Generic;
using PPC_2010.Social.Facebook;
using System.Text.RegularExpressions;
using PPC_2010.Social;
using PPC_2010.Services;

namespace PPC_2010
{
    public partial class ArticleNavigate : System.Web.UI.UserControl
    {
        private readonly IArticleRepository _articleReposiotry;

        public IEnumerable<IArticle> LatestArticles { get; set; }
        public IArticle CurrentArticle { get; set; }

        protected string ShareUrl { get; set; }

        public ArticleNavigate()
        {
            _articleReposiotry = ServiceLocater.Instance.Locate<IArticleRepository>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var currentNode = Node.GetCurrent();
                CurrentArticle = _articleReposiotry.LoadArticle(currentNode.Id);

                var nextArticle = _articleReposiotry.NextArticle(currentNode.Id);
                SetButtonText(nextButton, nextArticle);
                
                var prevArticle = _articleReposiotry.PrevArticle(currentNode.Id);
                SetButtonText(prevButton, prevArticle);

                LatestArticles = _articleReposiotry
                    .LoadAllArticles()
                    .Take(50);

                if (CurrentArticle != null)
                    SetSocialTags(CurrentArticle);
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

        private void SetSocialTags(IArticle article)
        {
            ShareUrl = UrlService.MakeFullUrl(library.NiceUrl(article.Id));
            var tagsService = ServiceLocater.Instance.Locate<ISocialTagsService>();
            tagsService.AddSocialTags(this, new OpenGraphTags
            {
                Type = "article",
                Section = "Articles",
                Url = ShareUrl,
                Title = article.Title,
                Description = Regex.Replace(article.Text.Substring(0, Math.Min(150, article.Text.Length)), @"<[^>]+>|&nbsp;", "").Trim(),
            });
        }
    }
}