using Shared.Patterns.RepositoryPattern;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.IPL.Repository.Interfaces;

namespace UserPlatform.Shared.IPL.Repository;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly IBaseRepository<RefreshToken> _repository;

    public RefreshTokenRepository(IBaseRepository<RefreshToken> repository)
    {
        _repository = repository;        
    }

    public void Create(RefreshToken refreshToken)
    {
        _repository.Create(refreshToken);
    }

    public void Delete(RefreshToken refreshToken)
    {
        refreshToken.Revoke();
        _repository.Update(refreshToken);
    }

    public async Task<RefreshToken> GetTokenAsync(string token)
    {
        return await _repository.FindByPredicateAsync(x => string.Equals(x.Token, token));
    }

    public async Task<RefreshToken> GetTokenAsync(Guid userId)
    {
        return await _repository.FindByPredicateAsync(x => x.User.Id == userId && x.Revoked == false);
    }
}
