﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data
{
    using umbraco.cms.businesslogic.media;

    public class SermonCacheRepository : ISermonRepository
    {
        private ISermonRepository _repository;

        public SermonCacheRepository(ISermonRepository repository)
        {
            _repository = repository;
        }

        public ISermon LoadCurrentSermon(string recordingSession)
        {
            return _repository.LoadCurrentSermon(recordingSession);
        }

        public ISermon LoadSermon(int sermonId)
        {
            return GetSermons()
                .Single(s => s.Id == sermonId);
        }

        public IEnumerable<ISermon> LoadLastSermons(int count)
        {
            return GetSermons()
                .Take(count);
        }

        public IEnumerable<ISermon> LoadSermonsByPage(int pageNumber, int itemsPerPage)
        {
            return GetSermons()
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public IEnumerable<ISermon> LoadAllSermons()
        {
            return GetSermons();
        }

        public int GetNumberOfSermons()
        {
            return _repository.GetNumberOfSermons();
        }
        

        public void Dispose()
        {
            _repository.Dispose();
        }

        private static readonly object cacheLock = new object();
        private static Dictionary<int, ISermon> sermonCache
        {
            get
            {
                Dictionary<int, ISermon> cache = HttpContext.Current.Application["SermonCache"] as Dictionary<int, ISermon>;
                if (cache == null)
                    cache = new Dictionary<int, ISermon>();
                return cache;
            }
            set
            {
                HttpContext.Current.Application["SermonCache"] = value;
            }
        }

        private IEnumerable<ISermon> GetSermons()
        {
            var sermonRoot = umbraco.cms.businesslogic.media.Media.GetRootMedias().FirstOrDefault(m => m != null && m.ContentType != null && m.ContentType.Alias == Data.Media.SermonMediaRepository.SermonFolderAlias);

            lock (sermonCache)
            {
                if (sermonRoot.ChildCount != sermonCache.Count)
                {
                    RefreshSermons();
                }

                return sermonCache.Values
                    .OrderByDescending(s => s.RecordingDate)
                    .ThenByDescending(s => s.RecordingSession)
                    .ToList();
            }
        }

        public void InvalidateCache()
        {
            lock (cacheLock)
            {
                sermonCache = new Dictionary<int, ISermon>();
            }
        }

        public void RefreshSermons()
        {
            lock (cacheLock)
            {
                var sermons = _repository.LoadAllSermons().ToList();

                sermonCache = new Dictionary<int, ISermon>();

                sermons.ForEach(s => sermonCache.Add(s.Id, s));
            }
        }
    }
}