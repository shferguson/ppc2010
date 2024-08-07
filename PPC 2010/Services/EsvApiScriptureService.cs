﻿using System;
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
        private string Url = "http://www.esvapi.org/v2/rest/passageQuery?key={0}&passage={1}&include-footnotes=false&include-headings=false&include-subheadings=false&audio-format=mp3";

        public EsvApiScriptureService(string key)
        {
            this.Key = key;
        }

        public string GetScriptureTextHtml(ScriptureReferences scriptureReferences)
        {
            if (!scriptureReferences.HasReference)
                return string.Empty;

            var url =  string.Format(Url, Key, scriptureReferences.ScriptureString);
            string scriptureText = HttpContext.Current.Cache[url] as string;
            if (scriptureText == null)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        scriptureText = reader.ReadToEnd();

                    HttpContext.Current.Cache[url] = scriptureText;
                }
                catch (WebException ex)
                {
                    scriptureText = ex.Message;
                }
            }

            return scriptureText;
        }
    }
}