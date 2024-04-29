using Catering.Shared.IPL.Repositories.Interfaces;
using Catering.Shared.DL.Models;
using Shared.Patterns.CQRS.Queries;
using Shared.Patterns.RepositoryPattern;

namespace Catering.Shared.IPL.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly IBaseRepository<Customer> _repository;

    public CustomerRepository(IBaseRepository<Customer> repository)
    {
        _repository = repository;        
    }

    public async Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Customer, TMapping> query) where TMapping : BaseReadModel
    {
        return await _repository.AllAsync(query);
    }

    public void Create(Customer customer)
    {
        _repository.Create(customer);
    }

    public void Delete(Customer customer)
    {
        _repository.Delete(customer);
    }

    public async Task<Customer> GetSingleAsync(Func<Customer, bool> predicate)
    {
        return await _repository.FindByPredicateAsync(predicate);
    }

    public void Update(Customer customer)
    {
        _repository.Update(customer);
    }
}
