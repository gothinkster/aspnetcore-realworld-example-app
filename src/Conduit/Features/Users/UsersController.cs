using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Users
{
    [Route("users")]
    public class UsersController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<UserEnvelope> Create([FromBody] Create.Command command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }


        [HttpPost("login")]
        public async Task<UserEnvelope> Login([FromBody] Login.Command command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}