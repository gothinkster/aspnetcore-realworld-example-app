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
using RealWorld.Infrastructure.Security;

namespace RealWorld.Features.Users
{
    [Route("user")]
    [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<UserEnvelope> GetCurrent()
        {
            var user = await _mediator.Send(new Details.Query()
            {
                Username = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value
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