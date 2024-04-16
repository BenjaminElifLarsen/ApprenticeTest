using CustomerFrontEnd.Services.Contracts;

namespace CustomerFrontEnd.Services.AuthenticationStorage;

public class AuthenticationStorage : IAuthenticationStorage
{
    //private readonly ProtectedSessionStorage _protectedSessionStorage;

    //public AuthenticationStorage(ProtectedSessionStorage sessionStorage)
    //{
    //    _protectedSessionStorage = sessionStorage;     
    //}

    public string Token { get; set; }
    public string RefreshToken { get; set; }

    //public async Task<string> GetTokenAsync()
    //{
    //    return (await _protectedSessionStorage.GetAsync<string>("token")).Value!;
    //}

    //public async Task SetTokenAsync(string token)
    //{
    //    await _protectedSessionStorage.SetAsync("token", token);
    //}
}
