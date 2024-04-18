using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Patterns.ResultPattern;
using UserPlatform.Models.Internal;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Communication.Contracts;

public interface ICommunication
{
    public Result TransmitUser(User user);
    public Task<Result<IEnumerable<MenuListQueryResponse>>> ReceiveAllMenues(User user);
    public Result TransmitPlaceOrder(OrderPlacementRequest orderPlacementRequest);
    public Task<Result<GetOrdersQueryResponse>> GetOrdersForUser(User user);
    public Result TransmitUserChanges(UserUpdateDTO changes);
}
