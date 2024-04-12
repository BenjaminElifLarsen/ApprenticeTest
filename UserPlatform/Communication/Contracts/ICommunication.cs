using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Communication.Contracts;

public interface ICommunication
{
    public Result TransmitUser(User user);
    public Task<Result<IEnumerable<MenuListQueryResponse>>> ReceiveAllMenues(User user);
    public Result TransmitPlaceOrder(OrderPlacementRequest orderPlacementRequest);
}
