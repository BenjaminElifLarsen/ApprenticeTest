using Catering.Shared.DL.Communication.Models;
using Shared.Communication.Models.Dish;
using Shared.Communication.Models.Menu;

namespace Catering.DataProcessingPlatform.Extensions;

internal static class CommandToRequestExtensions
{
    public static DishCreationRequest ToRequest(this DishCreationCommand command)
    {
        DishCreationRequest request = new() { Name = command.Name };
        return request;
    }

    public static MenuCreationRequest ToRequest(this MenuCreationCommand command)
    {
        MenuCreationRequest request = new()
        {
            Description = command.Description,
            Name = command.Name,
            Parts = command.Dishes.Select(x => new MenuPartCreationRequest()
            {
                Amount = x.Amount,
                Id = x.DishId,
                Price = x.Price,
            })
        };
        return request;
    }
}
