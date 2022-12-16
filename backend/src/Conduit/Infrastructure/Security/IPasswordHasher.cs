using System;
using System.Threading.Tasks;

namespace Conduit.Infrastructure.Security
{
    public interface IPasswordHasher : IDisposable
    {
        Task<byte[]> Hash(string password, byte[] salt);
    }
}
