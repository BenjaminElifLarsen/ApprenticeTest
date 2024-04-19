using Shared.Communication.Models.Order;

namespace Catering.API.Models.Order.Response;

public sealed class FutureOrderResponse
{
    public IEnumerable<FutureOrderDetailsResponse> OrderDetails { get; private set; }

    public FutureOrderResponse(GetOrdersFututureQueryResponse response)
    {
        OrderDetails = response.Orders.Select(x => new FutureOrderDetailsResponse(x));
    }
}

public sealed class FutureOrderDetailsResponse
{
    public Guid OrderId { get; private set; }
    public Guid MenuId { get; private set; }
    public string MenuName { get; private set; }
    public DateTime Time { get; private set; }
    public Guid CustomerId { get; private set; }
    public string CustomerName { get; private set; }

    public FutureOrderDetailsResponse(GetOrdersFutureDetailsQueryResponse response)
    {
        OrderId = response.OrderId;
        MenuId = response.MenuId;
        MenuName = response.MenuName;
        Time = response.Time;
        CustomerId = response.CustomerId;
        CustomerName = response.CustomerName;        
    }
}
