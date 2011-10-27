using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using PPC_2010.Data;
using System.IO;

namespace PPC_2010.Services
{
    /// <summary>
    /// Uses the ESV api to get scripture text from esv.org - the key will be removed expire in a year
    /// </summary>
    public class EsvApiScriptureService : IScriptureService
    {
        private readonly string Key = "";
        private string Url = "http://www.esvapi.org/v2/rest/passageQuery?key={0}&passage={1}&include-footnotes=false&include-headings=false&include-subheadings=false";

        public EsvApiScriptureService(string key)
        {
            this.Key = key;
        }

        public string GetScriptureTextHtml(ScriptureReferences scriptureReferences)
        {
            if (!scriptureReferences.HasReference)
                return string.Empty;

            string scriptureText = HttpContext.Current.Cache[BuildCacheKey(scriptureReferences)] as string;
            if (scriptureText == null)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        string.Format(Url, Key, scriptureReferences.ScriptureString));
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        scriptureText = reader.ReadToEnd();

                    HttpContext.Current.Cache[BuildCacheKey(scriptureReferences)] = scriptureText;
                }
                catch (WebException ex)
                {
                    scriptureText = ex.Message;
                }
            }

            return scriptureText;
        }

        private string BuildCacheKey(ScriptureReferences scriptureReferences)
        {
            return "ESV - " + scriptureReferences.ScriptureString;
        }
    }
}