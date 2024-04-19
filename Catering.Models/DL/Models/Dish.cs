using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public sealed class Dish : IAggregateRoot
{
    private Guid _id;
    private string _name;
    private HashSet<ReferenceId> _menues;

    public Guid Id { get => _id; private set => _id = value; }
    public string Name { get => _name; private set => _name = value; }
    public IEnumerable<ReferenceId> Menues { get => _menues; private set => _menues = value.ToHashSet(); }

    private Dish() // EF
    {
        
    }

    internal Dish(string name)
    {
        _name = name;
        _id = Guid.NewGuid();
        _menues = [];        
    }

    public void AddMenu(Guid id)
    {
        _menues.Add(new(id));
    }
}
