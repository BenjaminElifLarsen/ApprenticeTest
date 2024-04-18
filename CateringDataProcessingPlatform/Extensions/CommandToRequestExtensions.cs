using Catering.Shared.DL.Communication.Models;
using Shared.Communication.Models.Dish;

namespace CateringDataProcessingPlatform.Extensions;

internal static class CommandToRequestExtensions
{
    public static DishCreationRequest ToRequest(this DishCreationCommand command)
    {
        DishCreationRequest request = new() { Name = command.Name };
        return request;
    } 
}
