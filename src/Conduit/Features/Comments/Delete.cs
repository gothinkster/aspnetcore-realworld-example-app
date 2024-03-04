using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Comments;

public class Delete
{
    public record Command(string Slug, int Id) : IRequest;

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => RuleFor(x => x.Slug).NotNull().NotEmpty();
    }

    public class QueryHandler(ConduitContext context) : IRequestHandler<Command>
    {
        public async Task Handle(Command message, CancellationToken cancellationToken)
        {
            var article =
                await context
                    .Articles.Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken)
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Article = Constants.NOT_FOUND }
                );

            var comment =
                article.Comments.FirstOrDefault(x => x.CommentId == message.Id)
                ?? throw new RestException(
                    HttpStatusCode.NotFound,
                    new { Comment = Constants.NOT_FOUND }
                );

            context.Comments.Remove(comment);
            await context.SaveChangesAsync(cancellationToken);
            await Task.FromResult(Unit.Value);
        }
    }
}
