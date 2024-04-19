using Catering.API.Models.Menu.Request;
using Catering.API.Models.Menu.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.Contracts;

public interface IMenuService
{
    public Task<Result> CreateMenuAsync(MenuCreateRequest request);
    public Task<Result<MenuListResponse>> GetMenuesAysnc();
}
