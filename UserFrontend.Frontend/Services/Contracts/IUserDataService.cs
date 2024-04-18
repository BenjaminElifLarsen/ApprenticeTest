using UserFrontend.Frontend.Models;
using UserFrontend.Frontend.Models.User.Requests;

namespace UserFrontend.Frontend.Services.Contracts;

public interface IUserDataService
{
    public Task<UserData> SendAuthenticateRequestAsync(LoginRequest request);
    public Task<UserData> CreateUserRequestAsync(UserRequest request);
    public UserData? GetUserData();
    public Task LogoffAsync();
    public Task RefreshTokenAsync();
}
