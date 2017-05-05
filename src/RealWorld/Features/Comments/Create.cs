using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Domain;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Comments
{
    public class Create
    {
        public class CommentData
        {
            public string Body { get; set; }
        }

        public class CommentDataValidator : AbstractValidator<CommentData>
        {
            public CommentDataValidator()
            {
                RuleFor(x => x.Body).NotNull().NotEmpty();
            }
        }

        public class Command : IRequest<CommentEnvelope>
        {
            public CommentData Comment { get; set; }

            public string Slug { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Comment).NotNull().SetValidator(new CommentDataValidator());
            }
        }

        public class Handler : IAsyncRequestHandler<Command, CommentEnvelope>
        {
            private readonly RealWorldContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(RealWorldContext db, ICurrentUserAccessor currentUserAccessor)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<CommentEnvelope> Handle(Command message)
            {
                var article = await _db.Articles
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug);

                if (article == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }

                var author = await _db.Persons.FirstAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername());
                
                var comment = new Comment()
                {
                    Author = author,
                    Body = message.Comment.Body,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _db.Comments.AddAsync(comment);

                article.Comments.Add(comment);

                await _db.SaveChangesAsync();

                return new CommentEnvelope(comment);
            }
        }
    }
}
