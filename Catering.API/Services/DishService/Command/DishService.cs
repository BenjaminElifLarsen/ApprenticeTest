using Catering.API.Models.Dish.Request;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.DishService;

public sealed partial class DishService
{
    public Task<Result> CreateDishAsync(DishCreateRequest request)
    {
        return _communication.TransmitCreateDishAsync(request);
    }
}
