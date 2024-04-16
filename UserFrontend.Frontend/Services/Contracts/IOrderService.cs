using Shared.Patterns.ResultPattern;
using UserFrontend.Frontend.Models.Order.Requests;
using UserFrontend.Frontend.Models.Order.Responses;

namespace UserFrontend.Frontend.Services.Contracts;

public interface IOrderService
{
    public Task<Result<IEnumerable<MenuResponse>>> GetMenuesAsync();
    public Task<Result> PlaceOrderAsync(OrderRequest body);
    public Task<Result<OrderCollectionResponse>> GetOrders();
}
