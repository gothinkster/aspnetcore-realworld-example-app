using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Comments
{
    public class AllList
    {
        public class Query : IRequest<CommentsEnvelope>
        {
            public Query( string author, int articleId, string createdAt, string updatedAt, int? limit, int? offset, bool afterAdminReview)
            {
                Author = author;
                ArticleId = articleId;
                CreatedAt = createdAt;
                UpdatedAt = updatedAt;
                Limit = limit;
                Offset = offset;
                AfterAdminReview = afterAdminReview;
            }

            public string Author { get; }
            public int? ArticleId { get; }
            public string CreatedAt { get;  }
            public string UpdatedAt { get;  }
            public int? Limit { get; }
            public int? Offset { get; }
            public bool AfterAdminReview { get; }
        }

        public class QueryHandler : IRequestHandler<Query, CommentsEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public QueryHandler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<CommentsEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                IQueryable<Comment> queryable = _context.Comments.GetAllData().Where(x=>x.IsBanned == true);

                if (message.AfterAdminReview)
                {
                    queryable = queryable.Where(x => x.AfterAdminReview == message.AfterAdminReview);
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
                        return new CommentsEnvelope();
                    }
                }

                if (message.ArticleId != null)
                {                  
                    queryable = queryable.Where(x => x.ArticleId == message.ArticleId);                  
                }

                if (message.CreatedAt != null)
                {
                    queryable = queryable.Where(x => x.CreatedAt == DateTime.Parse(message.CreatedAt));
                }

                if (message.UpdatedAt != null)
                {
                    queryable = queryable.Where(x => x.UpdatedAt == DateTime.Parse(message.UpdatedAt));
                }

                var comments = await queryable                    
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip(message.Offset ?? 0)
                    .Take(message.Limit ?? 20)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new CommentsEnvelope()
                {
                    Comments = comments,
                    CommentsCount = queryable.Count()
                };
            }
        }
    }
}