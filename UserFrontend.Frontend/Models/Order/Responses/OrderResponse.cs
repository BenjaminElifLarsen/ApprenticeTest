namespace UserFrontend.Frontend.Models.Order.Responses;

public class OrderCollectionResponse
{
    public IEnumerable<OrderResponse> Orders { get; set; }
}

public sealed class OrderResponse
{
    public Guid OrderId { get; set; }
    public Guid MenuId { get; set; }
    public string Name { get; set; } 
    public DateTime Time { get; set; }
}
