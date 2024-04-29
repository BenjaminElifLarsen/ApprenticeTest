using Catering.Shared.DL.Models;
using Shared.Patterns.CQRS.Queries;

namespace Catering.Shared.IPL.Repositories.Interfaces;

public interface IMenuRepository
{
    public Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Menu, TMapping> query) where TMapping : BaseReadModel;
    public Task<Menu> GetSingleAsync(Func<Menu, bool> predicate);
    public void Create(Menu menu);
    public void Delete(Menu menu);
    public void Update(Menu menu);
}
