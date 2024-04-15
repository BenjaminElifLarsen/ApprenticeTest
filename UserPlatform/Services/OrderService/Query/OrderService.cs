using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService
{
    public async Task<Result<IEnumerable<MenuListQueryResponse>>> GetMenuesAsync()
    {
        var result = await _communication.ReceiveAllMenues(null!);
        return result;
    }

    public async Task<Result<GetOrdersQueryResponse>> GetOrdersForUserAsync(Guid userId)
    {
        var user = await _unitOfWork.UserRepository.GetSingleAsync(userId);
        if(user is null)
            return new InvalidAuthentication<GetOrdersQueryResponse>();
        return await _communication.GetOrdersForUser(user);
    }
}
