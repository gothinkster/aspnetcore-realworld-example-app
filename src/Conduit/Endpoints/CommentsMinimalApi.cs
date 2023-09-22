using Conduit.Features.Comments;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Conduit.Endpoints
{
    public static class CommentsMinimalApi
    {
        public static void RegisterCommentEndpoints(this WebApplication app)
        {
            app.MapPost("/articles/{slug}/comments", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                [FromBody] Create.Model model, CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(new Create.Command(model, slug), cancellationToken));

            app.MapGet("/articles/{slug}/comments", async (string slug,
               [FromBody] Create.Model model, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(new List.Query(slug), cancellationToken));

            app.MapDelete("/articles/{slug}/comments/{id}", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)] async (string slug,
                int id, CancellationToken cancellationToken, IMediator mediator) => await mediator.Send(new Delete.Command(slug, id), cancellationToken));

        }
    }
}

