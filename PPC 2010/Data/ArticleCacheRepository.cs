using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data
{
    public class ArticleCacheRepository : IArticleRepository
    {
        private readonly IArticleRepository _repository = null;

        public ArticleCacheRepository(IArticleRepository repository)
        {
            _repository = repository;
        }

        public IArticle LoadLatestArticle()
        {
            return ArticleCache
                .OrderByDescending(a => a.Value.Date)
                .FirstOrDefault().Value;
        }

        public IArticle LoadArticle(int articleId)
        {
            return ArticleCache[articleId];
        }

        public IArticle NextArticle(int articleId)
        {
            return ArticleCache[articleId];
        }

        public IArticle PrevArticle(int articleId)
        {
            return ArticleCache[articleId];
        }

        public IEnumerable<IArticle> LoadLastArticles(int count)
        {
            return ArticleCache
                .Take(count)
                .Select(a => a.Value);
        }

        public IEnumerable<IArticle> LoadArticlesByPage(int pageNumber, int itemsPerPage)
        {
            return ArticleCache
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .Select(a => a.Value);
        }

        public IEnumerable<IArticle> LoadAllArticles()
        {
            return ArticleCache.Values;
        }

        public int GetNumberOfArticles()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        private static readonly object cacheLock = new object();
        private static Dictionary<int, IArticle> ArticleCache
        {
            get
            {
                Dictionary<int, IArticle> cache = HttpContext.Current.Application["ArticleCache"] as Dictionary<int, IArticle>;
                if (cache == null)
                    cache = new Dictionary<int, IArticle>();
                return cache;
            }
            set
            {
                HttpContext.Current.Application["ArticleCache"] = value;
            }
        }

        private IEnumerable<IArticle> GetArticles()
        {
            lock (ArticleCache)
            {
                return ArticleCache.Values.ToList();
            }
        }

        public void InvalidateCache()
        {
            lock (cacheLock)
            {
                ArticleCache = null;
            }
        }

        public void RebuildCache()
        {
            lock (cacheLock)
            {
                var Articles = _repository.LoadAllArticles().ToList();

                ArticleCache = new Dictionary<int, IArticle>();

                Articles.ForEach(s => ArticleCache.Add(s.Id, s));
            }
        }


        public void RefreshArticles()
        {
        }

        public void RefreshArticle(int articleId)
        {
        }
    }
}