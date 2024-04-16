using CustomerFrontEnd.Models.User.Requests;
using CustomerFrontEnd.Models.User.Responses;
using CustomerFrontEnd.Services.Contracts;
using CustomerFrontEnd.Settings;
using Microsoft.Extensions.Options;
using Shared.Service;
namespace CustomerFrontEnd.Services.UserService;

public class UserService : HttpBaseService, IUserService
{
    public UserService(IOptions<UrlEndpoint> baseUrl) : base(baseUrl.Value.Url)
    {
    }

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        LoginRequest body = new(username, password);
        HttpResponseMessage responseMessage = null!;
        using HttpRequestMessage requestMessage = new(HttpMethod.Post, "api/User/Login");
        requestMessage.AttachBody(body);
        try
        {
            responseMessage = await SendAsync(requestMessage);            
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
}
