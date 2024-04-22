using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;

namespace Catering.Shared.IPL.Repositories.Interfaces;

public interface IOrderRepository
{
    public Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Order, TMapping> query) where TMapping : BaseReadModel;
    public Task<Order> GetSingleAsync(Func<Order, bool> predicate);
    public void Create(Order order);
    public void Delete(Order order);
    public void Update(Order order);    
    public Task<IEnumerable<TMapping>> AllByPredicate<TMapping>(BaseQuery<Order, TMapping> query, Func<Order, bool> predicate) where TMapping : BaseReadModel;
}
