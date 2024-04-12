using Shared.Patterns.ResultPattern;
using UserPlatform.Models.User.Requests;
using UserPlatform.Models.User.Responses;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Shared.DL.Factories;

namespace UserPlatform.Services.UserService;

internal sealed partial class UserService
{
    public async Task<Result<UserAuthResponse>> CreateUser(UserCreationRequest request)
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
        throw new NotImplementedException();
    }

    public Task<Result> UserLogin(UserLoginRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Result> UserLogoff(Guid userId)
    {
        throw new NotImplementedException();
    }

}
