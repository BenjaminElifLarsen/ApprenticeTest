using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace Catering.Shared.DL.Factories.CustomerFactory;

public sealed class CustomerValidationData(IEnumerable<CustomerData> idsInUse)
{
    public IEnumerable<CustomerData> IdsInUse { get; private set; } = idsInUse;
}

public sealed class CustomerData : BaseReadModel
{
    public Guid Id { get; private set; }

    public CustomerData(Guid id)
    {
        
    }
}

public sealed class CustomerDataQuery : BaseQuery<CustomerData, CustomerData>
{
    public override Expression<Func<CustomerData, CustomerData>> Map()
    {
        return e => new(e.Id);
    }
}
