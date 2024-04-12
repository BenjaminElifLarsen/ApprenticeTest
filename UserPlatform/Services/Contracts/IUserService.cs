using Shared.Patterns.ResultPattern;
using UserPlatform.Models.User.Requests;
using UserPlatform.Models.User.Responses;
using UserPlatform.Shared.Communication.Models;

namespace UserPlatform.Services.Contracts;

public interface IUserService
{
    public Task<Result<UserAuthResponse>> CreateUser(UserCreationRequest request);
    public Task<Result> UserLogin(UserLoginRequest request);
    public Task<Result> UserLogoff(Guid userId);
}
