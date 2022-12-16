using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles
{
    public class List
    {
        public record Query(string Tag, string Author, string FavoritedUsername, int? Limit, int? Offset, bool IsFeed = false) : IRequest<ArticlesEnvelope>;

        public class QueryHandler : IRequestHandler<Query, ArticlesEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public QueryHandler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<ArticlesEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<Article> queryable = _context.Articles.GetAllData();

                if (message.IsFeed && _currentUserAccessor.GetCurrentUsername() != null)
                {
                    var currentUser = await _context.Persons.Include(x => x.Following).FirstOrDefaultAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);
                    queryable = queryable.Where(x => currentUser.Following.Select(y => y.TargetId).Contains(x.Author!.PersonId));
                }

                if (!string.IsNullOrWhiteSpace(message.Tag))
                {
                    var tag = await _context.ArticleTags.FirstOrDefaultAsync(x => x.TagId == message.Tag, cancellationToken);
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
                    var author = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Author, cancellationToken);
                    if (author != null)
                    {
                        queryable = queryable.Where(x => x.Author == author);
                    }
                    else
                    {
                        return new ArticlesEnvelope();
                    }
                }

                if (!string.IsNullOrWhiteSpace(message.FavoritedUsername))
                {
                    var author = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.FavoritedUsername, cancellationToken);
                    if (author != null)
                    {
                        queryable = queryable.Where(x => x.ArticleFavorites.Any(y => y.PersonId == author.PersonId));
                    }
                    else
                    {
                        return new ArticlesEnvelope();
                    }
                }

                var articles = await queryable
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new ArticlesEnvelope()
                {
                    Articles = articles,
                    ArticlesCount = queryable.Count()
                };
            }
        }
    }
}
