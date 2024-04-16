using CustomerFrontEnd.Services.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace CustomerFrontEnd.Services;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly IAuthenticationStorage _authenticationStorage;

    public CustomAuthStateProvider(IAuthenticationStorage authenticationStorage)
    {
        _authenticationStorage = authenticationStorage;        
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = _authenticationStorage.Token;

        var identify = new ClaimsIdentity(token);
        
        if(!string.IsNullOrWhiteSpace(token))
        {
            var payload = token.Split('.')[1];
            var bytes = Convert.FromBase64String(payload);
            var keyValues = JsonSerializer.Deserialize<Dictionary<string, object>>(bytes)!;
            identify = new ClaimsIdentity(keyValues.Select(x => new Claim(x.Key, x.Value.ToString()!)));
        }
        var user = new ClaimsPrincipal(identify);
        var state = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(state));
        return state;
    }
}
