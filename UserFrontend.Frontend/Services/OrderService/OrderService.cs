using Shared.Patterns.ResultPattern;
using Shared.Service;
using UserFrontend.Frontend.Models.Order.Requests;
using UserFrontend.Frontend.Models.Order.Responses;
using UserFrontend.Frontend.Services.Contracts;

namespace UserFrontend.Frontend.Services.OrderService;

public class OrderService : IOrderService
{
    private readonly HttpClient _client;
    private readonly IAuthenticationStorage _authenticationStorage;
    public OrderService(HttpClient client, IAuthenticationStorage authenticationStorage)
    {
        _client = client;
        _authenticationStorage = authenticationStorage;
    }

    public async Task<Result<IEnumerable<MenuResponse>>> GetMenuesAsync()
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "api/Order/Menues");
        requestMessage.AddBearerToken(_authenticationStorage.Token);
        try
        {
            responseMessage = await _client.SendAsync(requestMessage);
        }
        catch (Exception ex)
        {
            return new BadRequestResult<IEnumerable<MenuResponse>>(new());
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            return new SuccessResult<IEnumerable<MenuResponse>>(responseMessage.ToModel<IEnumerable<MenuResponse>>());
        }
        return new BadRequestResult<IEnumerable<MenuResponse>>(new());
    }

    public async Task<Result<OrderCollectionResponse>> GetOrders()
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, "api/Order/Orders");
        requestMessage.AddBearerToken(_authenticationStorage.Token);
        try
        {
            responseMessage = await _client.SendAsync(requestMessage);
        }
        catch (Exception ex)
        {
            return null!;
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            return new SuccessResult<OrderCollectionResponse>(responseMessage.ToModel<OrderCollectionResponse>());
        }
        return null!;
    }

    public async Task<Result> PlaceOrderAsync(OrderRequest body)
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, "api/Order");
        requestMessage.AttachBody(body);
        requestMessage.AddBearerToken(_authenticationStorage.Token);
        try
        {
            responseMessage = await _client.SendAsync(requestMessage);
        }
        catch (Exception ex)
        {
            return new BadRequestResult(new());
        }

        if (responseMessage.IsSuccessStatusCode)
        {
            return new SuccessNoDataResult();
        }
        return new BadRequestResult(new());
    }
}

public static class Helper
{
    public static void AddBearerToken(this HttpRequestMessage requestMessage, string token)
    {
        requestMessage.Headers.Authorization = new("Bearer", token);
    }
}
