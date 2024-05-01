using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using UserFrontend.Frontend.Models;
using UserFrontend.Frontend.Models.User.Requests;
using UserFrontend.Frontend.Services.Contracts;

namespace UserFrontend.Frontend.Services.UserAuthenticationStateProvider;

public class UserAuthenticationStateProvider : AuthenticationStateProvider, IDisposable
{
    private readonly IUserDataService _userDataService;

    public UserData CurrentUser { get; private set; } = new();

    public UserAuthenticationStateProvider(IUserDataService userDataService)
    {
        _userDataService = userDataService;
        AuthenticationStateChanged += OnAuthenticationStateChangedAsync;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();
        var userData = _userDataService.GetUserData();
        if(userData is not null)
        { // TODO: use the refresh token. Not sure how to automate this really, at least not compared to Angular
            //var authenticatedUserData = await _userDataService.sen
        }
        return new(principal);
    }

    public async Task LoginAsync(LoginRequest request)
    {
        var principal = new ClaimsPrincipal();
        var userData = await _userDataService.SendAuthenticateRequestAsync(request);
        if(userData is not null)
        {
            principal = userData.ToClaimsPrincipal();
        }
        CurrentUser = userData!;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    public async Task<ResponseCarrier> CreateAsync(UserRequest request)
    {
        var principal = new ClaimsPrincipal();
        var userData = await _userDataService.CreateUserRequestAsync(request);
        if (userData is not null && userData.Data is not null)
        {
            principal = userData.Data.ToClaimsPrincipal();
        }
        CurrentUser = userData is null ? null! : userData.Data!;
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        if (userData is null)
            return null!;
        return new ResponseCarrier(userData.Error);
    }

    public async Task LogOffAsync()
    {
        await _userDataService.LogoffAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
    }

    private async void OnAuthenticationStateChangedAsync(Task<AuthenticationState> task)
    {
        var state = await task;
        if(state is not null)
        {
            CurrentUser = UserData.FromClaimsPrincipal(state.User);
        }
    }

    public void Dispose()
    {
        AuthenticationStateChanged -= OnAuthenticationStateChangedAsync;
    }
}
