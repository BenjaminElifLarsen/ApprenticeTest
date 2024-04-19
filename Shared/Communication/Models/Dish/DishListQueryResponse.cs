using Shared.Patterns.CQRS.Queries;

namespace Shared.Communication.Models.Dish;

public sealed class DishListQueryResponse
{
    public IEnumerable<DishListPartQueryResponse> Dishes { get; set; }

    private DishListQueryResponse()
    {
        
    }

    public DishListQueryResponse(IEnumerable<DishListPartQueryResponse> dishes)
    {
        Dishes = dishes;        
    }
}

public sealed class DishListPartQueryResponse : BaseReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Guid> MenuIds { get; set; }

    private DishListPartQueryResponse()
    {
        
    }

    public DishListPartQueryResponse(Guid id, string name, IEnumerable<Guid> menuIds)
    {
        Id = id;
        Name = name;
        MenuIds = menuIds;

    }
}