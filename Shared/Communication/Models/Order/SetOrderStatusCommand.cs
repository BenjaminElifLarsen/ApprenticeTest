using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.Order;

public sealed class SetOrderStatusCommand : ICommand
{
    public Guid OrderId { get; set; }
    public int OrderStatus { get; set; }

    public SetOrderStatusCommand()
    {
        
    }

    public SetOrderStatusCommand(Guid id, int status)
    {
        OrderId = id;
        OrderStatus = status;        
    }
}
