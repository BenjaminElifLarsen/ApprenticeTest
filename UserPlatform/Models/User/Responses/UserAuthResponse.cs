namespace UserPlatform.Models.User.Responses;

public sealed class UserAuthResponse
{
    public string UserName { get; private set; }
    public string Token { get; private set; }
    public string RefreshToken { get; private set; }

    public UserAuthResponse(string userName, string token, string refreshToken)
    {
        UserName = userName;
        Token = token;
        RefreshToken = refreshToken;
    }
}
