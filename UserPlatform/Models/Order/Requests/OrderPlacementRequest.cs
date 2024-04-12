namespace UserPlatform.Models.Order.Requests;

public sealed class OrderPlacementRequest
{
    public Guid UserId { get; set; }
    public Guid MenuId { get; set; }
    public DateTime OrderedTo { get; set; }
}
