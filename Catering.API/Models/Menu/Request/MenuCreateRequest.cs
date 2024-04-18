namespace Catering.API.Models.Menu.Request;

public sealed class MenuCreateRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<MenuPartCreateRequest> Dishes { get; set; }
}

public sealed class MenuPartCreateRequest
{
    public Guid DishId { get; set; }
    //public string Name { get; set; }
    public float Price { get; set; }
    public ushort Amount { get; set; }
}