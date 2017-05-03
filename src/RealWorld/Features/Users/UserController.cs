using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorld.Infrastructure;
using RealWorld.Infrastructure.Security;

namespace RealWorld.Features.Users
{
    [Route("user")]
    [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
    public class UserController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UserController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet]
        public async Task<UserEnvelope> GetCurrent()
        {
            return await _mediator.Send(new Details.Query()
            {
                Username = _currentUserAccessor.GetCurrentUsername()
            });
        }

        [HttpPut]
        public Task<User> UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}