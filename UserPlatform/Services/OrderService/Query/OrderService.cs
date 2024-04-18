using Shared.Communication.Models.Menu;
using Shared.Patterns.ResultPattern;
using UserPlatform.Models.Order.Responses;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService
{
    public async Task<Result<IEnumerable<MenuListQueryResponse>>> GetMenuesAsync()
    {
        var result = await _communication.ReceiveAllMenuesAsync(null!);
        return result;
    }

    public async Task<Result<GetOrdersResponse>> GetOrdersForUserAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetSingleAsync(userId);
        if(user is null)
        {
            _logger.Error("{Identifier}: {Method} was activated without a found user {UserId}", _identifier, nameof(GetOrdersForUserAsync), userId);
            return new InvalidAuthentication<GetOrdersResponse>();
        }
        var result = await _communication.GetOrdersForUserAsync(user);
        if (!result)
        {
            return new BadRequestResult<GetOrdersResponse>(result.Errors);
        }
        var orders = result.Data.Orders.Select(x => new GetOrdersMenuResponse(x));
        return new SuccessResult<GetOrdersResponse>(new GetOrdersResponse(orders));
    }
}
