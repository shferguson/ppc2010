using System;
using System.Collections.Generic;
using System.Linq;
using PPC_2010.Extensions;

namespace PPC_2010.Data.Media
{
    using PPC_2010.Services;
    using Umbraco.Core.Models;

    public class MediaSermon : Sermon
    {
        private IMedia _media = null;

        public MediaSermon(IMedia media)
        {
            this._media = media;
        }

        public IMedia Media { get { return _media; } }

        public override int SortOrder
        {
            get { return _media.SortOrder; }
            set { _media.SortOrder = value; }
        }

        public override int Id
        {
            get { return _media.Id; }
        }

        private string title = null;
        public override string Title
        {
            get
            {
                if (title == null)
                    title = _media.GetValue<string>("title");
                return title;
            }
            set
            {
                title = value;
                _media.SetValue("title", title);
            }
        }

        private DateTime? recordingDate = null;
        public override DateTime? RecordingDate
        {
            get
            {
                if (recordingDate == null)
                    recordingDate = _media.GetValue<DateTime?>("recordingDate") ?? DateTime.MinValue;
                return recordingDate.Value;
            }
            set
            {
                recordingDate = value;
                _media.SetValue("recordingDate", value);
            }
        }

        private int speakerTitleId = -1;
        public override int SpeakerTitleId
        {
            get
            {
                if (speakerTitleId == -1)
                    speakerTitleId = _media.Properties["speakerTitle"].Id;
                return speakerTitleId;
            }
            set
            {
            }
        }

        private string speakerTitle = null;
        public override string SpeakerTitle
        {
            get
            {
                if (speakerTitle == null)
                    speakerTitle = _media.GetValue<string>("speakerTitle");
                return speakerTitle;
            }
            set
            {
            }
        }

        private int speakerNameId = -1;
        public override int SpeakerNameId
        {
            get
            {
                if (speakerNameId == -1)
                    speakerNameId = _media.GetValue<int>("speakerName");
                return speakerNameId;
            }
            set
            {
            }
        }

        private string speakerName = null;
        private string GetSpeakerName()
        {
            if (speakerName == null)
                speakerName = _media.GetPreValue("speakerName");
            return speakerName;

        }

        public override string SpeakerName
        {
            get
            {
                return SpeakerNameHelper.SpeakerName(GetSpeakerName());
            }
            set
            {
            }
        }

        public override string SpeakerFormalName
        {
            get
            {
                return SpeakerNameHelper.FormalName(GetSpeakerName());
            }
            set
            {
            }
        }

        private int recordingSessionId = -1;
        public override int RecordingSessionId
        {
            get
            {
                if (recordingSessionId == -1)
                    recordingSessionId = _media.GetValue<int>("recordingSession");
                return recordingSessionId;
            }
            set
            {
            }
        }

        private string recordingSession = null;
        public override string RecordingSession
        {
            get
            {
                if (_media == null)
                    throw new Exception("Media is null");

                if (recordingSession == null)
                    recordingSession = _media.GetPreValue("recordingSession");

                return recordingSession;
            }
            set
            {
            }
        }

        private int sermonSeriesId = -1;
        public override int SermonSeriesId
        {
            get
            {
                if (sermonSeriesId == -1)
                    sermonSeriesId = _media.GetValue<int>("sermonSeries");
                return sermonSeriesId;
            }
            set
            {
            }
        }

        private string sermonSeries = null;
        public override string SermonSeries
        {
            get
            {
                if (sermonSeries == null)
                    sermonSeries = _media.GetPreValue("sermonSeries");
                return sermonSeries;
            }
            set
            {
            }
        }

        private string url = null;
        protected override string GetUrl()
        {
            if (url == null)
                url = "~" + _media.GetValue<string>("audioFile");
            return url;
        }

        public override string ScriptureReferenceText
        {
            get { return _media.GetValue<string>("scriptureReferenceText"); }
            set { }
        }

        public override string Book
        {
            get { 
                // This should work, but Umbraco errors out on only this prevalue
                //return _media.GetPreValue("book"); 
                var id = _media.GetValue<int>("book");
                if (id <= 0) return null;
                var bookPreValue = ServiceLocater.Instance.Locate<IPreValueRepository>().GetById(id);
                if (bookPreValue == null) return null;
                return bookPreValue.Value;
            }
            set { }
        }


        public override int? StartChapter
        {
            get { return _media.GetValue<int?>("startChapter"); }
            set { }
        }

        public override int? StartVerse
        {
            get { return _media.GetValue<int?>("startVerse"); }
            set { }
        }

        public override int? EndChapter
        {
            get { return _media.GetValue<int?>("endChapter"); }
            set { }
        }

        public override int? EndVerse
        {
            get { return _media.GetValue<int?>("endVerse"); }
            set { }
        }


        #region Static Methods

        public static IEnumerable<string> GetSpeakerList()
        {
            var sermon = (MediaSermon)new SermonRepository().LoadLastSermons(1).First();
            return sermon._media.GetPrevaluesForProperty("speakerName");
        }

        public static IEnumerable<string> GetSermonSeriesList()
        {
            var sermon = (MediaSermon)new SermonRepository().LoadLastSermons(1).First();
            return sermon._media.GetPrevaluesForProperty("sermonSeries");
        }

        public static IEnumerable<string> GetRecordingSessionList()
        {
            var sermon = (MediaSermon)new SermonRepository().LoadLastSermons(1).First();
            return sermon._media.GetPrevaluesForProperty("recordingSession");
        }

        #endregion
    }
}



