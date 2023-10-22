using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace PPC_2010.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static string ToQueryString(this NameValueCollection nvc)
        {
            var formatted =
                from key in nvc.AllKeys
                from value in nvc.GetValues(key)
                select $"{HttpUtility.UrlEncode(key)}={HttpUtility.UrlEncode(value)}";
            return "?" + string.Join("&", formatted);
        }
    }
}