using System;
using System.Web;

namespace PPC_2010.Services
{
    static class UrlService
    {
        public static string MakeRelativeUrl(string url)
        {
            url = CleanUrl(url);
            var requestPath = HttpContext.Current.Request.ApplicationPath;
            if (requestPath != "/")
                return new Uri(new Uri(requestPath, UriKind.Relative), url).ToString();
            return new Uri(url, UriKind.Relative).ToString();
        }

        public static string MakeFullUrl(string url)
        {
            url = CleanUrl(url);
            var uri = new Uri(GetSiteUri(), url);
            return uri.ToString();
        }

        private static Uri GetSiteUri()
        {
            var requestUrl = HttpContext.Current.Request.Url;
            return new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port, HttpContext.Current.Request.ApplicationPath).Uri;
        }

        private static string CleanUrl(string url)
        {
            return url.Replace("~", "");
        }
    }
}
