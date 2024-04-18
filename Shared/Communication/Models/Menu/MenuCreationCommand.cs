using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.Menu;

public sealed class MenuCreationCommand : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<MenuPartCreationCommand> Dishes { get; set; }
}

public sealed class MenuPartCreationCommand : ICommand
{
    public Guid DishId { get; set; }
    public string Name { get; set; }
    public float Price { get; set; }
    public uint Amount { get; set; }
}
