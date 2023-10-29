using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data.Media
{
    using System;
    using Umbraco.Core;
    using Umbraco.Core.Models;
    using Umbraco.Core.Services;
    
    public class SermonRepository : ISermonRepository
    {
        public const string SermonAlias = "Sermon";
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

        private static void GetSermonsRecursive(IMedia root, List<MediaSermon> sermonList)
        {
            foreach (var item in root.Children())
            {
                switch (item.ContentType?.Alias)
                {
                    case SermonAlias:
                        sermonList.Add(new MediaSermon(item)); 
                        break;
                    case SermonFolderAlias:
                        GetSermonsRecursive(item, sermonList);
                       break;
                }

            }

        }


        private static IEnumerable<MediaSermon> GetSermons()
        {
            var mediaService = ServiceLocater.Instance.Locate<IMediaService>();
            var sermonRoot = mediaService.GetRootMedia().FirstOrDefault(m => m?.ContentType?.Alias == SermonFolderAlias);
            if (sermonRoot != null)
            {
                var list = new List<MediaSermon>();
                GetSermonsRecursive(sermonRoot, list);

                return list
                    .Where(s => s.AudioFile != null)
                    .OrderBy(s => s.RecordingDate)
                    .ThenByDescending(s => s.RecordingSession)
                    .ThenByDescending(s => s.SermonSeries)
                    .ThenByDescending(s => s.Id)
                    .ToArray();
            }
            return Enumerable.Empty<MediaSermon>();
        }

        private static IEnumerable<IMedia> GetMediaUnderSermons()
        {
            var mediaService = ServiceLocater.Instance.Locate<IMediaService>();
            var sermonRoot = mediaService.GetRootMedia().FirstOrDefault(m => m?.ContentType?.Alias == SermonFolderAlias);
            return GetSortedMedia(sermonRoot);
        }

        private static IEnumerable<IMedia> GetSortedMedia(IMedia sermonRoot)
        {
            if (sermonRoot != null)
            {
                return sermonRoot.Children()
                    .OrderByDescending(m => m.ContentType?.Alias == SermonAlias ? 0 : 1)
                    .ThenByDescending(m =>
                    {
                        if (m.ContentType?.Alias == SermonAlias) return new MediaSermon(m).RecordingDate;
                        return DateTime.MinValue;
                    })
                    .ThenByDescending(m => m.Name)
                    .ThenByDescending(m => m.Id)
                    .ToArray();
            }
            return Enumerable.Empty<IMedia>();
        }

        public void RefreshSermons()
        {
        }

        public void RefreshSermon(int sermonId, bool deleted)
        {
        }

        public void UpdateSermonSort(IMedia updatedMedia)
        {
            var media = GetSortedMedia(updatedMedia.Parent());

            int i = 1;
            foreach (var item in media)
            {
                if (item.SortOrder != i)
                {
                    item.SortOrder = i;
                    ApplicationContext.Current.Services.MediaService.Save(item, userId: 0, raiseEvents: false);
                }

                i++;
            }
        }
    }
}