using System;

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
}
