namespace UserPlatform.Models.User.Requests;

public sealed class UserLoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
