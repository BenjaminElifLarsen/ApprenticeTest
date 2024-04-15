using Catering.Shared.DL.Models;
using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public sealed class Menu : IAggregateRoot
{
    private Guid _id;
    private string _name;
    private string _description;
    private HashSet<MenuPart> _parts;
    private HashSet<MenuOrder> _orders;

    public Guid Id { get => _id; private set => _id = value; }
    public string Name { get => _name; private set => _name = value; }
    public string Description { get => _description; private set => _description = value; }
    public IEnumerable<MenuPart> Parts { get => _parts; private set => _parts = value.ToHashSet(); }
    public IEnumerable<MenuOrder> Orders { get => _orders; private set => _orders = value.ToHashSet(); }

    private Menu()
    {
        
    }

    internal Menu(string name, string description) // TODO: use factory to set these
    {
        _name = name;
        _description = description;
        _parts = [];
    }

    public bool AddOrder(Guid id, DateTime time)
    {
        return _orders.Add(new(id, time));
    }

    public bool AddMenuPart(MenuPart part)
    {
        if (_parts.Any(x => x.Dish == part.Dish))
            throw new Exception();
        return _parts.Add(part);
    }
}
