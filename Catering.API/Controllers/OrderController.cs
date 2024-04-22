using Catering.API.Extensions;
using Catering.API.Models.Order.Request;
using Catering.API.Services.Contracts;
using Catering.Shared.DL.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;        
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        return this.FromResult(await _orderService.GetOrderOverviewAsync());
    }

    [HttpGet("Upcoming")]
    public async Task<IActionResult> GetFutureOrders()
    {
        return this.FromResult(await _orderService.GetFutureOrdersAsync());
    }

    [HttpPost("Status/Processing")]
    public async Task<IActionResult> SetOrderToProcessing([FromBody] SetOrderStatusRequest request)
    {
        request.SetStatus(OrderState.Preparing);
        return this.FromResult(await _orderService.SetOrderStatusAsync(request));
    }

    [HttpPost("Status/DeliveryReady")]
    public async Task<IActionResult> SetOrderToDeliveryReady([FromBody] SetOrderStatusRequest request)
    {
        request.SetStatus(OrderState.ReadyToDeliver);
        return this.FromResult(await _orderService.SetOrderStatusAsync(request));
    }

    [HttpPost("Status/Delivered")]
    public async Task<IActionResult> SetOrderToDelivered([FromBody] SetOrderStatusRequest request)
    {
        request.SetStatus(OrderState.Delivered);
        return this.FromResult(await _orderService.SetOrderStatusAsync(request));
    }

    [HttpPost("Status/FailedToDeliver")]
    public async Task<IActionResult> SetOrderToFailedToDeliver([FromBody] SetOrderStatusRequest request)
    {
        request.SetStatus(OrderState.FailedToBeDelivered);
        return this.FromResult(await _orderService.SetOrderStatusAsync(request));
    }
}
