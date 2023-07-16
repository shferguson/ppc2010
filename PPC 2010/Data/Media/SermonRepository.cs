using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data.Media
{
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    
    public class SermonRepository : ISermonRepository
    {
        public const string SermonFolderAlias = "SermonFolder";

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            IEnumerable<ISermon> sermons = LoadAllSermons();

            ISermon sermon = sermons.FirstOrDefault(s => s.RecordingSession == recordingSession);

            // Reload the sermon so we don't run into caching issues
            return LoadSermon(sermon.Id);
        }

        public ISermon LoadSermon(int sermonId)
        {
            IMedia media = ServiceLocater.Instance.Locate<IMediaService>().GetById(sermonId);
            if (media != null)
                return new MediaSermon(media);
            return null;
        }

        public IEnumerable<ISermon> LoadLastSermons(int count)
        {
            IEnumerable<ISermon> sermons = LoadAllSermons();

            return sermons.Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            var sermons = LoadAllSermons();
            return sermons.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return GetSermons();
        }

        public int GetNumberOfSermons()
        {
            return GetSermons().Count();
        }

        public void Dispose() { }

        private static IEnumerable<MediaSermon> GetSermons()
        {
            var mediaService = ServiceLocater.Instance.Locate<IMediaService>();
            var sermonRoot = mediaService.GetRootMedia().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);
            if (sermonRoot != null)
            {
                return sermonRoot.Children().Select(m => new MediaSermon(m))
                    .OrderByDescending(s => s.RecordingDate)
                    .ThenByDescending(s => s.Id)
                    .ToArray();
            }
            return Enumerable.Empty<MediaSermon>();
        }

        public void UpdateSermonSort()
        {
            var sermons = GetSermons();

            int i = 1;
            foreach (var sermon in sermons)
            {
                if (sermon.SortOrder != i)
                {
                    sermon.SortOrder = i;
                    ApplicationContext.Current.Services.MediaService.Save(sermon.Media, 0, false);
                }

                i++;
            }
        }


        public void RefreshSermons()
        {
        }

        public void RefreshSermon(int sermonId, bool deleted)
        {
        }
    }
}