using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.web;
using Umbraco.Core.Models;

namespace PPC_2010.Data.Media
{
    public class Article : IArticle
    {
        private readonly IContent _content = null;

        public Article(IContent doc)
        {
            _content = doc;
        }

        public int SortOrder
        {
            get { return _content.SortOrder; }
            set { _content.SortOrder = value; }
        }

        public int Id
        {
            get { return _content.Id; }
        }

        public string Title
        {
            get { return _content.GetValue<string>("title"); }
            set { _content.SetValue("title", value); }
        }

        public DateTime? Date
        {
            get { return _content.GetValue<DateTime?>("date"); }
            set { _content.SetValue("date", value); }
        }

        public string ScriptureReference
        {
            get { return _content.GetValue<string>("scriptureReference"); }
            set { _content.SetValue("scriptureReference", value); }
        }

        public string Text
        {
            get { return _content.GetValue<string>("text"); }
            set { _content.SetValue("text", value); }
        }
    }
}

