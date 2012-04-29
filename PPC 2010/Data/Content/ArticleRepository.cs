using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPC_2010.Data.Content
{
    using umbraco.cms.businesslogic;

    public class ArticleRepository
    {
        public const string ArticleContainer = "Articles";
        public const string ArticleAlias = "Article";

        public IArticle LoadCurrentArticle(string recordingSession)
        {
            IEnumerable<IArticle> Articles = LoadAllArticles();

            IArticle Article = Articles.FirstOrDefault(s => s.RecordingSession == recordingSession);

            // Reload the Article so we don't run into caching issues
            return LoadArticle(Article.Id);
        }

        public IArticle LoadArticle(int articleId)
        {
            Content content = new Content(articleId);
            if (content != null)
                return new Article(content);
            return null;
        }

        public IEnumerable<IArticle> LoadLastArticles(int count)
        {
            IEnumerable<IArticle> Articles = LoadAllArticles();

            return Articles.Take(count);
        }

        public IEnumerable<IArticle> LoadArticlesByPage(int pageNumber, int itemsPerPage)
        {
            var Articles = GetArticles();
            return Articles.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage);
        }

        public IEnumerable<IArticle> LoadAllArticles()
        {
            return GetArticles();
        }

        public int GetNumberOfArticles()
        {
            return GetArticles().Count();
        }

        public void Dispose() { }

        private static IEnumerable<IArticle> GetArticles()
        {
            var articles = Content.getContentOfContentType(ContentType.GetByAlias(ArticleAlias))
                .Select(a => new Article(a))
                .OrderByDescending(a => a.Date);

            return articles.ToArray();
        }
    }
}