using Conduit.Features.Tags;
using MediatR;
using Microsoft.AspNetCore.Builder;
using System.Threading;

namespace Conduit.MinimalApi
{
    public static class Tags
    {
        public static void RegisterProfileEndpoints(this WebApplication app) =>
           app.MapGet("tags", async (
               CancellationToken cancellationToken,
               IMediator mediator)
               => await mediator.Send(new List.Query(), cancellationToken));
    }
}
