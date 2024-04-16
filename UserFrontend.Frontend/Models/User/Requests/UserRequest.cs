namespace UserFrontend.Frontend.Models.User.Requests;

public class UserRequest
{
    public string CompanyName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Password { get; set; }
    public string PasswordReentered { get; set; }
}
