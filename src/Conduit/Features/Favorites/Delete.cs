using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Features.Articles;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Favorites;

public class Delete
{
    public record Command(string Slug) : IRequest<ArticleEnvelope>;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            DefaultValidatorExtensions.NotNull(RuleFor(x => x.Slug)).NotEmpty();
        }
    }

    public class QueryHandler : IRequestHandler<Command, ArticleEnvelope>
    {
        private readonly ConduitContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public QueryHandler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _currentUserAccessor = currentUserAccessor;
        }

        public async Task<ArticleEnvelope> Handle(
            Command message,
            CancellationToken cancellationToken
        )
        {
            var article =
                await _context.Articles.FirstOrDefaultAsync(
                    x => x.Slug == message.Slug,
                    cancellationToken
                )
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );

            var person = await _context.Persons.FirstOrDefaultAsync(
                x => x.Username == _currentUserAccessor.GetCurrentUsername(),
                cancellationToken
            );
            if (person is null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );
            }

            var favorite = await _context.ArticleFavorites.FirstOrDefaultAsync(
                x => x.ArticleId == article.ArticleId && x.PersonId == person.PersonId,
                cancellationToken
            );

            if (favorite != null)
            {
                _context.ArticleFavorites.Remove(favorite);
                await _context.SaveChangesAsync(cancellationToken);
            }

            article =
                await _context.Articles
                    .GetAllData()
                    .FirstOrDefaultAsync(
                        x => x.ArticleId == article.ArticleId,
                        cancellationToken
                    );
            if (article is null)
            {
                throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );
            }

            return new ArticleEnvelope(article);
        }
    }
}
