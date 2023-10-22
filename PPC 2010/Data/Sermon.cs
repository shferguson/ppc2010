using System;

namespace PPC_2010.Data
{
    public abstract class Sermon : ISermon
    {
        public abstract int SortOrder { get; set; }
        public abstract int Id { get; }
        public abstract string Title { get; set; }
        public abstract DateTime? RecordingDate { get; set; }
        public abstract int SpeakerTitleId { get; set; }
        public abstract string SpeakerTitle { get; set; }
        public abstract int SpeakerNameId { get; set; }
        public abstract string SpeakerName { get; set; }
        public abstract string SpeakerFormalName { get; set; }
        public abstract int RecordingSessionId { get; set; }
        public abstract string RecordingSession { get; set; }
        public abstract int SermonSeriesId { get; set; }
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

    public class PodSermon : ISermon
    {
        public static PodSermon Clone(ISermon sermon)
        {
            return new PodSermon
            {
                SortOrder = sermon.SortOrder,
                Id = sermon.Id,
                Title = sermon.Title,
                RecordingDate = sermon.RecordingDate,
                SpeakerTitleId = sermon.SpeakerTitleId,
                SpeakerTitle = sermon.SpeakerTitle,
                SpeakerNameId = sermon.SpeakerNameId,
                SpeakerName = sermon.SpeakerName,
                SpeakerFormalName = sermon.SpeakerFormalName,
                RecordingSessionId = sermon.RecordingSessionId,
                RecordingSession = sermon.RecordingSession,
                SermonSeriesId = sermon.SermonSeriesId,
                SermonSeries = sermon.SermonSeries,
                ScriptureReferenceText = sermon.ScriptureReferenceText,
                Book = sermon.Book,
                StartChapter = sermon.StartChapter,
                StartVerse = sermon.StartVerse,
                EndChapter = sermon.EndChapter,
                EndVerse = sermon.EndVerse,
                ScriptureReference = sermon.ScriptureReference,
                RecordingUrl = sermon.RecordingUrl,
                SermonUrl = sermon.SermonUrl,
            };
        }

        public int SortOrder { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime? RecordingDate { get; set; }
        public int SpeakerTitleId { get; set; }
        public string SpeakerTitle { get; set; }
        public int SpeakerNameId { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerFormalName { get; set; }
        public int RecordingSessionId { get; set; }
        public string RecordingSession { get; set; }
        public int SermonSeriesId { get; set; }
        public string SermonSeries { get; set; }
        public string ScriptureReferenceText { get; set; }
        public string Book { get; set; }
        public int? StartChapter { get; set; }
        public int? StartVerse { get; set; }
        public int? EndChapter { get; set; }
        public int? EndVerse { get; set; }
        public ScriptureReferences ScriptureReference { get; set; }
        public string RecordingUrl { get; set;}
        public string SermonUrl { get; set; }
    }
}