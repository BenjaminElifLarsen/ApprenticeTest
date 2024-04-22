using CateringDataProcessingPlatform.DL.Models;
using Shared.Communication.Models.Order;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;

internal sealed class OrderStatusQuery : BaseQuery<Order, GetOrderOverviewPartQueryResponse>
{
    public override Expression<Func<Order, GetOrderOverviewPartQueryResponse>> Map()
    {
        return e => new(e.Id, (int)e.Status);
    }
}
