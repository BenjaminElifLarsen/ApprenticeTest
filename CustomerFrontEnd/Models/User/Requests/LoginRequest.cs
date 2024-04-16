namespace CustomerFrontEnd.Models.User.Requests;

public class LoginRequest
{
    public string UserName { get; private set; }
    public string Password { get; private set; }

    public LoginRequest(string username, string password)
    {
        UserName = username;
        Password = password;        
    }
}
