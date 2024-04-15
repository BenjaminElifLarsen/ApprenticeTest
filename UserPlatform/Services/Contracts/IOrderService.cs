using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Models.Order.Responses;

namespace UserPlatform.Services.Contracts;

public interface IOrderService
{
    public Task<Result<IEnumerable<MenuListQueryResponse>>> GetMenuesAsync();
    public Task<Result> PlaceOrderAsync(OrderPlacementRequest request);
    public Task<Result<GetOrdersResponse>> GetOrdersForUserAsync(Guid userId); 
}
