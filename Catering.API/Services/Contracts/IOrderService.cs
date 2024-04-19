using Catering.API.Models.Order.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.Contracts;

public interface IOrderService
{
    public Task<Result<FutureOrderResponse>> GetOrdersAsync();
}
