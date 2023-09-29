
using System.Threading;
using Conduit.Features.Articles;
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
                IMediator mediator) => await mediator.Send(new List.Query(tag, author, favorited, limit, offset), cancellationToken)).WithOpenApi();

            app.MapGet("articles/feed", async ([FromQuery] string tag,
                [FromQuery] string author,
                [FromQuery] string favorited,
                [FromQuery] int? limit,
                [FromQuery] int? offset,
                CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(new List.Query(tag, author, favorited, limit, offset))).WithOpenApi();

            app.MapGet("articles/{slug}", async (string slug,
                CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(new Details.Query(slug), cancellationToken)).WithOpenApi();

            app.MapPost("articles", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async ([FromBody] Create.Command command,
               CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(command, cancellationToken)).WithOpenApi();

            app.MapPut("articles/{slug}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                [FromBody] Edit.Model model,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Edit.Command(model, slug), cancellationToken)).WithOpenApi();

            app.MapDelete("articles/{slug}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(slug), cancellationToken)).WithOpenApi();

            return app;
        }
    }
}
