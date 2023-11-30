using PPC_2010.Data;
using System.Threading.Tasks;

namespace PPC_2010.Services
{
    public interface ISermonAudioApi
    {
        Task<string> Create(ISermon sermon);
        Task Update(string id, ISermon sermon);
        Task Delete(string id);

        Task UploadFile(string id, string filePath);
    }

    public class NullSermonAudioApi : ISermonAudioApi
    {
        public Task<string> Create(ISermon sermon)
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

        public Task Update(string id, ISermon sermon)
        {
            return Task.CompletedTask;
        }

        public Task UploadFile(string id, string filePath)
        {
            return Task.CompletedTask;
        }
    }
}