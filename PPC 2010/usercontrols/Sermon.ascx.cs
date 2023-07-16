using System;
using System.Web.UI;
using PPC_2010.Data;
using PPC_2010.Services;
using PPC_2010.Data.Media;
using PPC_2010.Social;
using PPC_2010.Social.Facebook;
using PPC_2010.TimeZone;

namespace PPC_2010
{
    public partial class Sermon : System.Web.UI.UserControl
    {
        public string SermonId { get; set; }
        public string ScriptureApiKey { get; set; }

        public string SundaySermonType { get; set; }

        private IScriptureService scriptureService = null;
        
        protected string ShareUrl { get; set; }
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
            using (ISermonRepository repository = ServiceLocater.Instance.Locate<ISermonRepository>())
            {
                ISermon sermon = null;
                bool loadCurrentSermon = sermonId <= 0;
                if (loadCurrentSermon)
                    sermon = repository.LoadCurrentSermon(SundaySermonType);
                else
                    sermon = repository.LoadSermon(sermonId);

                if (sermon != null)
                {
                    string baseUrl = string.Format("{0}", (Request.ApplicationPath.Equals("/")) ? string.Empty : Request.ApplicationPath);

                    sermonTitle.Text = sermon.Title;
                    speakerName.Text = sermon.SpeakerName;
                    recordingDate.Text = sermon.RecordingDate.GetValueOrDefault().ToShortDateString();
                    recordingSession.Text = sermon.RecordingSession;
                    RecordingUrl = UrlService.MakeRelativeUrl(sermon.RecordingUrl);

                    scriptureText.Text = scriptureService.GetScriptureTextHtml(sermon.ScriptureReference);

                    SetSocialTags(sermon, loadCurrentSermon);
                }
            }
        }

        private void SetSocialTags(ISermon sermon, bool isCurrentSermonLink)
        {
            ShareUrl = UrlService.MakeFullUrl(sermon.SermonUrl);

            var tagUrl = UrlService.MakeFullUrl(Request.Path);

            var tagsService = ServiceLocater.Instance.Locate<ISocialTagsService>();
            tagsService.AddSocialTags(this, new OpenGraphTags
            {
                Type = "article",
                Section = "Sermons",
                Url = tagUrl,
                Title = sermon.Title,
                Description = sermon.ScriptureReference.ToString(),
                Date = sermon.RecordingDate.HasValue ? TimeZoneConverter.ConvertToEastern(sermon.RecordingDate.Value).AddHours(12) : (DateTime?)null,
                ExpirationDate = isCurrentSermonLink && sermon.RecordingDate.HasValue ? TimeZoneConverter.ConvertToEastern(sermon.RecordingDate.Value).AddDays(7).AddHours(16) : (DateTime?)null,
            });
        }
    }
}
