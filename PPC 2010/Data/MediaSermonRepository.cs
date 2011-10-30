using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.media;

namespace PPC_2010.Data
{
    public class SermonMediaRepository : ISermonRepository
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
                return MediaSermon(media);
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

        private static readonly object cacheLock = new object();
        private static Dictionary<int, MediaSermon> sermonCache
        {
            get
            {
                Dictionary<int, MediaSermon> cache = HttpContext.Current.Application["SermonCache"] as Dictionary<int, MediaSermon>;
                if (cache == null)
                    cache = new Dictionary<int, MediaSermon>();
                return cache;
            }
            set
            {
                HttpContext.Current.Application["SermonCache"] = value;
            }
        }

        private static IEnumerable<MediaSermon> GetSermons()
        {
            Media sermonRoot = Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);

            lock (sermonCache)
            {
                if (sermonRoot.ChildCount != sermonCache.Count)
                {
                    RebuildCache();
                }

                return sermonCache.Values.AsEnumerable<MediaSermon>()
                    .OrderByDescending(s => s.RecordingDate)
                    .ThenByDescending(s => s.RecordingSession)
                    .ToList();
            }
        }

        public static void UpdateCache(Media media)
        {
            lock (cacheLock)
            {
                if (media.ContentType.Alias == SermonAlias)
                    sermonCache[media.Id] = new MediaSermon(media);
            }
        }

        public static void InvalidateCache()
        {
            lock (cacheLock)
            {
                sermonCache = new Dictionary<int, MediaSermon>();
            }
        }

        public static void RebuildCache()
        {
            lock (cacheLock)
            {
                InvalidateCache();
                Media sermonRoot = Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == SermonFolderAlias);
                if (sermonRoot != null)
                    sermonRoot.Children.Select(m => MediaSermon(m)).ToArray();
            }
        }


        private static MediaSermon MediaSermon(Media media)
        {
            MediaSermon sermon = null;
            lock (cacheLock)
            {
                if (sermonCache.ContainsKey(media.Id))
                {
                    sermon = sermonCache[media.Id];
                }
                else
                {
                    sermon = new MediaSermon(media);
                    sermonCache[media.Id] = sermon;
                }
            }

            return sermon;
        }

        public static void OrderSermons()
        {
            IEnumerable<ISermon> sermons = GetSermons();

            int i = 1;
            foreach (var sermon in sermons)
            {
                if (sermon.SortOrder != i)
                {
                    // sermon.SortOrder = i;
                }

                i++;
            }
        }
    }
}