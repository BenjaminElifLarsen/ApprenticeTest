namespace Shared.Communication.Models;

public sealed class OrderPlaceCommand
{
    public Guid UserId { get; set; }
    public Guid MenuId { get; set; }
    public DateTime OrderedTo { get; set; }
}
