using Microsoft.AspNetCore.Mvc;
using UserPlatform.Services.Contracts;

namespace UserPlatform.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IOrderService _orderService;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    public WeatherForecastController(IOrderService orderService)
    {
        _orderService = orderService;
    }


    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet("Test")]
    public async Task<IActionResult> GetTest()
    {
        var result = await _orderService.GetMenuesAsync();
        return Ok(result);
    }
}
