using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;

namespace Catering.Shared.IPL.Repositories.Interfaces;

public interface ICustomerRepository
{
    public Task<IEnumerable<TMapping>> AllAsync<TMapping>(BaseQuery<Customer, TMapping> query) where TMapping : BaseReadModel;
    public Task<Customer> GetSingleAsync(Func<Customer, bool> predicate);
    public void Create(Customer customer);
    public void Delete(Customer customer);
}
