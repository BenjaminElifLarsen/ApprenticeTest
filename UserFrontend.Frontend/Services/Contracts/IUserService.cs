using UserFrontend.Frontend.Models;
using UserFrontend.Frontend.Models.User.Requests;
using UserFrontend.Frontend.Models.User.Responses;

namespace UserFrontend.Frontend.Services.Contracts;

public interface IUserService
{
    public Task<LoginResponse> LoginAsync(LoginRequest body);
    public Task<ResponseCarrier<LoginResponse>> CreateUserAsync(UserRequest body);
    public Task LogoffAsync(string token);
    public Task UpdateAsync(UserUpdateRequest body);
}
