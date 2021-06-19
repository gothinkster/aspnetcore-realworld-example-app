using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Comments
{
    public class Create
    {
        public record CommentData(string? Body);
        public record Command(Model Model, string Slug) : IRequest<CommentEnvelope>;
        public record Model(CommentData Comment) : IRequest<CommentEnvelope>;
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Model.Comment.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, CommentEnvelope>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<CommentEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var article = await _context.Articles
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

                if (article == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Article = Constants.NOT_FOUND });
                }

                var author = await _context.Persons.FirstAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);

                var comment = new Comment()
                {
                    Author = author,
                    Body = message.Model.Comment.Body ?? string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _context.Comments.AddAsync(comment, cancellationToken);

                article.Comments.Add(comment);

                await _context.SaveChangesAsync(cancellationToken);

                return new CommentEnvelope(comment);
            }
        }
    }
}
