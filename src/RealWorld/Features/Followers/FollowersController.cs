using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealWorld.Features.Profiles;
using RealWorld.Infrastructure.Security;

namespace RealWorld.Features.Followers
{
    [Route("profiles")]
    public class FollowersController : Controller
    {
        private readonly IMediator _mediator;

        public FollowersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{username}/follow")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public async Task<ProfileEnvelope> Follow(string username)
        {
            return await _mediator.Send(new Add.Command(username));
        }

        [HttpDelete("{username}/follow")]
        [Authorize(ActiveAuthenticationSchemes = JwtIssuerOptions.Scheme)]
        public async Task<ProfileEnvelope> Unfollow(string username)
        {
            return await _mediator.Send(new Delete.Command(username));
        }
    }
}