using System.Collections.Generic;
using CvCreator.Api.Model;

namespace CvCreator.Api
{
    public interface IInMemoryStorage
    {
        void AddArticle(Article article);
        List<Article> GetArticles { get; }

    }

    public class InMemoryStorage : IInMemoryStorage
    {
        private readonly List<Article> _articles = new List<Article>();

        public void AddArticle(Article article)
        {
            _articles.Add(article);
        }

        public List<Article> GetArticles => _articles;
    }
}
