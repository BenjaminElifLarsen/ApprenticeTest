using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public sealed record MenuPart : ValueObject
{
    private ReferenceId _dish;
    private ushort _amount;
    private float _price;
    private string _name;

    public ReferenceId Dish { get => _dish; set => _dish = value; }
    public ushort Amount { get => _amount; set => _amount = value; }
    public float Price { get => _price; set => _price = value; }
    public string Name { get => _name; set => _name = value; }

    private MenuPart()
    {
        
    }

    internal MenuPart(Guid dishId, ushort amount, float price, string name)
    {
        _dish = new ReferenceId(dishId);
        _amount = amount;        
        _price = price;
        _name = name;
    }
}
