using System.Net;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles
{
    public class Details
    {
        public class Query : IRequest<ArticleEnvelope>
        {
            public Query(string slug)
            {
                Slug = slug;
            }

            public string Slug { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, ArticleEnvelope>
        {
            private readonly ConduitContext _context;

            public QueryHandler(ConduitContext context)
            {
                _context = context;
            }

            public async Task<ArticleEnvelope> Handle(Query message)
            {
                var article = await _context.Articles.GetAllData()
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug);

                if (article == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                return new ArticleEnvelope(article);
            }
        }
    }
}