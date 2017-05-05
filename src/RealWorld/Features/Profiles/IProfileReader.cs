using System.Threading.Tasks;

namespace RealWorld.Features.Profiles
{
    public interface IProfileReader
    {
        Task<ProfileEnvelope> ReadProfile(string username);
    }
}