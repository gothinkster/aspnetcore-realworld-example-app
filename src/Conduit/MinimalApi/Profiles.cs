using MediatR;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Conduit.Features.Profiles;
using Microsoft.AspNetCore.Routing;

namespace Conduit.MinimalApi
{
    public static class Profiles
    {
        public static RouteGroupBuilder RegisterProfileEndpoints(this RouteGroupBuilder app)
        {
            app.MapGet("profiles/{username}", async (string username,
                CancellationToken cancellationToken,
                IMediator mediator)
                => await mediator.Send(new Details.Query(username), cancellationToken));
            return app;
        }
    }
}
