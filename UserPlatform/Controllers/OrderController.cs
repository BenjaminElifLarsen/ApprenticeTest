using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserPlatform.Extensions;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Services.Contracts;
using UserPlatform.Sys;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = AccessLevels.DEFAULT_USER)]
    public class OrderController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService, ILogger logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpGet("Menues")]
        public async Task<IActionResult> GetMenues()
        {
            var result = await _orderService.GetMenuesAsync();
            return this.FromResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderPlacementRequest request)
        {
            var userId = ClaimHandling.GetUserId(HttpContext);
            if(userId == Guid.Empty)
            {
                return Unauthorized();
            }
            request.SetUserId(userId);
            var result = await _orderService.PlaceOrderAsync(request);
            return this.FromResult(result);
        }

        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            var userId = ClaimHandling.GetUserId(HttpContext);
            if (userId == Guid.Empty)
            {
                return Unauthorized();
            }
            var result = await _orderService.GetOrdersForUserAsync(userId);
            return this.FromResult(result);
        }
    }
}
