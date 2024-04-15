using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.DL.Factories.RefreshTokenFactory;

public sealed class RefreshTokenFactory : IRefreshTokenFactory
{
    public RefreshToken Build(Guid id, string token)
    {
        return new RefreshToken(id, token);
    }
}
