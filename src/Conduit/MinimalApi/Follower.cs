using Conduit.Features.Followers;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace Conduit.MinimalApi
{
    public static class Follower
    {
        public static RouteGroupBuilder RegisterFollowerEndpoints(this RouteGroupBuilder app)
        {
            app.MapPost("profiles/{username}/follow", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([Validate] string username, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(new Add.Command(username), cancellationToken));

            app.MapDelete("profiles/{slug}/comments/{id}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([Validate] string username, CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(username), cancellationToken));

            return app;
        }
    }
}
