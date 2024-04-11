using Shared.DDD;

namespace UserPlatform.Shared.DL.Models;

public sealed record UserContact : ValueObject
{
    private string? _email;
    private string? _phone;

    public string? Email { get => _email; private set => _email = value; }
    public string? Phone { get => _phone; private set => _phone = value; }

    private UserContact()
    {
        
    }

    public UserContact(string? email, string? phone)
    {
        _email = email;
        _phone = phone;        
    }
}
