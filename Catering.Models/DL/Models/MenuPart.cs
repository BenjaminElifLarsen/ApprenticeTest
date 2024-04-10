using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

public sealed record MenuPart : ValueObject
{
    private ReferenceId _dish;
    private uint _amount;
    private float _price;

    public ReferenceId Dish { get => _dish; set => _dish = value; }
    public uint Amount { get => _amount; set => _amount = value; }
    public float Price { get => _amount; set => _price = value; }

    private MenuPart()
    {
        
    }

    internal MenuPart(Guid dishId, uint amount, float price)
    {
        _dish = new ReferenceId(dishId);
        _amount = amount;        
        _price = price;
    }
}
