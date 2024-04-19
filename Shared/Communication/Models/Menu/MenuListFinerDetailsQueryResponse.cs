using Shared.Patterns.CQRS.Queries;

namespace Shared.Communication.Models.Menu;

public sealed class MenuListFinerDetailsQueryResponse
{
    public IEnumerable<MenuListFinerDetailsMenuQueryResponse> Parts { get; set; }

    private MenuListFinerDetailsQueryResponse()
    {
        
    }

    public MenuListFinerDetailsQueryResponse(IEnumerable<MenuListFinerDetailsMenuQueryResponse> parts)
    {
        Parts = parts;        
    }

}



public sealed class MenuListFinerDetailsMenuQueryResponse : BaseReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<MenuListFinerDetailsMenuPartQueryResponse> Parts { get; set; }

    private MenuListFinerDetailsMenuQueryResponse()
    {
        
    }

    public MenuListFinerDetailsMenuQueryResponse(Guid id, string name, string description, IEnumerable<MenuListFinerDetailsMenuPartQueryResponse> parts)
    {
        Id = id;
        Name = name;
        Description = description;
        Parts = parts;        
    }
}

public sealed class MenuListFinerDetailsMenuPartQueryResponse : BaseReadModel
{
    public Guid DishId { get; set; }
    public ushort Amount { get; set; }
    public float Price { get; set; }
    public string Name { get; set; }

    private MenuListFinerDetailsMenuPartQueryResponse()
    {
        
    }

    public MenuListFinerDetailsMenuPartQueryResponse(Guid dishId, ushort amount, float price, string name)
    {
        DishId = dishId;
        Amount = amount;
        Price = price;
        Name = name;
    }
}