using System.Web;

namespace PPC_2010
{
    /// <summary>
    /// Summary description for ArticleFeed
    /// </summary>
    public class ArticleFeed : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Feed removed");
            context.Response.StatusCode = 410; // Gone
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}