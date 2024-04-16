namespace CustomerFrontEnd.Services.Contracts;

public interface IAuthenticationStorage
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
