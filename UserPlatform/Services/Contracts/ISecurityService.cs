using Shared.Patterns.ResultPattern;
using UserPlatform.Models.User.Requests;
using UserPlatform.Models.User.Responses;

namespace UserPlatform.Services.Contracts;

internal interface ISecurityService
{
    public Task<Result<UserAuthResponse>> AuthenticateAsync(UserLoginRequest request);
    public Task<Result<UserAuthResponse>> RefreshToken(RefreshTokenRequest request);
    public Task<Result> RevokeRefreshToken(string request);
}
