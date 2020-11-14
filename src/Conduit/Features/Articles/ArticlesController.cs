using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Articles
{
    [Route("articles")]
    public class ArticlesController : Controller
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ArticlesEnvelope> Get([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new List.Query(tag, author, favorited, limit, offset), cancellationToken);
        }

        [HttpGet("feed")]
        public async Task<ArticlesEnvelope> GetFeed([FromQuery] string tag, [FromQuery] string author, [FromQuery] string favorited, [FromQuery] int? limit, [FromQuery] int? offset, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new List.Query(tag, author, favorited, limit, offset)
            {
                IsFeed = true
            });
        }

        [HttpGet("{slug}")]
        public async Task<ArticleEnvelope> Get(string slug, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new Details.Query(slug), cancellationToken);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> Create([FromBody]Create.Command command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpPut("{slug}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<ArticleEnvelope> Edit(string slug, [FromBody]Edit.Command command, CancellationToken cancellationToken)
        {
            command.Slug = slug;
            return await _mediator.Send(command, cancellationToken);
        }

        [HttpDelete("{slug}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete(string slug, CancellationToken cancellationToken)
        {
            await _mediator.Send(new Delete.Command(slug), cancellationToken);
        }
    }
}