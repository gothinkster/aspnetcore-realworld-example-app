using MediatR;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Conduit.Features.Profiles;

namespace Conduit.MinimalApi
{
    public static class Profiles
    {
        public static void RegisterProfileEndpoints(this WebApplication app) =>
            app.MapGet("profiles/{username}", async (string username,
                CancellationToken cancellationToken,
                IMediator mediator)
                => await mediator.Send(new Details.Query(username), cancellationToken)).WithOpenApi();
    }
}
