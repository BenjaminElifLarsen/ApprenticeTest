using Shared.Patterns.ResultPattern;
using UserPlatform.Models.User.Requests;
using UserPlatform.Models.User.Responses;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Shared.DL.Factories.UserFactory;

namespace UserPlatform.Services.UserService;

internal sealed partial class UserService
{
    public async Task<Result<UserAuthResponse>> CreateUserAsync(UserCreationRequest request)
    {
        var userData = await _unitOfWork.UserRepository.AllAsync(new UserDataQuery());
        UserValidationData uvd = new(userData);
        var result = _userFactory.Build(request, uvd);
        if (!result)
            throw new NotImplementedException();
        var user = result.Data;
        _unitOfWork.UserRepository.Create(user);
        _unitOfWork.Commit();
        var comResult = _communication.TransmitUser(user); // TODO: what to do if it fails
        var authResult = await _securityService.AuthenticateAsync(new UserLoginRequest() { UserName = request.CompanyName, Password = request.Password });
        return authResult;
    }

    public async Task<Result<UserAuthResponse>> UserLogin(UserLoginRequest request)
    {
        return await _securityService.AuthenticateAsync(request);
    }

    public async Task<Result> UserLogoff(string token)
    {
        return await _securityService.RevokeRefreshTokenAsync(token);
    }

}
