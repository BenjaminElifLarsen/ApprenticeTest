using Shared.Communication.Models.Menu;

namespace Catering.API.Models.Menu.Response;

public sealed class MenuListResponse
{
    public IEnumerable<MenuListDetailsResponse> Menues { get; private set; }
    public MenuListResponse(MenuListFinerDetailsQueryResponse response)
    {
        Menues = response.Parts.Select(x => new MenuListDetailsResponse(x));
    }
}

public sealed class MenuListDetailsResponse
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public IEnumerable<MenuListDetailsPartResponse> Parts { get; private set; }

    public MenuListDetailsResponse(MenuListFinerDetailsMenuQueryResponse response)
    {
        Id = response.Id;
        Name = response.Name;
        Description = response.Description;
        Parts = response.Parts.Select(x => new MenuListDetailsPartResponse(x));
    }
}

public sealed class MenuListDetailsPartResponse
{
    public Guid DishId { get; private set; }
    public ushort Amount { get; private set; }
    public float Price { get; private set; }
    public string Name { get; private set; }

    public MenuListDetailsPartResponse(MenuListFinerDetailsMenuPartQueryResponse response)
    {
        DishId = response.DishId;
        Amount = response.Amount;
        Price = response.Price;
        Name = response.Name;
    }
}
