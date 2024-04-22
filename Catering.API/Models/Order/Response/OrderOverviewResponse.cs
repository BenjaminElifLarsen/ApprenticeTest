using Catering.Shared.DL.Models.Enums;
using Shared.Communication.Models.Order;

namespace Catering.API.Models.Order.Response;

public class OrderOverviewResponse
{
    public Guid OrderId { get; private set; }
    public OrderState Status { get; private set; }

    public OrderOverviewResponse(GetOrderOverviewPartQueryResponse response)
    {
        OrderId = response.OrderId;
        Status = (OrderState)response.Status;        
    }
}
