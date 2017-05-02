using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            var user = await _mediator.Send(new Details.Query()
            {
                Username = _currentUserAccessor.GetCurrentUsername()
            });
            if (user != null)
            {
                return new UserEnvelope
                {
                    User = user
                };
            }
            return null;
        }

        [HttpPut]
        public Task<Domain.User> UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}