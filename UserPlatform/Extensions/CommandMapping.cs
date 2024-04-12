using Shared.Communication.Models;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Shared.DL.Models;

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
}
