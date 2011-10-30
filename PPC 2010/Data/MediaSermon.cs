using System;
using System.Collections.Generic;
using System.Linq;
using PPC_2010.Extensions;
using umbraco.cms.businesslogic.media;

namespace PPC_2010.Data
{
    public class MediaSermon : Sermon
    {
        private Media media = null;

        public MediaSermon(Media media)
        {
            this.media = media;
        }

        public override int SortOrder
        {
            get { return media.sortOrder; }
            set { media.sortOrder = value; }
        }

        public override int Id
        {
            get { return media.Id; }
        }

        private string title = null;
        public override string Title
        {
            get
            {
                if (title == null)
                    title = media.getProperty("title").Value as string;
                return title;
            }
            set
            {
                title = value;
                media.getProperty("title").Value = title;
            }
        }

        private DateTime? recordingDate = null;
        public override DateTime RecordingDate
        {
            get
            {
                if (recordingDate == null)
                    recordingDate = media.getProperty("recordingDate").Value as DateTime? ?? DateTime.MinValue;
                return recordingDate.Value;
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
                    speakerTitle = media.getProperty("speakerTitle").GetPreValueAsString();
                return speakerTitle;
            }
            set
            {
            }
        }

        private string speakerName = null;
        public override string SpeakerName
        {
            get
            {
                if (speakerName == null)
                    speakerName = media.getProperty("speakerName").GetPreValueAsString();
                return speakerName;
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
                if (recordingSession == null)
                    recordingSession = media.getProperty("recordingSession").GetPreValueAsString();

                return recordingSession;
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
                    sermonSeries = media.getProperty("sermonSeries").GetPreValueAsString();
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
                url = "~" + media.getProperty("audioFile").Value as string;
            return url;
        }

        public override string ScriptureReferenceText
        {
            get { return media.getProperty("scriptureReferenceText").Value as string; }
            set { }
        }

        public override string Book
        {
            get { return media.getProperty("book").GetPreValueAsString(); }
            set { }
        }


        public override int? StartChapter
        {
            get { return media.getProperty("startChapter").Value as int?; }
            set { }
        }

        public override int? StartVerse
        {
            get { return media.getProperty("startVerse").Value as int?; }
            set { }
        }

        public override int? EndChapter
        {
            get { return media.getProperty("endChapter").Value as int?; }
            set { }
        }

        public override int? EndVerse
        {
            get { return media.getProperty("endVerse").Value as int?; }
            set { }
        }


        #region Static Methods

        public static IEnumerable<string> GetSpeakerList()
        {
            var sermon = (MediaSermon)new SermonMediaRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("speakerName").GetPreValues();
        }

        public static IEnumerable<string> GetSermonSeriesList()
        {
            var sermon = (MediaSermon)new SermonMediaRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("sermonSeries").GetPreValues();
        }

        public static IEnumerable<string> GetRecordingSessionList()
        {
            var sermon = (MediaSermon)new SermonMediaRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("recordingSession").GetPreValues();
        }

        #endregion
    }
}