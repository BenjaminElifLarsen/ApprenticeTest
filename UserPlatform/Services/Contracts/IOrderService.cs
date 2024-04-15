using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;
using UserPlatform.Models.Order.Requests;

namespace UserPlatform.Services.Contracts;

public interface IOrderService
{
    public Task<Result<IEnumerable<MenuListQueryResponse>>> GetMenuesAsync();
    public Task<Result> PlaceOrderAsync(OrderPlacementRequest request);
    public Task<Result<GetOrdersQueryResponse>> GetOrdersForUserAsync(Guid userId);
}
