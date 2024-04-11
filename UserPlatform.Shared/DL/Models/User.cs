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

    public Guid Id { get => _id; private set => _id = value; }
    public string CompanyName { get => _companyName; private set => _companyName = value; }
    public string Password { get => _password; private set => _password = value; }
    public UserContact Contact { get => _contact; private set => _contact = value; }
    public UserLocation Location { get => _location; private set => _location = value; }
    public IEnumerable<ReferenceId> Orders { get => _orders; private set => _orders = value.ToHashSet(); }

    private User()
    {
        
    }

    internal User(string companyName, UserContact contact, UserLocation location, string password)
    {
        _id = Guid.NewGuid();
        _companyName = companyName;
        _contact = contact;        
        _password = password;
        _location = location;
        _orders = [];
    }

    public void AddOrder(Guid id)
    {
        _orders.Add(new ReferenceId(id));
    }
}
