using Shared.Patterns.CQRS.Queries;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.IPL.Repository.Interfaces;

public interface IUserRepository
{
    public void Create(User user);
    public void Update(User user);
    public void Delete(User user);
    public Task<User> GetSingleAsync(string companyName);
    public Task<User> GetSingleAsync(Guid userId);
    public Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<User, TMapping> query) where TMapping : BaseReadModel;
}
