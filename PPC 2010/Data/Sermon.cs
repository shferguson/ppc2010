using System;
using System.Linq;
using PPC_2010.Extensions;
using umbraco.cms.businesslogic.media;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

namespace PPC_2010.Data
{
    // Changed all of the sermon properties so that they are lazily loaded to try to improve performance

    public class Sermon
    {
        private Media media = null;

        public Sermon(Media media)
        {
            this.media = media;
        }

        public int SortOrder
        {
            get { return media.sortOrder; }
            set { media.sortOrder = value; }
        }

        public int Id
        {
            get { return media.Id; }
        }

        private string title = null;
        public string Title {
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
        public DateTime RecordingDate
        {
            get
            {
                if (recordingDate == null)
                    recordingDate = media.getProperty("recordingDate").Value as DateTime? ?? DateTime.MinValue;
                return recordingDate.Value;
            }
        }

        private string speakerTitle = null;
        public string SpeakerTitle
        {
            get
            {
                if (speakerTitle == null)
                    speakerTitle = media.getProperty("speakerTitle").GetPreValueAsString();
                return speakerTitle;
            }
        }

        private string speakerName = null;
        public string SpeakerName
        {
            get
            {
                if (speakerName == null)
                    speakerName = media.getProperty("speakerName").GetPreValueAsString();
                return speakerName;
            }
        }

        private string recordingSession = null;
        public string RecordingSession
        {
            get
            {
                if (recordingSession == null)
                    recordingSession = media.getProperty("recordingSession").GetPreValueAsString();

                return recordingSession;
            }
        }

        private string sermonSeries = null;
        public string SermonSeries
        {
            get
            {
                if (sermonSeries == null)
                    sermonSeries = media.getProperty("sermonSeries").GetPreValueAsString();
                return sermonSeries;
            }
        }
  
        private ScriptureReferences scriptureReferences = null;
        public ScriptureReferences ScriptureReference
        {
            get
            {
                if (scriptureReferences == null)
                    scriptureReferences = BuildScriptureReferences();
                return scriptureReferences;
            }
        }

        private string recordingUrl = null;
        public string RecordingUrl
        {
            get
            {
                if (recordingUrl == null)
                    recordingUrl = "~" + media.getProperty("audioFile").Value as string;
                return recordingUrl;
            }
        }

        public string SermonUrl
        {
            get
            {
                return "~" + "/Sermon.aspx?SermonId=" + Id;
            }
        }


        public string ScriptureReferenceText
        {
            get { return ScriptureReference.ScriptureString; }
        }

        private ScriptureReferences BuildScriptureReferences()
        {
            string scriptureReferenceText = media.getProperty("scriptureReferenceText").Value as string;
            if (!string.IsNullOrWhiteSpace(scriptureReferenceText))
            {
                scriptureReferences = new ScriptureReferences(scriptureReferenceText);
            }
            else
            {
                int startChapter = media.getProperty("startChapter").Value as int? ?? 1;
                int startVerse = media.getProperty("startVerse").Value as int? ?? 1;
                int? endChapter = media.getProperty("endChapter").Value as int?;
                int? endVerse = media.getProperty("endVerse").Value as int?;

                if (!endChapter.HasValue)
                    endChapter = startChapter;
                if (!endVerse.HasValue)
                    endVerse = startVerse;

                string book = media.getProperty("book").GetPreValueAsString();
                scriptureReferences = new ScriptureReferences(book, startChapter, startVerse, endChapter.Value, endVerse.Value);

            }

            return scriptureReferences;
        }

        #region Static Methods

        public static IEnumerable<string> GetSpeakerList()
        {
            var sermon = new SermonRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("speakerName").GetPreValues();
        }

        public static IEnumerable<string> GetSermonSeriesList()
        {
            var sermon = new SermonRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("sermonSeries").GetPreValues();
        }

        public static IEnumerable<string> GetRecordingSessionList()
        {
            var sermon = new SermonRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("recordingSession").GetPreValues();
        }

        #endregion
    }


    #region Old Sermon Logic
#if False
    public class Sermon
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime RecordingDate { get; set; }
        public string SpeakerTitle { get; set; }
        public string SpeakerName { get; set; }
        public string RecordingSession { get; set; }
        public string SermonSeries { get; set; }
        public ScriptureReferences ScriptureReference { get; set; }
        public string RecordingUrl { get; set; }
        public string SermonUrl { get; set; }

        public Guid Version { get; set; }

        public string ScriptureReferenceText
        {
            get { return ScriptureReference.ScriptureString; }
        }

        public static Sermon FromMedia(Media media) 
        {
            Sermon s = new Sermon();

            s.Id = media.Id;
            s.Title = media.getProperty("title").Value as string;
            s.RecordingDate = media.getProperty("recordingDate").Value as DateTime? ?? DateTime.MinValue;
            s.SpeakerTitle = media.getProperty("speakerTitle").GetPreValue<string>();
            s.SpeakerName = media.getProperty("speakerName").GetPreValue<string>();
            s.RecordingSession = media.getProperty("recordingSession").GetPreValue<string>();
            s.SermonSeries = media.getProperty("sermonSeries").GetPreValue<string>();
            s.RecordingUrl = "~" + media.getProperty("audioFile").Value as string;
            s.SermonUrl = "~" + "/Sermon.aspx?SermonId=" + s.Id;

            string scriptureReferenceText = media.getProperty("scriptureReferenceText").Value as string;
            if (!string.IsNullOrWhiteSpace(scriptureReferenceText))
            {
                s.ScriptureReference = new ScriptureReferences(scriptureReferenceText);
            }
            else
            {
                int startChapter = media.getProperty("startChapter").Value as int? ?? 1;
                int startVerse = media.getProperty("startVerse").Value as int? ?? 1;
                int? endChapter = media.getProperty("endChapter").Value as int?;
                int? endVerse = media.getProperty("endVerse").Value as int?;

                if (!endChapter.HasValue)
                    endChapter = startChapter;
                if (!endVerse.HasValue)
                    endVerse = startVerse;

                string book = media.getProperty("book").GetPreValue<string>();
                s.ScriptureReference = new ScriptureReferences(book, startChapter, startVerse, endChapter.Value, endVerse.Value);

                s.Version = media.Version;
            }

            return s;
        }
    }
#endif
    #endregion
}