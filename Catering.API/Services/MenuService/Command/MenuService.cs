using Catering.API.Models.Menu.Request;
using Shared.Patterns.ResultPattern;

namespace Catering.API.Services.MenuService;

public sealed partial class MenuService
{
    public Task<Result> CreateMenuAsync(MenuCreateRequest request)
    {
        return _communication.TransmitCreateMenuAsync(request);
    }
}
