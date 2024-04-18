using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserFrontend.Frontend.Models;
using UserFrontend.Frontend.Models.User.Requests;
using UserFrontend.Frontend.Services.Contracts;

namespace UserFrontend.Frontend.Services.UserDataService;

public sealed class UserDataService : IUserDataService
{
    private readonly IUserService _userService;
    private readonly IAuthenticationStorage _authenticationStorage;

    public UserDataService(IUserService userService, IAuthenticationStorage authenticationStorage)
    {
        _userService = userService;
        _authenticationStorage = authenticationStorage;        
    }


    public UserData? GetUserData()
    {
        var claimPrincipals = CreateClaimsPrincipalFromToken(_authenticationStorage.Token);
        return UserData.FromClaimsPrincipal(claimPrincipals);
    }

    public async Task LogoffAsync()
    {
        var refreshToken = _authenticationStorage.RefreshToken;
        _authenticationStorage.RefreshToken = null!;
        await _userService.LogoffAsync(refreshToken);
    }

    public Task RefreshTokenAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<UserData> SendAuthenticateRequestAsync(LoginRequest request)
    {
        var response = await _userService.LoginAsync(request);
        if (response is null)
        {
            return null!;
        }
        string token = response.Token;
        string refreshToken = response.RefreshToken;
        _authenticationStorage.Token = token;
        _authenticationStorage.RefreshToken = refreshToken;
        var claimsPrincipal = CreateClaimsPrincipalFromToken(token);
        var userData = UserData.FromClaimsPrincipal(claimsPrincipal);
        return userData;
    }

    public async Task<UserData> CreateUserRequestAsync(UserRequest request)
    {
        var response = await _userService.CreateUserAsync(request);
        if (response is null)
        {
            return null!;
        }
        string token = response.Token;
        string refreshToken = response.RefreshToken;
        _authenticationStorage.Token = token;
        _authenticationStorage.RefreshToken = refreshToken;
        var claimsPrincipal = CreateClaimsPrincipalFromToken(token);
        var userData = UserData.FromClaimsPrincipal(claimsPrincipal);
        return userData;
    }

    private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();
        if(tokenHandler.CanReadToken(token))
        {
            var jst = tokenHandler.ReadJwtToken(token);
            identity = new(jst.Claims, "Bearer");
        }
        return new(identity);
    }
}
