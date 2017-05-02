using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace RealWorld.Features.Users
{
    [Route("user")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public Task<Domain.User> GetCurrent()
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        public Task<Domain.User> UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}