using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Articles
{
    public class Create
    {
        public class ArticleData
        {
            public string Title { get; set; }

            public string Description { get; set; }

            public string Body { get; set; }

            public string[] TagList { get; set; }
        }

        public class ArticleDataValidator : AbstractValidator<ArticleData>
        {
            public ArticleDataValidator()
            {
                RuleFor(x => x.Title).NotNull().NotEmpty();
                RuleFor(x => x.Description).NotNull().NotEmpty();
                RuleFor(x => x.Body).NotNull().NotEmpty();
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
                RuleFor(x => x.Article).NotNull().SetValidator(new ArticleDataValidator());
            }
        }

        public class Handler : IRequestHandler<Command, ArticleEnvelope>
        {
            private readonly ConduitContext _db;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ConduitContext db, ICurrentUserAccessor currentUserAccessor)
            {
                _db = db;
                _currentUserAccessor = currentUserAccessor;
            }

            public async Task<ArticleEnvelope> Handle(Command message, CancellationToken cancellationToken)
            {
                var author = await _db.Persons.FirstAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername());
                var tags = new List<Tag>();
                foreach(var tag in (message.Article.TagList ?? Enumerable.Empty<string>()))
                {
                    var t = await _db.Tags.FindAsync(tag);
                    if (t == null)
                    {
                        t = new Tag()
                        {
                            TagId = tag
                        };
                        await _db.Tags.AddAsync(t);
                        //save immediately for reuse
                        await _db.SaveChangesAsync();
                    }
                    tags.Add(t);
                }

                var article = new Article()
                {
                    Author = author,
                    Body = message.Article.Body,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Description = message.Article.Description,
                    Title = message.Article.Title,
                    Slug = message.Article.Title.GenerateSlug()
                };
                await _db.Articles.AddAsync(article);

                await _db.ArticleTags.AddRangeAsync(tags.Select(x => new ArticleTag()
                {
                    Article = article,
                    Tag = x
                }));

                await _db.SaveChangesAsync();

                return new ArticleEnvelope(article);
            }
        }
    }
}
