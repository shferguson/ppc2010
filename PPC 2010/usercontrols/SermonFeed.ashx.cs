using System;
using System.Text;
using System.Web;
using Microsoft.SyndicationFeed.Rss;
using Microsoft.SyndicationFeed;
using System.Xml;
using PPC_2010.Data;

namespace PPC_2010
{
    /// <summary>
    /// Summary description for SermonFeed
    /// </summary>
    public class SermonFeed : IHttpHandler
    {
        public bool IsReusable => true;

        public async void ProcessRequest(HttpContext context)
        {
            string baseUrl = string.Format("{0}://{1}{2}",
                        HttpContext.Current.Request.Url.Scheme,
                        HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                        (HttpContext.Current.Request.ApplicationPath.Equals("/")) ? string.Empty : HttpContext.Current.Request.ApplicationPath
                        );

            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "application/rss+xml";

            using (XmlWriter xmlWriter = XmlWriter.Create(context.Response.OutputStream, new XmlWriterSettings() { Async = true, Indent = true }))
            {
                var writer = new RssFeedWriter(xmlWriter);

                await writer.WriteTitle("Providence PCA Sermons");
                await writer.WritePubDate(DateTime.Now);
                await writer.WriteDescription("Sermons from Providence Presbyterian Church in Robinson, PA");
                await writer.Write(new SyndicationLink(new Uri("https://providencepgh.org")));

                using (ISermonRepository repsository = ServiceLocater.Instance.Locate<ISermonRepository>())
                {
                    foreach (var s in repsository.LoadLastSermons(50))
                    {
                        var item = new SyndicationItem()
                        {
                            Title = s.Title,
                            Description = string.IsNullOrEmpty(s.ScriptureReference.ScriptureString) ? s.SpeakerFormalName : s.SpeakerFormalName + " - " + s.ScriptureReference.ScriptureString,
                            Published = s.RecordingDate.GetValueOrDefault().Date.AddHours(12),
                            Id = baseUrl + s.RecordingUrl.Replace("~", "")
                        };

                        item.AddLink(new SyndicationLink(new Uri(baseUrl + s.RecordingUrl.Replace("~", ""))));

                        await writer.Write(item);
                    }
                }

                xmlWriter.Flush();
            }
        }
    }
}