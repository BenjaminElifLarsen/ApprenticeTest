using Shared.Service;
using UserFrontend.Frontend.Models.User.Requests;
using UserFrontend.Frontend.Models.User.Responses;
using UserFrontend.Frontend.Services.Contracts;
namespace UserFrontend.Frontend.Services.UserService;

public class UserService : IUserService
{
    private readonly HttpClient _client;
    public UserService(HttpClient client)
    {
        _client = client;
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

    public Task LogoffAsync(string token)
    {
        throw new NotImplementedException();
    }
}
