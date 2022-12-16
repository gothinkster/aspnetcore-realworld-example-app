using System.Threading;
using System.Threading.Tasks;
using Conduit.Features.Profiles;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Followers
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
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public Task<ProfileEnvelope> Follow(string username, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Add.Command(username), cancellationToken);
        }

        [HttpDelete("{username}/follow")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public Task<ProfileEnvelope> Unfollow(string username, CancellationToken cancellationToken)
        {
            return _mediator.Send(new Delete.Command(username), cancellationToken);
        }
    }
}
