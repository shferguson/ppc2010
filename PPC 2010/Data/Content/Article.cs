using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.NodeFactory;

namespace PPC_2010.Data.Content
{
    using umbraco.cms.businesslogic;

    public class Article : IArticle
    {
        private readonly Content _content;

        public Article(int id)
        {
            _content = new Content(id);
        }

        public Article(Content content)
        {
            _content = content;
        }

        public int Id
        {
            get { return _content.Id; }
        }

        public string Title
        {
            get { return _content.getProperty("title").Value as string; }
            set { _content.getProperty("title").Value = value; }
        }

        public DateTime? Date
        {
            get { return _content.getProperty("date").Value as DateTime?; }
            set { _content.getProperty("date").Value = Date; }
        }

        public string ScriptureReference
        {
            get { return _content.getProperty("scriptureReference").Value as string; }
            set { _content.getProperty("scriptureReference").Value = value; }
        }

        public string Text
        {
            get { return _content.getProperty("text").Value as string; }
            set { _content.getProperty("text").Value = value; }
        }
    }
}