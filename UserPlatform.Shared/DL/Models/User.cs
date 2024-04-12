using Shared.DDD;

namespace UserPlatform.Shared.DL.Models;

public sealed class User : IAggregateRoot
{
    private Guid _id;
    private string _companyName;
    private string _password;
    private UserContact _contact;
    private UserLocation _location;
    private HashSet<ReferenceId> _orders;
    private HashSet<ReferenceId> _refreshTokens;

    public Guid Id { get => _id; private set => _id = value; }
    public string CompanyName { get => _companyName; private set => _companyName = value; }
    public string Password { get => _password; private set => _password = value; }
    public UserContact Contact { get => _contact; private set => _contact = value; }
    public UserLocation Location { get => _location; private set => _location = value; }
    public IEnumerable<ReferenceId> Orders { get => _orders; private set => _orders = value.ToHashSet(); }
    public IEnumerable<ReferenceId> RefreshTokens { get => _refreshTokens; private set => _refreshTokens = value.ToHashSet(); }

    private User()
    {
        
    }

    internal User(string companyName, UserContact contact, UserLocation location)
    {
        _id = Guid.NewGuid();
        _companyName = companyName;
        _contact = contact;        
        _password = null!;
        _location = location;
        _orders = [];
        _refreshTokens = [];
    }

    public void SetPassword(string password)
    {
        _password = password;
    }

    public void AddOrder(Guid id)
    {
        _orders.Add(new ReferenceId(id));
    }

    public void AddRefreshToken(Guid id)
    {
        _refreshTokens.Add(new ReferenceId(id));
    }
}
