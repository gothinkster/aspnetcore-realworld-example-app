using Conduit.Domain;
using Conduit.Features.Profiles;
using Conduit.Features.Users;
using ForgeMap;

namespace Conduit.Infrastructure;

[ForgeMap]
public partial class ConduitMapper
{
    public partial User Forge(Person source);

    public partial Profile ForgeProfile(Person source);
}
