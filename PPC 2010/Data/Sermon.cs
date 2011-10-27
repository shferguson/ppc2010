using System;
using System.Linq;
using PPC_2010.Extensions;
using umbraco.cms.businesslogic.media;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Data;

namespace PPC_2010.Data
{
    public interface ISermon
    {
        int SortOrder { get; set; }
        int Id { get; }
        string Title { get; set; }
        DateTime RecordingDate { get; set; }
        string SpeakerTitle { get; set; }
        string SpeakerName { get; set; }
        string RecordingSession { get; set; }
        string SermonSeries { get; set; }
        string ScriptureReferenceText { get; set; }
        string Book { get; set; }
        int? StartChapter { get; set; }
        int? StartVerse { get; set; }
        int? EndChapter { get; set; }
        int? EndVerse { get; set; }

        ScriptureReferences ScriptureReference { get; }
        string RecordingUrl { get; }
        string SermonUrl { get; }
    }

    public abstract class Sermon : ISermon
    {
        public abstract int SortOrder { get; set; }
        public abstract int Id { get; }
        public abstract string Title { get; set; }
        public abstract DateTime RecordingDate { get; set; }
        public abstract string SpeakerTitle { get; set; }
        public abstract string SpeakerName { get; set; }
        public abstract string RecordingSession { get; set; }
        public abstract string SermonSeries { get; set; }
        public abstract string ScriptureReferenceText { get; set; }
        public abstract string Book { get; set; }
        public abstract int? StartChapter { get; set; }
        public abstract int? StartVerse { get; set; }
        public abstract int? EndChapter { get; set; }
        public abstract int? EndVerse { get; set; }

        protected abstract string GetUrl();

        private ScriptureReferences scriptureReferences = null;
        public virtual ScriptureReferences ScriptureReference { get { return BuildScriptureReferences(); } }

        public virtual string RecordingUrl 
        { 
            get {  return "~" + GetUrl(); } 
        }

        public virtual string SermonUrl
        {
            get { return "~" + "/Sermon.aspx?SermonId=" + Id; }
        }

        private ScriptureReferences BuildScriptureReferences()
        {
            if (scriptureReferences == null)
            {
                if (!string.IsNullOrWhiteSpace(ScriptureReferenceText))
                {
                    scriptureReferences = new ScriptureReferences(ScriptureReferenceText);
                }
                else
                {
                    scriptureReferences = new ScriptureReferences(Book, StartChapter, StartVerse, EndChapter, EndVerse);
                }
            }

            return scriptureReferences;
        }
    }

    // Changed all of the sermon properties so that they are lazily loaded to try to improve performance

    public class SermonFromMedia : Sermon
    {
        private Media media = null;

        public SermonFromMedia(Media media)
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
            var sermon = (SermonFromMedia)new SermonMediaRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("speakerName").GetPreValues();
        }

        public static IEnumerable<string> GetSermonSeriesList()
        {
            var sermon = (SermonFromMedia)new SermonMediaRepository().LoadLastSermons(1).First();
            return sermon.media.getProperty("sermonSeries").GetPreValues();
        }

        public static IEnumerable<string> GetRecordingSessionList()
        {
            var sermon = (SermonFromMedia)new SermonMediaRepository().LoadLastSermons(1).First();
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