using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models.User;

public sealed class UserUpdateCommand : ICommand
{
    public Guid UserId { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }

    public UserUpdateCommand()
    {
        
    }

    public UserUpdateCommand(Guid userId, string? street, string? city)
    {
        UserId = userId;
        Street = street;
        City = city;
    }
}
