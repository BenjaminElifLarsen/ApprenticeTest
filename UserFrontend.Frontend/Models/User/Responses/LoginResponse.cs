namespace UserFrontend.Frontend.Models.User.Responses;

public class LoginResponse
{
    public string UserName { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
