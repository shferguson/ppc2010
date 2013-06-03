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
}