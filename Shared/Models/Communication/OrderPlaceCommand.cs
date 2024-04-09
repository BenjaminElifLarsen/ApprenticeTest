namespace Shared.Models.Communication;

public sealed class OrderPlaceCommand
{
    public Guid UserId { get; set; }
    public Guid MenuId { get; set; }
    public DateTime OrderedToo { get; set; }
}
