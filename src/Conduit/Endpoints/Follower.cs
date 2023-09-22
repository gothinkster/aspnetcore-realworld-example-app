using Conduit.Features.Followers;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using System.Threading;

namespace Conduit.Endpoints
{
    public static class Follower
    {
        public static void RegisterFollowerEndpoints(this WebApplication app)
        {
            app.MapPost("/profiles/{username}/follow", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async (string username, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(new Add.Command(username), cancellationToken));

            app.MapDelete("/profiles/{slug}/comments/{id}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async (string username, CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(username), cancellationToken));
        }
    }
}
