using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PPC_2010.Data.LinqToSql
{
    public class LinqToSqlArticleRepository : IArticleRepository
    {
        private readonly ProvidenceDbDataContext _providence;

        public LinqToSqlArticleRepository(ProvidenceDbDataContext providence)
        {
            _providence = providence;
        }

        public IArticle LoadLatestArticle()
        {
            return _providence.Articles
                .OrderByDescending(a => a.Date)
                .FirstOrDefault();
        }

        public IArticle LoadArticle(int articleId)
        {
            return _providence.Articles
                .Where(a => a.Id == articleId)
                .FirstOrDefault();
        }

        public IArticle NextArticle(int articleId)
        {
            IArticle article = LoadArticle(articleId);
            return _providence.Articles
                .Where(a => a.Date > article.Date)
                .OrderBy(a => a.Date)
                .FirstOrDefault();
        }

        public IArticle PrevArticle(int articleId)
        {
            IArticle article = LoadArticle(articleId);
            return _providence.Articles
                .Where(a => a.Date < article.Date)
                .OrderByDescending(a => a.Date)
                .FirstOrDefault();
        }

        public IEnumerable<IArticle> LoadLastArticles(int count)
        {
            return SortedArticles()
                .Take(count);
        }

        public IEnumerable<IArticle> LoadArticlesByPage(int pageNumber, int itemsPerPage)
        {
            return SortedArticles()
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage);
        }

        public IEnumerable<IArticle> LoadAllArticles()
        {
            return SortedArticles();
        }

        public int GetNumberOfArticles()
        {
            return _providence.Articles.Count();
        }

        public void Dispose() { }

        private IEnumerable<Article> SortedArticles()
        {
            return _providence.Articles
                .OrderByDescending(a => a.Date);
        }
    }
}