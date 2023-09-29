using Conduit.Features.Tags;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Threading;

namespace Conduit.MinimalApi
{
    public static class Tags
    {
        public static RouteGroupBuilder RegisterTagsEndpoints(this RouteGroupBuilder app)
        {
            app.MapGet("tags", async (
                CancellationToken cancellationToken,
                IMediator mediator)
                => await mediator.Send(new List.Query(), cancellationToken));
            return app;
        }
    }
}
