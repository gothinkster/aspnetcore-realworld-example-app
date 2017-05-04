using RealWorld.Domain;

namespace RealWorld.Features.Articles
{
    public class ArticleEnvelope
    {
        public ArticleEnvelope(Article article)
        {
            Article = article;
        }

        public Article Article { get; }
    }
}