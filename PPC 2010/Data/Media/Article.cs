using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.web;

namespace PPC_2010.Data.Media
{
    public class Article : IArticle
    {
        private readonly Document _doc = null;

        public Article(Document doc)
        {
            _doc = doc;
        }

        public int SortOrder
        {
            get { return _doc.sortOrder; }
            set { _doc.sortOrder = value; }
        }

        public int Id
        {
            get { return _doc.Id; }
        }

        public string Title
        {
            get { return _doc.getProperty("title").Value as string; }
            set { _doc.getProperty("title").Value = value; }
        }

        public DateTime? Date
        {
            get { return _doc.getProperty("date").Value as DateTime?; }
            set { _doc.getProperty("date").Value = value; }
        }

        public string ScriptureReference
        {
            get { return _doc.getProperty("scriptureReference").Value as string; }
            set { _doc.getProperty("scriptureReference").Value = value; }
        }

        public string Text
        {
            get { return _doc.getProperty("text").Value as string; }
            set { _doc.getProperty("text").Value = value; }
        }
    }
}