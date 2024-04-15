using Shared.Patterns.ResultPattern;
using UserPlatform.Models.Order.Requests;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService
{
    public Task<Result> PlaceOrderAsync(OrderPlacementRequest request)
    {
        var result = _communication.TransmitPlaceOrder(request);
        return Task.FromResult(result);
    }
}
