using System.Collections.Generic;

namespace PPC_2010.Data
{
    public interface IPreValueRepository
    {

        IPreValue GetById(int id);
        IEnumerable<IPreValue> BibleBooks();
        IEnumerable<IPreValue> Speakers();
        IEnumerable<IPreValue> Sessions();
        IEnumerable<IPreValue> SermonSeries();
    }
}
