using CustomerFrontEnd.Models.User.Requests;
using CustomerFrontEnd.Models.User.Responses;

namespace CustomerFrontEnd.Services.Contracts;

public interface IUserService
{
    public Task<LoginResponse> LoginAsync(LoginRequest body);
    public Task<LoginResponse> CreateUserAsync(UserRequest body);
    public Task LogoffAsync(string token);
}
