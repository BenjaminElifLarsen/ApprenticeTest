using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;

internal sealed class OrderDetails : BaseReadModel
{
    public Guid OrderId { get; private set; }
    public DateTime OrderedTo { get; private set; }
    public Guid MenuId { get; private set; }
    public Guid CustomerId { get; private set; }

    public OrderDetails(Guid orderId, Guid menuId, Guid customerId, DateTime orderedTo)
    {
        OrderId = orderId;
        MenuId = menuId;
        CustomerId = customerId;
        OrderedTo = orderedTo;        
    }
}

internal sealed class OrderDetailsQuery : BaseQuery<Order, OrderDetails>
{
    public override Expression<Func<Order, OrderDetails>> Map()
    {
        return e => new(e.Id, e.Menu.Id, e.Customer.Id, e.Time.Time);
    }
}

