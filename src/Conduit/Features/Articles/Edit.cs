using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles
{
    public class Edit
    {
        public class ArticleData
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Body { get; set; }
        }

        public class Command : IRequest<ArticleEnvelope>
        {
            public ArticleData Article { get; set; }
            public string Slug { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Article).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, ArticleEnvelope>
        {
            private readonly ConduitContext _db;

            public Handler(ConduitContext db)
            {
                _db = db;
            }

            public async Task<ArticleEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var article = await _db.Articles
                    .Where(x => x.Slug == message.Slug)
                    .FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }


                article.Description = message.Article.Description ?? article.Description;
                article.Body = message.Article.Body ?? article.Body;
                article.Title = message.Article.Title ?? article.Title;
                article.Slug = article.Title.GenerateSlug();

                if (_db.ChangeTracker.Entries().First(x => x.Entity == article).State == EntityState.Modified)
                {
                    article.UpdatedAt = DateTime.UtcNow;
                }
                
                await _db.SaveChangesAsync();

                return new ArticleEnvelope(await _db.Articles.GetAllData()
                    .Where(x => x.Slug == article.Slug)
                    .FirstOrDefaultAsync());            }
        }
    }
}
