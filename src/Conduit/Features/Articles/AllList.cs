using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles
{
    public class AllList
    {
        public class Query : IRequest<ArticlesEnvelope>
        {
            public Query( string author, string title, string createdAt, string updatedAt, int? limit, int? offset)
            {
                Author = author;
                Title = title;
                CreatedAt = createdAt;
                UpdatedAt = updatedAt;
                Limit = limit;
                Offset = offset;
            }

            public string Author { get; }
            public string Title { get; }
            public string CreatedAt { get;  }
            public string UpdatedAt { get;  }
            public int? Limit { get; }
            public int? Offset { get; }
        }

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
                if (!string.IsNullOrWhiteSpace(message.Title))
                {                  
                    queryable = queryable.Where(x => x.Title == message.Title);                  
                }
                if (message.CreatedAt != null)
                {
                    queryable = queryable.Where(x => x.CreatedAt == DateTime.Parse(message.CreatedAt));
                }
                if (message.UpdatedAt != null)
                {
                    queryable = queryable.Where(x => x.UpdatedAt == DateTime.Parse(message.UpdatedAt));
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