
using System.Threading;
using Conduit.Features.Articles;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Conduit.MinimalApi
{
    public static class Articles
    {
        public static RouteGroupBuilder RegisterArticleEndpoints(this RouteGroupBuilder app)
        {
            app.MapGet("articles", async ([FromQuery] string tag,
                [FromQuery] string author,
                [FromQuery] string favorited,
                [FromQuery] int? limit,
                [FromQuery] int? offset,
                CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(new List.Query(tag, author, favorited, limit, offset), cancellationToken));

            app.MapGet("articles/feed", async ([FromQuery] string tag,
                [FromQuery] string author,
                [FromQuery] string favorited,
                [FromQuery] int? limit,
                [FromQuery] int? offset,
                CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(new List.Query(tag, author, favorited, limit, offset)));

            app.MapGet("articles/{slug}", async ([Validate] string slug,
                CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(new Details.Query(slug), cancellationToken));

            app.MapPost("articles", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async ([Validate][FromBody] Create.Command command,
               CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(command, cancellationToken));

            app.MapPut("articles/{slug}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                [Validate][FromBody] Edit.Model model,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Edit.Command(model, slug), cancellationToken));

            app.MapDelete("articles/{slug}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async ([Validate] string slug,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(slug), cancellationToken));

            return app;
        }
    }
}
