namespace Shared.Communication.Models.Order;

public sealed class GetOrdersFututureQueryResponse
{
    public IEnumerable<GetOrdersFutureDetailsQueryResponse> Orders { get; set; }

    private GetOrdersFututureQueryResponse()
    {
        
    }

    public GetOrdersFututureQueryResponse(IEnumerable<GetOrdersFutureDetailsQueryResponse> orders)
    {
        Orders = orders;        
    }

}

public sealed class GetOrdersFutureDetailsQueryResponse
{
    public Guid OrderId { get; set; }
    public Guid MenuId { get; set; }
    public string MenuName { get; set; }
    public DateTime Time { get; set; }
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; }

    private GetOrdersFutureDetailsQueryResponse()
    {

    }

    public GetOrdersFutureDetailsQueryResponse(Guid orderId, DateTime orderedTo, Guid menuId, string menuName, Guid customerId, string customerName)
    {
        OrderId = orderId;
        MenuId = menuId;
        MenuName = menuName;
        CustomerId = customerId;
        Time = orderedTo;
        CustomerName = customerName;
    }
}
