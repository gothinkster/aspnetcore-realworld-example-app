using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Comments;

[Route("articles")]
public class CommentsController(IMediator mediator) : Controller
{
    [HttpPost("{slug}/comments")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public Task<CommentEnvelope> Create(
        string slug,
        [FromBody] Create.Model model,
        CancellationToken cancellationToken
    ) => mediator.Send(new Create.Command(model, slug), cancellationToken);

    [HttpGet("{slug}/comments")]
    public Task<CommentsEnvelope> Get(string slug, CancellationToken cancellationToken) =>
        mediator.Send(new List.Query(slug), cancellationToken);

    [HttpDelete("{slug}/comments/{id}")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
    public Task Delete(string slug, int id, CancellationToken cancellationToken) =>
        mediator.Send(new Delete.Command(slug, id), cancellationToken);
}
