using Catering.API.Models.Dish.Request;
using Catering.API.Models.Menu.Request;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Communication.Contract;

public interface ICommunication
{
    public Task<Result> TransmitCreateDishAsync(DishCreateRequest request);
    public Task<Result> TransmitCreateMenuAsync(MenuCreateRequest request);
}
