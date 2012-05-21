using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data.Media
{
    using umbraco.cms.businesslogic.media;

    public class SermonRepository : ISermonRepository
    {
        public const string SermonFolderAlias = "SermonFolder";
        public const string SermonAlias = "Sermon";

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            IEnumerable<ISermon> sermons = LoadAllSermons();

            ISermon sermon = sermons.FirstOrDefault(s => s.RecordingSession == recordingSession);

            // Reload the sermon so we don't run into caching issues
            return LoadSermon(sermon.Id);
        }

        public ISermon LoadSermon(int sermonId)
        {
            Media media = new Media(sermonId);
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
            Media sermonRoot = Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);
            if (sermonRoot != null)
            {
                return sermonRoot.Children.Select(m => new MediaSermon(m))
                    .OrderByDescending(s => s.RecordingDate)
                    .ThenByDescending(s => s.RecordingSession)
                    .ToArray();
            }
            return Enumerable.Empty<MediaSermon>();
        }


        public void RefreshSermons()
        {
        }
    }
}