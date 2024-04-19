using Catering.API.Models.Dish.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.DishService;

public sealed partial class DishService
{
    public async Task<Result<DishListResponse>> GetDishesAsync()
    {
        var result = await _communication.GetDishesAsync();
        if (!result)
            return new BadRequestResult<DishListResponse>(result.Errors);
        var data = new DishListResponse(result.Data!);
        return new SuccessResult<DishListResponse>(data);
    }
}
