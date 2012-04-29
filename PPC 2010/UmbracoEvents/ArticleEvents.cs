using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.web;
using umbraco.cms.businesslogic;

namespace PPC_2010.UmbracoEvents
{
    public class ArticleEvents
    {
        static ArticleEvents()
        {
            Content.AfterNew += new EventHandler<NewEventArgs>(Content_AfterNew);
            Content.BeforeSave += new EventHandler<SaveEventArgs>(Content_BeforeSave);
            Content.AfterSave += new EventHandler<SaveEventArgs>(Content_AfterSave);
        }

        static void Content_AfterSave(object sender, SaveEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void Content_AfterNew(object sender, NewEventArgs e)
        {
            throw new NotImplementedException();
        }

        static void Content_BeforeSave(object sender, SaveEventArgs e)
        {
            throw new NotImplementedException();
        }
        
    }
}