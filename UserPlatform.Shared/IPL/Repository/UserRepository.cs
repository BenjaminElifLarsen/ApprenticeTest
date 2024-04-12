using Shared.Patterns.CQRS.Queries;
using Shared.Patterns.RepositoryPattern;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.IPL.Repository.Interfaces;

namespace UserPlatform.Shared.IPL.Repository;

public sealed class UserRepository : IUserRepository
{
    private readonly IBaseRepository<User> _repository;

    public UserRepository(IBaseRepository<User> repository)
    {
        _repository = repository;        
    }

    public async Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<User, TMapping> query) where TMapping : BaseReadModel
    {
        return await _repository.AllAsync(query);
    }

    public void Create(User user)
    {
        _repository.Create(user);
    }

    public void Delete(User user)
    {
         _repository.Delete(user);
    }

    public async Task<User> GetSingleAsync(string companyName)
    {
        return await _repository.FindByPredicateAsync(x => string.Equals(x.CompanyName, companyName));
    }
}
