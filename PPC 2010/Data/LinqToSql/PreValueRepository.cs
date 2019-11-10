using System.Collections.Generic;
using System.Linq;

namespace PPC_2010.Data.LinqToSql
{
    class PreValueRepository : IPreValueRepository
    {
        private readonly ProvidenceDbDataContext _providence;

        private const string BibleBooksName = "Bible Books";
        private const string SpeakersName = "Speaker Name";
        private const string SessionsName = "Recording Session";
        private const string SermonSeriesName = "Sermon Series";

        public PreValueRepository(ProvidenceDbDataContext providence)
        {
            _providence = providence;
        }

        public IEnumerable<IPreValue> BibleBooks()
        {
            return GetPreValues(BibleBooksName);
        }

        public IEnumerable<IPreValue> Speakers()
        {
            return GetPreValues(SpeakersName);
        }

        public IEnumerable<IPreValue> Sessions()
        {
            return GetPreValues(SessionsName);
        }

        public IEnumerable<IPreValue> SermonSeries()
        {
            return GetPreValues(SermonSeriesName);
        }

        private IEnumerable<IPreValue> GetPreValues(string name)
        {
            return _providence.PreValues
                .Where(v => v.Name == name)
                .OrderBy(v => v.SortOrder);
        }

        public IPreValue GetById(int id)
        {
            return _providence.PreValues.FirstOrDefault(x => x.Id == id);
        }
    }
}