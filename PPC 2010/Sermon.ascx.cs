using System;
using System.Web.UI;
using PPC_2010.Data;
using PPC_2010.Data.LinqToSql;
using PPC_2010.Services;
using umbraco.cms.businesslogic.media;

namespace PPC_2010
{
    public partial class Sermon : System.Web.UI.UserControl
    {
        public string SermonId { get; set; }
        public string ScriptureApiKey { get; set; }

        public string SundaySermonType { get; set; }

        protected int MediaPlayerHeight { get; set; }

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
                SetMediaPlayerHeight();
                SetDownloadImageUrl();
            }
        }

        private void SetDownloadImageUrl()
        {
            MediaRepository repository = new MediaRepository();
            Media media = repository.GetMediaByAliasPath("Images/DownloadSermonButton");

            if (media != null)
            {
                string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

                object umbracoFile = media.getProperty("umbracoFile").Value;
                if (umbracoFile != null)
                    DownloadImageUrl = umbracoFile.ToString().Replace("~", baseUrl);
            }
        }

        private void LoadSermon(int sermonId)
        {
            using (ISermonRepository repository = new LinqToSqlSermonRepository())
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
                    recordingDate.Text = sermon.RecordingDate.ToShortDateString();
                    recordingSession.Text = sermon.RecordingSession;
                    RecordingUrl = sermon.RecordingUrl.Replace("~", baseUrl);

                    scriptureText.Text = scriptureService.GetScriptureTextHtml(sermon.ScriptureReference);
                }
            }
        }

        private void SetMediaPlayerHeight()
        {
            // There is a different control on each browser
            // MSIE uses the IE ActiveX control which is 50 pixels
            // Other browsers on Windows use the Firefox media player plug in which is 45 pixels
            // On Mac, all browsers use the QuickTime control which has no header

            if (string.IsNullOrEmpty(Request.UserAgent))
            {
                sermonPlayerPanel.Visible = false;
                return;
            }

            string userAgent = Request.UserAgent.ToLower();

            if (userAgent.Contains("mobile"))
            {
                sermonPlayerPanel.Visible = false;
            }
            else if (userAgent.Contains("windows"))
            {
                if (userAgent.Contains("msie"))
                    MediaPlayerHeight = 50;
                else
                    MediaPlayerHeight = 45;
            }
            else if (userAgent.Contains("macintosh"))
            {
                MediaPlayerHeight = 16;
            }
            else
            {
                MediaPlayerHeight = 50;
            }
        }
    }
}
