using Shared.Patterns.ResultPattern;
using System.Diagnostics;
using UserPlatform.Models.Internal;
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
        var comResult = _communication.TransmitUser(user); // TODO: what to do if it fails. Sage handling? Not sure if got the time to handle that. Mayhaps a single case? 
        var authResult = await _securityService.AuthenticateAsync(new UserLoginRequest() { UserName = request.CompanyName, Password = request.Password });
        return authResult;
    }

    public async Task<Result<UserAuthResponse>> UserLoginAsync(UserLoginRequest request)
    {
        TimeSpan sleepLength = TimeSpan.FromSeconds(0.5);
        Stopwatch sw = new();
        sw.Start();
        var result = await _securityService.AuthenticateAsync(request);
        sw.Stop();
        var timePassed = sw.Elapsed; 
        var missingTime = sleepLength - timePassed;
        if(missingTime.TotalMilliseconds > 0)
            Thread.Sleep(missingTime);
        return result;
    }

    public async Task<Result> UserLogoff(string token)
    {
        return await _securityService.RevokeRefreshTokenAsync(token);
    }

    public async Task<Result<UserAuthResponse>> RefreshTokenAsync(RefreshTokenRequest token)
    {
        return await _securityService.RefreshTokenAsync(token);
    }

    public async Task<Result> UpdateUserAsync(UserUpdateDTO request)
    {
        var user = await _unitOfWork.UserRepository.GetSingleAsync(request.Id);
        if (user is null)
        {
            return new NotFoundResult(new());
        }
        if (request.Street is not null && !string.IsNullOrWhiteSpace(request.Street.Data))
            user.UpdateStreet(request.Street.Data);
        if (request.City is not null && !string.IsNullOrWhiteSpace(request.City.Data))
            user.UpdateCity(request.City.Data);
        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.Commit();
        var comResult = _communication.TransmitUserChanges(request);
        return comResult;
    }

}
