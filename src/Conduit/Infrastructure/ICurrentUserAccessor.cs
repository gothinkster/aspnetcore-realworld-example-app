namespace Conduit.Infrastructure
{
    public interface ICurrentUserAccessor
    {
        string? GetCurrentUsername();
    }
}
