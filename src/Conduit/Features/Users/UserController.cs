using System.Threading;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Features.Users
{
    [Route("user")]
    [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
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
        public async Task<UserEnvelope> GetCurrent(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new Details.Query()
            {
                Username = _currentUserAccessor.GetCurrentUsername()
            }, cancellationToken);
        }

        [HttpPut]
        public async Task<UserEnvelope> UpdateUser([FromBody]Edit.Command command, CancellationToken cancellationToken)
        {
            return await _mediator.Send(command, cancellationToken);
        }
    }
}