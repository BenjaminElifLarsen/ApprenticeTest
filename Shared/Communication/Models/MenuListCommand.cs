using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models;

public sealed class MenuListCommand : ICommand
{
    public Guid Id { get; set; }
}
