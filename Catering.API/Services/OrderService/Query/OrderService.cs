using Catering.API.Models.Order.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.OrderService;

public sealed partial class OrderService
{
    public async Task<Result<FutureOrderResponse>> GetFutureOrdersAsync()
    {
        var result = await _communication.GetFutureOrdersAsync();
        if(!result)
            return new BadRequestResult<FutureOrderResponse>(result.Errors);
        var data = new FutureOrderResponse(result.Data!);
        return new SuccessResult<FutureOrderResponse>(data);
    }


    public async Task<Result<IEnumerable<OrderOverviewResponse>>> GetOrderOverviewAsync()
    {
        var result = await _communication.GetOrdersOverviewAsync();
        if (!result)
            return new BadRequestResult<IEnumerable<OrderOverviewResponse>>(result.Errors);
        var data = result.Data.Orders.Select(x => new OrderOverviewResponse(x));
        return new SuccessResult<IEnumerable<OrderOverviewResponse>>(data);
    }
}
