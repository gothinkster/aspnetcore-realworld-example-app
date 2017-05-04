using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using RealWorld.Infrastructure;

namespace RealWorld.Features.Articles
{
    public class List
    {
        public class Query : IRequest<ArticlesEnvelope>
        {
            public Query(string tag, string author, string favorited, int? limit, int? offset)
            {
                Tag = tag;
                Author = author;
                Favorited = favorited;
                Limit = limit;
                Offset = offset;
            }

            public string Tag { get; }
            public string Author { get; }
            public string Favorited { get; }
            public int? Limit { get; }
            public int? Offset { get; }
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
                IQueryable<Article> queryable = _context.Articles
                    .Include(x => x.ArticleTags);

                if (!string.IsNullOrWhiteSpace(message.Tag))
                {
                    var tag = await _context.ArticleTags.FirstOrDefaultAsync(x => x.TagId == message.Tag);
                    if (tag != null)
                    {
                        queryable = queryable.Where(x => x.ArticleTags.Select(y => y.TagId).Contains(tag.TagId));
                    }
                    else
                    {
                        return new ArticlesEnvelope();
                    }
                }
                if (!string.IsNullOrWhiteSpace(message.Author))
                {
                    var author = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Author);
                    if (author != null)
                    {
                        queryable = queryable.Where(x => x.Author == author);
                    }
                    else
                    {
                        return new ArticlesEnvelope();
                    }
                }

                var articles = await queryable
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20).ToListAsync();
                return new ArticlesEnvelope()
                {
                    Articles = articles
                };
            }
        }
    }
}