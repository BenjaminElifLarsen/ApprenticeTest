using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.Order;

public sealed class GetOrdersCommand : ICommand
{
    public Guid UserId { get; set; }
}
