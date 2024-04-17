using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.Order;

public sealed class OrderPlaceCommand : ICommand
{
    public Guid UserId { get; set; }
    public Guid MenuId { get; set; }
    public DateTime OrderedTo { get; set; }
}
