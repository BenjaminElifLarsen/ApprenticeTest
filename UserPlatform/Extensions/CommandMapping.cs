using Shared.Communication.Models.Order;
using Shared.Communication.Models.User;
using System.Text.Json;
using System.Text;
using UserPlatform.Models.Internal;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Shared.DL.Models;
using ICommand = Shared.Patterns.CQRS.Commands.ICommand;

namespace UserPlatform.Extensions;

public static class CommandMapping
{
    public static UserCreationCommand ToCommand(this User user)
    {
        UserCreationCommand command = new()
        {
            UserId = user.Id,
            City = user.Location.City,
            Street = user.Location.Street,
        };
        return command;
    }

    public static OrderPlaceCommand ToCommand(this OrderPlacementRequest request)
    {
        OrderPlaceCommand command = new()
        {
            UserId = request.UserId,
            MenuId = request.MenuId,
            OrderedTo = request.OrderedTo,
        };
        return command;
    }

    public static UserUpdateCommand ToCommand(this UserUpdateDTO request)
    {
        UserUpdateCommand command = new()
        {
            City = request.City?.Data,
            Street = request.Street?.Data,
            UserId = request.Id,
        };
        return command;
    }

    public static byte[] ToBody<T>(this T command) where T : ICommand
    {
        var message = JsonSerializer.Serialize(command);
        var body = Encoding.UTF8.GetBytes(message);
        return body;
    }
}
