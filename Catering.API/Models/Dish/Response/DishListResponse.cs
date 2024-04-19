using Shared.Communication.Models.Dish;

namespace Catering.API.Models.Dish.Response;

public sealed class DishListResponse
{
    public IEnumerable<DishListPartResponse> Dishes { get; private set; }

    public DishListResponse(DishListQueryResponse response)
    {
        Dishes = response.Dishes.Select(x => new DishListPartResponse(x));        
    }
}

public sealed class DishListPartResponse
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public IEnumerable<Guid> MenuIds { get; private set; }

    public DishListPartResponse(DishListPartQueryResponse response)
    {
        Id = response.Id;
        Name = response.Name;        
        MenuIds = response.MenuIds;
    }
}