using Catering.API.Models.Order.Request;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.OrderService;

public sealed partial class OrderService
{
    public Task<Result> SetOrderStatusAsync(SetOrderStatusRequest request)
    {
        return _communication.TransmitOrderStatusAsync(request);
    }
}
