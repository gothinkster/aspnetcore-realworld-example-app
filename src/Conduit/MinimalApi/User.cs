using MediatR;
using System.Threading;
using Microsoft.AspNetCore.Builder;
using Conduit.Infrastructure;
using Conduit.Features.Users;
using Microsoft.AspNetCore.Mvc;
using Conduit.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;

namespace Conduit.MinimalApi
{
    public static class User
    {
        public static void RegisterUsersEndpoint(this WebApplication app)
        {
            app.MapGet("user", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async (ICurrentUserAccessor currentUserAccessor, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(new Details.Query(currentUserAccessor.GetCurrentUsername() ?? "<unknown>"), cancellationToken)).WithOpenApi();

            app.MapPut("user", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([FromBody] Edit.Command command, CancellationToken cancellationToken,
                IMediator mediator) => await mediator.Send(command, cancellationToken)).WithOpenApi();

            app.MapPost("users", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([FromBody] Create.Command command, CancellationToken cancellationToken,
               IMediator mediator) => await mediator.Send(command, cancellationToken)).WithOpenApi();

            app.MapPost("users/login", [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
            async ([FromBody] Login.Command command, CancellationToken cancellationToken,
              IMediator mediator) => await mediator.Send(command, cancellationToken)).WithOpenApi();
        }
    }
}
