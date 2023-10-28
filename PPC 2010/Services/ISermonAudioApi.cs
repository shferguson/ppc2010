using PPC_2010.Data;
using System.Threading.Tasks;

namespace PPC_2010.Services
{
    public interface ISermonAudioApi
    {
        Task<string> Create(ISermon sermon, string filePath);
        Task Update(string id, ISermon sermon, string filePath);
        Task Delete(string id);
    }

    public class NullSermonAudioApi : ISermonAudioApi
    {
        public Task<string> Create(ISermon sermon, string filePath)
        {
            return Task.FromResult((string)null);
        }

        public Task Delete(string id)
        {
            return Task.CompletedTask;
        }

        public Task Update(string id, ISermon sermon, string filePath)
        {
            return Task.CompletedTask;
        }
    }

}