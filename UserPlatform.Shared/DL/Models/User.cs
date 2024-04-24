using Shared.DDD;

namespace UserPlatform.Shared.DL.Models;

public sealed class User : IAggregateRoot
{
    private Guid _id;
    private string _companyName;
    private string _password;
    private DateTime? _lastLogin;
    private UserContact _contact;
    private UserLocation _location;
    private HashSet<ReferenceId> _refreshTokens;

    public Guid Id { get => _id; private set => _id = value; }
    public string CompanyName { get => _companyName; private set => _companyName = value; }
    public string Password { get => _password; private set => _password = value; }
    public DateTime? LastLogin { get => _lastLogin; private set => _lastLogin = value; }
    public UserContact Contact { get => _contact; private set => _contact = value; }
    public UserLocation Location { get => _location; private set => _location = value; }
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
        _refreshTokens = [];
    }

    public void SetPassword(string password)
    {
        _password = password;
    }

    public void AddRefreshToken(Guid id)
    {
        _refreshTokens.Add(new ReferenceId(id));
    }

    public bool UpdateStreet(string street)
    {
        if(string.IsNullOrWhiteSpace(street))
            return false;
        _location = new UserLocation(_location.City, street);
        return true;
    }

    public bool UpdateCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return false;
        _location = new UserLocation(city, _location.Street);
        return true;
    }

    public void SetLastLogin(DateTime time)
    {
        _lastLogin = time;
    }
}
