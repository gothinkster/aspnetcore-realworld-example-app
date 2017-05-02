using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealWorld.Features.Users
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Domain.User> Create([FromBody] Create.Command command)
        {
            return await _mediator.Send(command);
        }


        [HttpPost("login")]
        public async Task<Domain.User> Login([FromBody] Login.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}