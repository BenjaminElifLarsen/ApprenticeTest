using CustomerFrontEnd.Models.User.Responses;

namespace CustomerFrontEnd.Services.Contracts;

public interface IUserService
{
    public Task<LoginResponse> LoginAsync(string username, string password);
}
