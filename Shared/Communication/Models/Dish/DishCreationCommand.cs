using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.Dish;

public sealed class DishCreationCommand : ICommand
{
    public string Name { get; set; }
}
