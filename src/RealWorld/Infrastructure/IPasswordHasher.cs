namespace RealWorld.Infrastructure
{
    public interface IPasswordHasher
    {
        byte[] Hash(string password, byte[] salt);
    }
}