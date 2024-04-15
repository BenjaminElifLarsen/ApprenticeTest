using Shared.Communication.Models.Order;

namespace UserPlatform.Models.Order.Responses;

public sealed class GetOrdersResponse(IEnumerable<GetOrdersMenuResponse> orders)
{
    public IEnumerable<GetOrdersMenuResponse> Orders { get; set; } = orders;
}

public sealed class GetOrdersMenuResponse(GetOrdersMenuQueryResponse order)
{
    public Guid OrderId { get; set; } = order.OrderId;
    public Guid MenuId { get; set; } = order.MenuId;
    public string Name { get; set; } = order.Name;
    public DateTime Time { get; set; } = order.Time;
}
