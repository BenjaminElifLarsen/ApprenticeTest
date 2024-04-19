using Catering.API.Models.Menu.Response;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.MenuService;

public sealed partial class MenuService
{
    public async Task<Result<MenuListResponse>> GetMenuesAysnc()
    {
        var result = await _communication.GetMenuesAsync();
        if(!result)
            return new BadRequestResult<MenuListResponse>(result.Errors);
        var data = new MenuListResponse(result.Data!);
        return new SuccessResult<MenuListResponse>(data);
    }
}
