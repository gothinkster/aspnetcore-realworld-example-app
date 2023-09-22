using Conduit.Features.Favorites;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using System.Threading;

namespace Conduit.MinimalApi
{
    public static class Favorites
    {
        public static void RegisterFavoritesEndpoint(this WebApplication app)
        {
            app.MapPost("articles/{slug}/favorite", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                 CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(new Add.Command(slug), cancellationToken)).WithOpenApi();

            app.MapDelete("articles/{slug}/favorite", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(slug), cancellationToken)).WithOpenApi();

        }
    }
}
