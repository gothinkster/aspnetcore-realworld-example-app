using System.Linq;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Errors;

namespace RealWorld.Features.Articles
{
    public class Edit
    {
        public class ArticleData
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Body { get; set; }

            public string Slug { get; set; }
        }

        public class ArticleDataValidator : AbstractValidator<ArticleData>
        {
            public ArticleDataValidator()
            {
                RuleFor(x => x.Slug).NotNull();
            }
        }

        public class Command : IRequest<ArticleEnvelope>
        {
            public ArticleData Article { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Article).NotNull();
            }
        }

        public class Handler : IAsyncRequestHandler<Command, ArticleEnvelope>
        {
            private readonly RealWorldContext _db;

            public Handler(RealWorldContext db)
            {
                _db = db;
            }

            public async Task<ArticleEnvelope> Handle(Command message)
            {
                var article = await _db.Articles
                    .Where(x => x.Slug == message.Article.Slug)
                    .Include(x => x.ArticleTags)
                    .Include(x => x.Author)
                    .FirstOrDefaultAsync();

                if (article == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }


                article.Description = message.Article.Description ?? article.Description;
                article.Body = message.Article.Body ?? article.Body;
                article.Title = message.Article.Title ?? article.Title;
                
                await _db.SaveChangesAsync();

                return new ArticleEnvelope(article);
            }
        }
    }
}
