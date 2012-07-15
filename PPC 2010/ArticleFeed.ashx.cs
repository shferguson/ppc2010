using System;
using System.Linq;
using System.Text.RegularExpressions;
using PPC_2010.Data;
using PPC_2010.TimeZone;
using RssToolkit.Rss;
using umbraco;

namespace PPC_2010
{
    /// <summary>
    /// Summary description for ArticleFeed
    /// </summary>
    public class ArticleFeed : RssToolkit.Rss.RssDocumentHttpHandler
    {
        const int TextSnippetLength = 400;

        protected override void PopulateRss(string rssName, string userName)
        {
            Rss.Version = "2.0";
            Rss.Channel = new RssToolkit.Rss.RssChannel();
            Rss.Channel.Title = rssName;
            Rss.Channel.Title = "Providence PCA Articles";
            Rss.Channel.PubDate = DateTime.UtcNow.ToString("r");
            Rss.Channel.LastBuildDate = DateTime.UtcNow.ToString("r");
            Rss.Channel.WebMaster = "newsletter@providence-pca.net (Providence Presbyterian Church)";
            Rss.Channel.Description = "Articles from Providence Presbyterian Church in Robinson Twp, PA";
            Rss.Channel.Link = Context.Request.Url.ToString();

            using (IArticleRepository repsository = ServiceLocator.Instance.Locate<IArticleRepository>())
            {
                Rss.Channel.Items = repsository.LoadLastArticles(50).Select(a =>
                    new RssItem()
                    {
                        Title = a.Title,
                        Description =  FormatDescription(a),
                        Link = library.NiceUrl(a.Id),
                        PubDateParsed = TimeZoneConverter.ConvertToEastern(a.Date.GetValueOrDefault().Date.AddHours(12)),
                        Guid = new RssGuid() { Text = library.NiceUrl(a.Id) }
                    }
                    ).ToList();
            }
        }

        private string FormatDescription(IArticle article)
        {
            string text = Regex.Replace(article.Text + "...", "<.*?>", string.Empty);

            if (text.Length > TextSnippetLength)
            {
                int spaceIndex = text.Substring(TextSnippetLength, text.Length - TextSnippetLength).IndexOfAny(new[] { ' ', '\t', '\r', '\n' }) + TextSnippetLength;

                text = text.Substring(0, spaceIndex) + "...";
            }

            return text;
        }
    }
}