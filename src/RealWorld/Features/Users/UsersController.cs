using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealWorld.Features.Users
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
        public async Task<UserEnvelope> Create([FromBody] Create.Command command)
        {
            var user = await _mediator.Send(command);
            if (user != null)
            {
                return new UserEnvelope
                {
                    User = user
                };
            }
            return null;
        }


        [HttpPost("login")]
        public async Task<UserEnvelope> Login([FromBody] Login.Command command)
        {
            var user = await _mediator.Send(command);
            if (user != null)
            {
                return new UserEnvelope
                {
                    User = user
                };
            }
            return null;
        }
    }
}