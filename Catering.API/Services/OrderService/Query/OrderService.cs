using Catering.API.Models.Order.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.OrderService;

public sealed partial class OrderService
{
    public async Task<Result<FutureOrderResponse>> GetOrdersAsync()
    {
        var result = await _communication.GetFutureOrdersAsync();
        if(!result)
            return new BadRequestResult<FutureOrderResponse>(result.Errors);
        var data = new FutureOrderResponse(result.Data!);
        return new SuccessResult<FutureOrderResponse>(data);
    }
}
