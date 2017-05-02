using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorld.Infrastructure.Security;

namespace RealWorld.Features.Profiles
{
    [Route("profiles")]
    public class ProfilesController : Controller
    {
        private readonly IMediator _mediator;

        public ProfilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{username}")]
        public async Task<ProfileEnvelope> Get(string username)
        {
            var profile = await _mediator.Send(new Profiles.Details.Query()
            {
                Username = username
            });
            if (profile != null)
            {
                return new ProfileEnvelope
                {
                    Profile = profile
                };
            }
            return null;
        }

        [HttpPost("{username}/follow")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public Task<ProfileEnvelope> Follow()
        {
            throw new NotImplementedException();
        }

        [HttpDelete("{username}/follow")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public Task<ProfileEnvelope> Unfollow()
        {
            throw new NotImplementedException();
        }
    }
}