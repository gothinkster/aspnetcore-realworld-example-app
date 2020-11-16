using System.Threading.Tasks;

namespace Conduit.Infrastructure.Security
{
    public interface IPasswordHasher
    {
        Task<byte[]> Hash(string password, byte[] salt);
    }
}