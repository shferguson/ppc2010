using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PPC_2010.Data.LinqToSql
{
    public class LinqToSqlSermonRepository : ISermonRepository
    {
        ProvidenceDbDataContext providence = new ProvidenceDbDataContext(ConfigurationManager.AppSettings["umbracoDbDSN"]);

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            return providence.Sermons
                .Where(s => s.RecordingSession == recordingSession)
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Take(1)
                .FirstOrDefault();
        }

        public ISermon LoadSermon(int sermonId)
        {
            return providence
                .Sermons
                .Single(s => ((LinqToSql.Sermon)s).Id == sermonId);
        }

        public IEnumerable<ISermon> LoadLastSermons(int count)
        {
            return providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            return providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return providence.Sermons
                .OrderByDescending(s => s.RecordingDate)
                .ThenByDescending(s => s.RecordingSession);
        }

        public int GetNumberOfSermons()
        {
            return providence.Sermons.Count();
        }

        public void Dispose()
        {
            providence.Dispose();
        }
    }
}