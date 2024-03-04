using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Conduit.Infrastructure;

public class CurrentUserAccessor(IHttpContextAccessor httpContextAccessor) : ICurrentUserAccessor
{
    public string? GetCurrentUsername() =>
        httpContextAccessor
            .HttpContext?.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)
            ?.Value;
}
