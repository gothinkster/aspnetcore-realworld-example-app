using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Tags;

[Route("tags")]
public class TagsController(IMediator mediator) : Controller
{
    [HttpGet]
    public Task<TagsEnvelope> Get(CancellationToken cancellationToken) =>
        mediator.Send(new List.Query(), cancellationToken);
}
