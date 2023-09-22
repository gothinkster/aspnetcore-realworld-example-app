
using System.Threading;
using Conduit.Features.Articles;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Conduit.Endpoints
{
    public static class ArticlesMinimalApi
    {
        public static void RegisterArticleEndpoints(this WebApplication app)
        {
            app.MapGet("/articles", async ([FromQuery] string tag,
                [FromQuery] string author,
                [FromQuery] string favorited,
                [FromQuery] int? limit,
                [FromQuery] int? offset,
                CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(new List.Query(tag, author, favorited, limit, offset), cancellationToken));

            app.MapGet("/articles/feed", async ([FromQuery] string tag,
                [FromQuery] string author,
                [FromQuery] string favorited,
                [FromQuery] int? limit,
                [FromQuery] int? offset,
                CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(new List.Query(tag, author, favorited, limit, offset)));

            app.MapGet("/articles/{slug}", async (string slug,
                CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(new Details.Query(slug), cancellationToken));

            app.MapPost("/articles", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async ([FromBody] Create.Command command,
               CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(command, cancellationToken));

            app.MapPut("/articles/{slug}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                [FromBody] Edit.Model model,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Edit.Command(model, slug), cancellationToken));

            app.MapDelete("/articles/{slug}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(slug), cancellationToken));
        }
    }
}