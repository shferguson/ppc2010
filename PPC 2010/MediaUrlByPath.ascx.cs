using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PPC_2010.Data;
using umbraco.cms.businesslogic.media;
using PPC_2010.Data.Media;

namespace PPC_2010
{
    public partial class MediaUrlByPath : System.Web.UI.UserControl
    {
        public string MediaPath { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(MediaPath))
                {
                    MediaRepository repository = new MediaRepository();
                    Media media = repository.GetMediaByAliasPath(MediaPath);

                    if (media != null)
                    {
                        string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

                        object umbracoFile = media.getProperty("umbracoFile").Value;
                        if (umbracoFile != null)
                            mediaUrl.Text = umbracoFile.ToString().Replace("~", baseUrl);
                    }
                }
            }
        }
    }
}