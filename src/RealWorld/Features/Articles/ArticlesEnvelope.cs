using System.Collections.Generic;
using RealWorld.Domain;

namespace RealWorld.Features.Articles
{
    public class ArticlesEnvelope
    {
        public List<Article> Articles { get; set; }

        public int ArticlesCount => Articles?.Count ?? 0;
    }

    public class ArticleEnvelope
    {
        public Article Article { get; set; }
    }
}