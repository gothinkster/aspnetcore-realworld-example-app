using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Articles
{
    public class List
    {
        public class Query : IRequest<ArticlesEnvelope>
        {
        }

        public class QueryHandler : IAsyncRequestHandler<Query, ArticlesEnvelope>
        {
            private readonly RealWorldContext _context;

            public QueryHandler(RealWorldContext context)
            {
                _context = context;
            }

            public async Task<ArticlesEnvelope> Handle(Query message)
            {
                var articles = await _context.Articles
                    .Include(x => x.ArticleTags)
                    .Take(20).ToListAsync();
                return new ArticlesEnvelope()
                {
                    Articles = articles
                };
            }
        }
    }
}