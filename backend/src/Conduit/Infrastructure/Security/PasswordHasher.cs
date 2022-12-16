using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Conduit.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly HMACSHA512 x = new(Encoding.UTF8.GetBytes("realworld"));

        public Task<byte[]> Hash(string password, byte[] salt)
        {
            var bytes = Encoding.UTF8.GetBytes(password);

            var allBytes = new byte[bytes.Length + salt.Length];
            Buffer.BlockCopy(bytes, 0, allBytes, 0, bytes.Length);
            Buffer.BlockCopy(salt, 0, allBytes, bytes.Length, salt.Length);

            return x.ComputeHashAsync(new MemoryStream(allBytes));
        }

        public void Dispose() => x.Dispose();
    }
}
