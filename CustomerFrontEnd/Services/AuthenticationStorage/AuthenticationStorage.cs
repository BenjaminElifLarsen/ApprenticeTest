using CustomerFrontEnd.Services.Contracts;

namespace CustomerFrontEnd.Services.AuthenticationStorage;

public class AuthenticationStorage : IAuthenticationStorage
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
