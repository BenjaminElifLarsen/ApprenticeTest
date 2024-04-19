using Catering.API.Extensions;
using Catering.API.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;        
    }

    [HttpGet]
    public async Task<IActionResult> GetMenues()
    {
        return this.FromResult(await _menuService.GetMenuesAysnc());
    }
}
