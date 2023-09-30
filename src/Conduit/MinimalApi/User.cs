using MediatR;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Conduit.Infrastructure;
using Conduit.Features.Users;
using Microsoft.AspNetCore.Mvc;
using Conduit.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;

namespace Conduit.MinimalApi
{
    public static class User
    {
        public static RouteGroupBuilder RegisterUsersEndpoint(this RouteGroupBuilder app)
        {
            app.MapGet("user", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([Validate] ICurrentUserAccessor currentUserAccessor, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(new Details.Query(currentUserAccessor.GetCurrentUsername() ?? "<unknown>"), cancellationToken));

            app.MapPut("user", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([Validate][FromBody] Edit.Command command, CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(command, cancellationToken));

            app.MapPost("user",
            async ([Validate][FromBody] Create.Command command, CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(command, cancellationToken));

            app.MapPost("user/login",
            async ([Validate][FromBody] Login.Command command, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(command, cancellationToken));

            return app;
        }
    }
}
