using Catering.Shared.DL.Models;
using Shared.Patterns.CQRS.Queries;

namespace Catering.Shared.IPL.Repositories.Interfaces;

public interface IDishRepository
{
    public Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Dish, TMapping> query) where TMapping : BaseReadModel;
    //public Task<TMapping> GetSingleAsync<TMapping>(Func<Dish, bool> predicate, BaseQuery<Dish, TMapping> query) where TMapping: BaseReadModel;
    public Task<Dish> GetSingleAsync(Func<Dish, bool> predicate);

    public void Create(Dish dish);
    public void Delete(Dish dish);
    public Task<bool> IsNameUnique(string name);
    public void Update(Dish dish);
}
