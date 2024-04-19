using Shared.Patterns.ResultPattern;
using UserPlatform.Models.User.Requests;
using UserPlatform.Models.User.Responses;

namespace UserPlatform.Services.Security;

internal sealed partial class SecurityService
{
    public async Task<Result<UserAuthResponse>> AuthenticateAsync(UserLoginRequest request)
    {
        var user = await _unitOfWork.UserRepository.GetSingleAsync(request.UserName);
        if(!_passwordHasher.VerifyPassword(user, request.Password))
        {
            return new InvalidAuthentication<UserAuthResponse>();
        }
        var preexistingToken = await _unitOfWork.RefreshTokenRepository.GetTokenAsync(user.Id);
        if(preexistingToken is not null)
        {
            _unitOfWork.RefreshTokenRepository.Delete(preexistingToken);
        }

        var token = CreateToken(user);
        var refreshToken = CreateRefreshToken(user);
        var rt = _refreshTokenFactory.Build(user.Id, refreshToken);
        user.AddRefreshToken(rt.Id);
        user.SetLastLogin(_time.GetCurrentTimeUtc());
        _unitOfWork.RefreshTokenRepository.Create(rt);
        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.Commit();
        UserAuthResponse response = new(user.CompanyName, token, refreshToken);
        _logger.Information("{Identifier}: User logged in at {Time}", _identifier, _time.GetCurrentTimeUtc());
        return new SuccessResult<UserAuthResponse>(response);
    }

    public async Task<Result<UserAuthResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var foundToken = await _unitOfWork.RefreshTokenRepository.GetTokenAsync(request.Token);
        if(foundToken is null)
        {
            return new InvalidAuthentication<UserAuthResponse>();
        }
        var tokenData = _jwtHandler.ReadJwtToken(request.Token);
        var exp = tokenData.ValidTo;
        if(exp <= _time.GetCurrentTimeUtc())
        {
            await RevokeRefreshTokenAsync(request.Token);
            return new InvalidAuthentication<UserAuthResponse>();
        }

        var userId = tokenData.Subject;
        if (userId is null) // TODO: something would be wrong with the token
        {
            return new InvalidAuthentication<UserAuthResponse>();
        }

        var user = await _unitOfWork.UserRepository.GetSingleAsync(Guid.Parse(userId));
        if (user is null) // TODO: something would be wrong with the token
        {
            return new InvalidAuthentication<UserAuthResponse>();
        }

        var token = CreateToken(user);
        var refreshToken = CreateRefreshToken(user);
        var rt = _refreshTokenFactory.Build(user.Id, refreshToken);
        user.AddRefreshToken(rt.Id);
        _unitOfWork.RefreshTokenRepository.Create(rt);
        _unitOfWork.UserRepository.Update(user);
        _unitOfWork.Commit();

        UserAuthResponse response = new(user.CompanyName, token, refreshToken);
        _logger.Information("{Identifier}: User refreshed token at {Time}", _identifier, _time.GetCurrentTimeUtc());
        return new SuccessResult<UserAuthResponse>(response);
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token)
    {
        var foundToken = await _unitOfWork.RefreshTokenRepository.GetTokenAsync(token);
        if(foundToken is not null) 
        {
            foundToken.Revoke();
            _unitOfWork.RefreshTokenRepository.Delete(foundToken);
            _unitOfWork.Commit();
        }
        _logger.Information("{Identifier}: Revoked token at {Time}", _identifier, _time.GetCurrentTimeUtc());
        return new SuccessNoDataResult();
    }
}
