using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace PPC_2010.Data.LinqToSql
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ProvidenceDbDataContext _providence;

        public ArticleRepository(ProvidenceDbDataContext providence)
        {
            _providence = providence;
        }

        public IArticle LoadLatestArticle()
        {
            return SortedArticles()
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
            return SortedArticles()
                .Where(a => a.Date > article.Date)
                .OrderBy(a => a.Date)
                .FirstOrDefault();
        }

        public IArticle PrevArticle(int articleId)
        {
            IArticle article = LoadArticle(articleId);
            return SortedArticles()
                .Where(a => a.Date < article.Date)
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
            // Filter out any future articles so that we can upload articles before we want them visible
            // and have them only show up that day
            return _providence.Articles
                .Where(a => a.Date <= DateTime.Today)
                .OrderByDescending(a => a.Date);
        }


        public void RefreshArticles()
        {
            _providence.ExecuteCommand("truncate table ppc2010.Article");
            _providence.ExecuteCommand(
                @"insert into ppc2010.Article
                 (Id, UmbracoTitle, Title, Date, Text, ScriptureReference)
                 (select Id, UmbracoTitle, Title, Date, Text, ScriptureReference from ppc2010.view_Articles where upper(UmbracoTitle) <> {0})",
                 Constants.RefreshIndicatorTitle.ToUpper()
            );
        }

        public void RefreshArticle(int articleId, bool deleted)
        {
            _providence.ExecuteCommand("delete from ppc2010.Article where Id = {0}", articleId);

            if (!deleted)
            {
                _providence.ExecuteCommand(
                    @"insert into ppc2010.Article
                 (Id, UmbracoTitle, Title, Date, Text, ScriptureReference)
                 (select Id, UmbracoTitle, Title, Date, Text, ScriptureReference from ppc2010.view_Articles where Id = {0} and upper(UmbracoTitle) <> {1})",
                 articleId,
                 Constants.RefreshIndicatorTitle.ToUpper()
                );
            }
        }
    }
}