using UserFrontend.Frontend.Models.User.Requests;
using UserFrontend.Frontend.Models.User.Responses;

namespace UserFrontend.Frontend.Services.Contracts;

public interface IUserService
{
    public Task<LoginResponse> LoginAsync(LoginRequest body);
    public Task<LoginResponse> CreateUserAsync(UserRequest body);
    public Task LogoffAsync(string token);
}
