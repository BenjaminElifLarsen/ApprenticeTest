using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.User;

public sealed class UserCreationCommand : ICommand
{
    public Guid UserId { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
}
