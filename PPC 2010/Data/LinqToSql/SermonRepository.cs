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
            _providence.RefreshSermons();
        }
    }
}