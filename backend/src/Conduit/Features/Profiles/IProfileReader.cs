using System.Threading;
using System.Threading.Tasks;

namespace Conduit.Features.Profiles
{
    public interface IProfileReader
    {
        Task<ProfileEnvelope> ReadProfile(string username, CancellationToken cancellationToken);
    }
}
