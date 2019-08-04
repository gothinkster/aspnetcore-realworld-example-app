using Conduit.Domain;
using System.Collections.Generic;

namespace Conduit.Features.Articles
{
    public class ArticlesEnvelope
    {
        public List<Article> Articles { get; set; }

        public int ArticlesCount { get; set; }
    }
}
