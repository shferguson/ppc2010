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
                .SortSermons()
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
                .SortSermons()
                .Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            return _providence.Sermons
                .SortSermons()
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return _providence.Sermons
                .SortSermons();
        }

        public int GetNumberOfSermons()
        {
            return _providence.Sermons.Count();
        }

        public void Dispose() { }

        public void RefreshSermons()
        {
            _providence.ExecuteCommand("truncate table ppc2010.Sermon");
            _providence.ExecuteCommand(
                @"insert into ppc2010.Sermon
                 (Id, UmbracoTitle, RecordingDate, Title, SpeakerNameId, SpeakerName, RecordingSessionId, RecordingSession, SermonSeriesId, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile)
                 (select Id, UmbracoTitle, RecordingDate, Title, SpeakerNameId, SpeakerName, RecordingSessionId, RecordingSession, SermonSeriesId, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile
                  from ppc2010.view_Sermons where upper(UmbracoTitle) <> {0} and SpeakerName is not null)",
                  Constants.RefreshIndicatorTitle.ToUpper()
            );
        }

        public void RefreshSermon(int sermonId, bool deleted)
        {
            _providence.ExecuteCommand("delete from ppc2010.Sermon where Id = {0}", sermonId);

            if (!deleted)
            {
                _providence.ExecuteCommand(
                    @"insert into ppc2010.Sermon
                 (Id, UmbracoTitle, RecordingDate, Title, SpeakerNameId, SpeakerName, RecordingSessionId, RecordingSession, SermonSeriesId, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile)
                 (select Id, UmbracoTitle, RecordingDate, Title, SpeakerNameId, SpeakerName, RecordingSessionId, RecordingSession, SermonSeriesId, SermonSeries, Book, StartChapter, StartVerse, EndChapter, EndVerse, ScriptureReferenceText, AudioFile from ppc2010.view_Sermons
                  where Id = {0} and upper(UmbracoTitle) <> {1})",
                     sermonId,
                     Constants.RefreshIndicatorTitle.ToUpper()
                );
            }
        }

        public void UpdateSermonSort()
        {
            _providence.ExecuteCommand(
                @"update n
                  set n.sortOrder = s.RowNumber
                  from umbracoNode n
                  inner join (select Id, ROW_NUMBER() OVER(order by RecordingDate desc, Id desc) as RowNumber from ppc2010.Sermon) s on s.Id = n.id");
        }
    }

    public static class SermonSort
    {
        public static IQueryable<Sermon> SortSermons(this IQueryable<Sermon> sermons)
        {
            return sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.Id);
        }
    }
}