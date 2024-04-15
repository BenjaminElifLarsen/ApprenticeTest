using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication.CQRS.Queries;

internal class OrderForCustomer : BaseReadModel
{
    public Guid Id { get; private set; }
    public DateTime Time { get; private set; }

    public OrderForCustomer(Guid id, DateTime time)
    {
        Id = id;
        Time = time;
    }
}

internal class OrderForCustomerQuery : BaseQuery<Order, OrderForCustomer>
{
    public override Expression<Func<Order, OrderForCustomer>> Map()
    {
        return e => new(e.Id, e.Time.Time);
    }
}
