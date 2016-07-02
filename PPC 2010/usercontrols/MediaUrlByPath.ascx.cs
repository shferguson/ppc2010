using System;
using System.Web.UI;
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
                    mediaUrl.Text = repository.GetMeduaUrlByAliasPath(MediaPath);
                }
            }
        }
    }
}