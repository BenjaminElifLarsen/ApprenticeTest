using Catering.API.Models.Dish.Request;
using Catering.API.Models.Dish.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.Contracts;

public interface IDishService
{
    public Task<Result> CreateDishAsync(DishCreateRequest request);
    public Task<Result<DishListResponse>> GetDishesAsync();
}
