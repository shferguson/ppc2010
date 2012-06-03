using System;
using System.Collections.Generic;

namespace PPC_2010.Data
{
    public interface IArticleRepository: IDisposable
    {
        IArticle LoadLatestArticle();
        IArticle LoadArticle(int articleId);
        IArticle NextArticle(int articleId);
        IArticle PrevArticle(int articleId);
        IEnumerable<IArticle> LoadLastArticles(int count);
        IEnumerable<IArticle> LoadArticlesByPage(int pageNumber, int itemsPerPage);
        IEnumerable<IArticle> LoadAllArticles();
        int GetNumberOfArticles();

        void RefreshArticles();
        void RefreshArticle(int articleId, bool deleted = false);
    }
}
