using System.Collections.Generic;

namespace PPC_2010.Data
{
    public interface IPrevalueRepository
    {
        IEnumerable<IPrevalue> BibleBooks();
        IEnumerable<IPrevalue> Speakers();
        IEnumerable<IPrevalue> Sessions();
        IEnumerable<IPrevalue> SermonSeries();
    }
}
