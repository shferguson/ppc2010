using PPC_2010.Data;

namespace PPC_2010.Services
{
    public interface ISermonPublishApi
    {
        void Delete(int sermonId);
        void Update(ISermon sermon);
        void Dispose();
    }

    public class NullSermonPublishApi : ISermonPublishApi
    {
        public void Delete(int sermonId) { }

        public void Update(ISermon sermon) { }
        public void Dispose() { }
    }
}