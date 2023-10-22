using Newtonsoft.Json;
using TagLib.Riff;

namespace PPC_2010.Services.SermonAudio
{
    public class Sermon
    {
        [JsonProperty("BroadcasterID")]
        public string BroadcasterId;
        public bool AcceptCopyright;
        public string FullTitle; 
        public string SpeakerName;
        public long PublishTimestamp;
        public string DisplayTitle;
        public string Subtitle;
        public bool NewsInFocus;
        public string PreachDate;
        public string EventType;
        public string BibleText;
        public string MoreInfoText;
        public string LanguageCode;
        public string Keywords;
        [JsonProperty("series_id")]
        public int SeriesId;
    }

    public class PatchSermon
    {
        public bool PublishNow;
        public int PublishTimestamp;
        public string Subtitle;
        [JsonProperty("series_id")]
        public int SeriesId;
        public string PublishDate;
    }

    public class SocialSharing
    {
        public bool Twitter;
        public bool Facebook;
        public bool Google;
    }
}