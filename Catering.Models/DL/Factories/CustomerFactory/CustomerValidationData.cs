using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace Catering.Shared.DL.Factories.CustomerFactory;

public sealed class CustomerValidationData(IEnumerable<CustomerData> customers)
{
    public IEnumerable<CustomerData> IdsInUse { get; private set; } = customers;
}

public sealed class CustomerData(Guid id) : BaseReadModel
{
    public Guid Id { get; private set; } = id;
}

public sealed class CustomerDataQuery : BaseQuery<Customer, CustomerData>
{
    public override Expression<Func<Customer, CustomerData>> Map()
    {
        return e => new(e.Id);
    }
}
