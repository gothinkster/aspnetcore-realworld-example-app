using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles;

public class Delete
{
    public record Command(string Slug) : IRequest;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Slug).NotNull().NotEmpty();
    }

    public class QueryHandler(ConduitContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command message, CancellationToken cancellationToken)
        {
            var article =
                await context.Articles.FirstOrDefaultAsync(
                    x => x.Slug == message.Slug,
                    cancellationToken
                )
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );

            context.Articles.Remove(article);
            await context.SaveChangesAsync(cancellationToken);
            await Task.FromResult(Unit.Value);
        }
    }
}
