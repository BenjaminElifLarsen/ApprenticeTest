using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

internal sealed record MenuPart : ValueObject
{
    private ReferenceId _dish;
    private uint _amount;

    public ReferenceId Dish { get => _dish; set => _dish = value; }
    public uint Amount { get => _amount; set => _amount = value; }

    private MenuPart()
    {
        
    }

    public MenuPart(Guid dishId, uint amount)
    {
        _dish = new ReferenceId(dishId);
        _amount = amount;        
    }
}
