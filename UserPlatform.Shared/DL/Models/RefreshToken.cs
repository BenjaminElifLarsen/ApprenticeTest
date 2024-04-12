using Shared.DDD;

namespace UserPlatform.Shared.DL.Models;

public class RefreshToken : IAggregateRoot
{
    private Guid _id;
    private string _token;
    private bool _revoked;
    private ReferenceId _user;

    public Guid Id { get => _id; private set => _id = value; }
    public string Token { get => _token; private set => _token = value; }
    public bool Revoked { get => _revoked; private set => _revoked = value; }
    public ReferenceId User { get => _user; private set => _user = value; }

    private RefreshToken()
    {
        
    }

    public RefreshToken(Guid userId, string token)
    {
        _id = Guid.NewGuid();
        _token = token;
        _user = new(userId);
        _revoked = false;
        
    }

    public void Revoke() => _revoked = true;
}
