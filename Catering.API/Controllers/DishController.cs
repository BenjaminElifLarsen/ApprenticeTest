using Catering.API.Extensions;
using Catering.API.Models.Dish.Request;
using Catering.API.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService;        
    }

    [HttpGet]
    public async Task<IActionResult> GetDishes()
    {
        return this.FromResult(await _dishService.GetDishesAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateDish([FromBody] DishCreateRequest request)
    {
        return this.FromResult(await _dishService.CreateDishAsync(request));
    }
}
