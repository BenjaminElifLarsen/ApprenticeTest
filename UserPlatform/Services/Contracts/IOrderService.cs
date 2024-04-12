using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;

namespace UserPlatform.Services.Contracts;

public interface IOrderService
{
    public Task<Result<IEnumerable<MenuListQueryResponse>>> GetMenuesAsync();
    public Task<>
}
