namespace UserPlatform.Models.Order.Requests;

public sealed class OrderPlacementRequest
{
    public Guid UserId { get; private set; }
    public Guid MenuId { get; set; }
    public DateTime OrderedTo { get; set; }

    public void SetUserId(Guid userId) => UserId = userId;
}
