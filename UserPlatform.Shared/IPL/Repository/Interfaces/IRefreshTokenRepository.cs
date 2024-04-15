using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.IPL.Repository.Interfaces;

public interface IRefreshTokenRepository
{
    public void Create(RefreshToken refreshToken);
    public void Delete(RefreshToken refreshToken);
    public Task<RefreshToken> GetTokenAsync(string token);
}
