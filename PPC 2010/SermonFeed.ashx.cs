using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PPC_2010.Data;
using RssToolkit.Rss;
using System.Web.UI;
using PPC_2010.TimeZone;

namespace PPC_2010
{
    /// <summary>
    /// Summary description for SermonFeed
    /// </summary>
    public class SermonFeed : RssToolkit.Rss.RssDocumentHttpHandler
    {
        protected override void PopulateRss(string rssName, string userName)
        {
            string baseUrl = string.Format("{0}://{1}{2}",  
                        HttpContext.Current.Request.Url.Scheme,  
                        HttpContext.Current.Request.ServerVariables["HTTP_HOST"],  
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? string.Empty : HttpContext.Current.Request.ApplicationPath  
                        );
            Rss.Version = "2.0";
            Rss.Channel = new RssToolkit.Rss.RssChannel();
            Rss.Channel.Title = rssName;
            Rss.Channel.Title = "Providence PCA Sermons";
            Rss.Channel.PubDate = DateTime.UtcNow.ToString("r");
            Rss.Channel.LastBuildDate = DateTime.UtcNow.ToString("r");
            Rss.Channel.WebMaster = "newsletter@providence-pca.net (Providence Presbyterian Church)";
            Rss.Channel.Description = "Sermons from Providence Presbyterian Church in Robinson, PA";
            Rss.Channel.Link = Context.Request.Url.ToString();
                        
            SermonRepository repsository = new SermonRepository();

            Rss.Channel.Items = repsository.LoadLastSermons(50).Select(s =>
                new RssItem()
                {
                    Title = s.Title,
                    Description = s.SpeakerName + " - " + s.ScriptureReferenceText,
                    //Link = baseUrl + "/Sermon?id=" + s.Id,
                    Link = baseUrl + s.RecordingUrl.Replace("~", ""),
                    PubDateParsed = TimeZoneConverter.ConvertToEastern(s.RecordingDate.Date.AddHours(12)),
                    //Guid = new RssGuid() { Text = baseUrl + "/Sermon?SermonId=" + s.Id },
                    Guid = new RssGuid() { Text = baseUrl + s.RecordingUrl.Replace("~", "") }
                }
                ).ToList();
        }        
    }
}