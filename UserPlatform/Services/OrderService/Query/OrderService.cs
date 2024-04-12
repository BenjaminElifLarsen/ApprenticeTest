using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;

namespace UserPlatform.Services.OrderService;

internal sealed partial class OrderService
{
    public Task<Result<IEnumerable<MenuListQueryResponse>>> GetMenuesAsync()
    {
        var result = _communication.ReceiveAllMenues(null!);
        return result;
    }
}
