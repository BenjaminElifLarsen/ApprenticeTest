using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.DL.Factories.RefreshTokenFactory;

public interface IRefreshTokenFactory
{
    public RefreshToken Build(Guid id, string token);
}
