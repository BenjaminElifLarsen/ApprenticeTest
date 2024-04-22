using Catering.API.Models.Order.Request;
using Catering.API.Models.Order.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.Contracts;

public interface IOrderService
{
    public Task<Result<FutureOrderResponse>> GetFutureOrdersAsync();
    public Task<Result> SetOrderStatusAsync(SetOrderStatusRequest request);
    public Task<Result<IEnumerable<OrderOverviewResponse>>> GetOrderOverviewAsync();
}
