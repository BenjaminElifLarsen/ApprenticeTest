using Catering.Shared.IPL.Repositories.Interfaces;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using Shared.Patterns.RepositoryPattern;

namespace Catering.Shared.IPL.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly IBaseRepository<Order> _repository;

    public OrderRepository(IBaseRepository<Order> repositroy)
    {
        _repository = repositroy;        
    }

    public async Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Order, TMapping> query) where TMapping : BaseReadModel
    {
        return await _repository.AllAsync(query);
    }

    public async Task<IEnumerable<TMapping>> AllByPredicate<TMapping>(BaseQuery<Order, TMapping> query, Func<Order, bool> predicate) where TMapping : BaseReadModel
    {
        return await _repository.FindManyByPredicateAsync(predicate, query);
    }

    public void Create(Order order)
    {
        _repository.Create(order);
    }

    public void Delete(Order order)
    {
        _repository.Delete(order);
    }

    public async Task<Order> GetSingleAsync(Func<Order, bool> predicate)
    {
        return await _repository.FindByPredicateAsync(predicate);
    }

    public void Update(Order order)
    {
        _repository.Update(order);
    }
}
