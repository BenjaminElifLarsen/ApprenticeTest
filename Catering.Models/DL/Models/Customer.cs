using Catering.Models.DL.Models;
using Shared.DDD;

namespace Catering.Shared.DL.Models;

public class Customer : IAggregateRoot
{
    private Guid _id;
    private string _customerName;
    private CustomerLocation _location;
    private HashSet<ReferenceId> _orders;

    public Guid Id { get => _id; private set => _id = value; }
    public string CustomerName { get => _customerName; private set => _customerName = value; }
    public CustomerLocation Location { get => _location; private set => _location = value; }
    public IEnumerable<ReferenceId> Orders { get => _orders; private set => _orders = value.ToHashSet(); }

    private Customer()
    {
        
    }

    internal Customer(Guid customerId, CustomerLocation location, string customerName)
    {
        _customerName = customerName;
        _id = customerId;
        _location = location;
        _orders = [];
    }

    public bool AddOrder(Guid id)
    {
        return _orders.Add(new(id));
    }

    public bool UpdateStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
            return false;
        _location = new CustomerLocation(_location.City, street);
        return true;
    }

    public bool UpdateCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
            return false;
        _location = new CustomerLocation(city, _location.Street);
        return true;
    }
}
