using System.Collections.Generic;
using System.Linq;

namespace PPC_2010.Data.LinqToSql
{
    class PrevalueRepository : IPrevalueRepository
    {
        private readonly ProvidenceDbDataContext _providence;

        private const string BibleBooksName = "Bible Books";
        private const string SpeakersName = "Speaker Name";
        private const string SessionsName = "Recording Session";
        private const string SermonSeriesName = "Sermon Series";

        public PrevalueRepository(ProvidenceDbDataContext providence)
        {
            _providence = providence;
        }

        public IEnumerable<IPrevalue> BibleBooks()
        {
            return GetPrevalues(BibleBooksName);
        }

        public IEnumerable<IPrevalue> Speakers()
        {
            return GetPrevalues(SpeakersName);
        }

        public IEnumerable<IPrevalue> Sessions()
        {
            return GetPrevalues(SessionsName);
        }

        public IEnumerable<IPrevalue> SermonSeries()
        {
            return GetPrevalues(SermonSeriesName);
        }

        private IEnumerable<IPrevalue> GetPrevalues(string name)
        {
            return _providence.Prevalues
                .Where(v => v.Name == name)
                .OrderBy(v => v.SortOrder);
        }
    }
}