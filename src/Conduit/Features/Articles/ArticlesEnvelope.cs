using System.Collections.Generic;
using Conduit.Domain;

namespace Conduit.Features.Articles
{
    public class ArticlesEnvelope
    {
        public List<Article> Articles { get; set; } = new();

        public int ArticlesCount { get; set; }
    }
}
