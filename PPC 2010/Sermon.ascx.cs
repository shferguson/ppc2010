using System;
using System.Web.UI;
using PPC_2010.Data;
using PPC_2010.Data.LinqToSql;
using PPC_2010.Services;
using umbraco.cms.businesslogic.media;
using PPC_2010.Data.Media;

namespace PPC_2010
{
    public partial class Sermon : System.Web.UI.UserControl
    {
        public string SermonId { get; set; }
        public string ScriptureApiKey { get; set; }

        public string SundaySermonType { get; set; }

        private IScriptureService scriptureService = null;
        
        protected string RecordingUrl { get; set; }
        protected string DownloadImageUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                scriptureService = new EsvApiScriptureService(ScriptureApiKey);

                int sermonId = 0;
                if (!int.TryParse(SermonId, out sermonId))
                    sermonId = -1;

                LoadSermon(sermonId);
                SetDownloadImageUrl();
            }
        }

        private void SetDownloadImageUrl()
        {
            MediaRepository repository = new MediaRepository();
            var media = repository.GetMediaByAliasPath("Images/DownloadSermonButton");

            if (media != null)
            {
                string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

                var umbracoFile = media.GetValue<string>("umbracoFile");
                if (umbracoFile != null)
                    DownloadImageUrl = umbracoFile.Replace("~", baseUrl);
            }
        }

        private void LoadSermon(int sermonId)
        {
            using (ISermonRepository repository = ServiceLocator.Instance.Locate<ISermonRepository>())
            {
                ISermon sermon = null;
                if (sermonId > 0)
                    sermon = repository.LoadSermon(sermonId);
                else
                    sermon = repository.LoadCurrentSermon(SundaySermonType);

                if (sermon != null)
                {
                    string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

                    sermonTitle.Text = sermon.Title;
                    speakerName.Text = sermon.SpeakerName;
                    recordingDate.Text = sermon.RecordingDate.GetValueOrDefault().ToShortDateString();
                    recordingSession.Text = sermon.RecordingSession;
                    RecordingUrl = sermon.RecordingUrl.Replace("~", baseUrl);

                    scriptureText.Text = scriptureService.GetScriptureTextHtml(sermon.ScriptureReference);
                }
            }
        }
    }
}
