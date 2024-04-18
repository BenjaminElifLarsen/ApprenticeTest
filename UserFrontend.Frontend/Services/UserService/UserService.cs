using Shared.Service;
using UserFrontend.Frontend.Models.User.Requests;
using UserFrontend.Frontend.Models.User.Responses;
using UserFrontend.Frontend.Services.Contracts;
using UserFrontend.Frontend.Services.OrderService;
namespace UserFrontend.Frontend.Services.UserService;

public class UserService : IUserService
{
    private readonly HttpClient _client;
    private readonly IAuthenticationStorage _authenticationStorage;

    public UserService(HttpClient client, IAuthenticationStorage authenticationStorage)
    {
        _client = client;
        _authenticationStorage = authenticationStorage;
    }

    public async Task<LoginResponse> CreateUserAsync(UserRequest body)
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, "api/User");
        requestMessage.AttachBody(body);
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
            return responseMessage.ToModel<LoginResponse>();
        }
        return null!;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest body)
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, "api/User/Login");
        requestMessage.AttachBody(body);
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
            return responseMessage.ToModel<LoginResponse>();
        }
        return null!;
    }

    public async Task LogoffAsync(string token)
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Delete, $"api/user?token={token}");
        requestMessage.AddBearerToken(_authenticationStorage.Token);
        try
        {
            responseMessage = await _client.SendAsync(requestMessage);
        }
        catch (Exception ex)
        {
            return;
        }
        return;
    }

    public async Task UpdateAsync(UserUpdateRequest body)
    {
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Put, "api/User");
        requestMessage.AttachBody(body);
        requestMessage.AddBearerToken(_authenticationStorage.Token);
        try
        {
            responseMessage = await _client.SendAsync(requestMessage);
        }
        catch (Exception ex)
        {
            return;
        }
        return;
    }

    // TODO: refresh 
}
