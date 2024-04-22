using Catering.API.Models.Dish.Request;
using Catering.API.Models.Menu.Request;
using Catering.API.Models.Order.Request;
using Catering.API.Models.Order.Response;
using Shared.Communication.Models.Dish;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Communication.Contract;

public interface ICommunication
{
    public Task<Result> TransmitCreateDishAsync(DishCreateRequest request);
    public Task<Result> TransmitCreateMenuAsync(MenuCreateRequest request);
    public Task<Result<GetOrdersFututureQueryResponse>> GetFutureOrdersAsync();
    public Task<Result<MenuListFinerDetailsQueryResponse>> GetMenuesAsync();
    public Task<Result<DishListQueryResponse>> GetDishesAsync();
    public Task<Result> TransmitOrderStatusAsync(SetOrderStatusRequest request);
    public Task<Result<GetOrderOverviewQueryResponse>> GetOrdersOverviewAsync();
}
