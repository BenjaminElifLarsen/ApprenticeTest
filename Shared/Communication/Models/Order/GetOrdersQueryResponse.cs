namespace Shared.Communication.Models.Order;

public sealed class GetOrdersQueryResponse
{
    public IEnumerable<GetOrdersMenuQueryResponse> Orders { get; set; }
}

public sealed class GetOrdersMenuQueryResponse
{
    public Guid OrderId { get; set; }
    public Guid MenuId { get; set; }
    public string Name { get; set; }
    public DateTime Time { get; set; }

    public GetOrdersMenuQueryResponse()
    {
        
    }

    public GetOrdersMenuQueryResponse(Guid orderId, string name, DateTime time, Guid menuId)
    {
        OrderId = orderId;
        Name = name;
        Time = time;
        MenuId = menuId;
    }
}
