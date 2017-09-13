using System;
using System.Web.UI;
using PPC_2010.Data;
using PPC_2010.Services;
using PPC_2010.Data.Media;
using PPC_2010.Social;
using PPC_2010.Social.Facebook;

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
            DownloadImageUrl = repository.GetMeduaUrlByAliasPath("Images/DownloadSermonButton");
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
                    RecordingUrl = UrlService.MakeRelativeUrl(sermon.RecordingUrl);

                    scriptureText.Text = scriptureService.GetScriptureTextHtml(sermon.ScriptureReference);

                    SetSocialTags(sermon);
                }
            }
        }

        private void SetSocialTags(ISermon sermon)
        {
            var url = new UriBuilder(Request.Url.Scheme, Request.Url.Host, Request.Url.Port).Uri;

            var tagsService = ServiceLocator.Instance.Locate<ISocialTagsService>();
            tagsService.AddSocialTags(this, new OpenGraphTags
            {
                Type = "article",
                Section = "Sermons",
                Url = UrlService.MakeFullUrl(sermon.SermonUrl),
                Title = sermon.Title,
                Description = sermon.ScriptureReference.ToString(),
            });
        }
    }
}
