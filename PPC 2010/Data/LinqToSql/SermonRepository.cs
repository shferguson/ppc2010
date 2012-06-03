using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PPC_2010.Data.LinqToSql
{
    public class SermonRepository : ISermonRepository
    {
        private readonly ProvidenceDbDataContext _providence;

        public SermonRepository(ProvidenceDbDataContext providence)
        {
            _providence = providence;
        }

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            return _providence.Sermons
                .Where(s => s.RecordingSession == recordingSession)
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Take(1)
                .FirstOrDefault();
        }

        public ISermon LoadSermon(int sermonId)
        {
            return _providence
                .Sermons
                .Single(s => ((LinqToSql.Sermon)s).Id == sermonId);
        }

        public IEnumerable<ISermon> LoadLastSermons(int count)
        {
            return _providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            return _providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return _providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession);
        }

        public int GetNumberOfSermons()
        {
            return _providence.Sermons.Count();
        }

        public void Dispose()  {  }

        public void RefreshSermons()
        {
            _providence.ExecuteCommand("truncate table ppc2010.Sermon");
            _providence.ExecuteCommand(
                @"insert into ppc2010.Sermon
                  (Id, UmbracoTitle, RecordingDate, Title, SpeakerName, RecordingSession, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile)
                  (select Id, UmbracoTitle, RecordingDate, Title, SpeakerName, RecordingSession, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile from ppc2010.view_Sermons where upper(UmbracoTitle) <> 'REFRESH')"
            );
        }

        public void RefreshSermon(int sermonId, bool deleted)
        {
            _providence.ExecuteCommand("delete from ppc2010.Sermon where Id = {0}", sermonId);

            if (!deleted)
            {
                _providence.ExecuteCommand(
                    @"insert into ppc2010.Sermon
                 (Id, UmbracoTitle, RecordingDate, Title, SpeakerName, RecordingSession, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile)
                 (select Id, UmbracoTitle, RecordingDate, Title, SpeakerName, RecordingSession, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile from ppc2010.view_Sermons where Id = {0} and upper(UmbracoTitle) <> 'REFRESH')",
                     sermonId
                );
            }
        }
    }
}