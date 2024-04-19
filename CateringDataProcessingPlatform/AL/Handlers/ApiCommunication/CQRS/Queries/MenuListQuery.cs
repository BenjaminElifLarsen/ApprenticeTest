using CateringDataProcessingPlatform.DL.Models;
using Shared.Communication.Models.Menu;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;

internal sealed class MenuListMenuQuery : BaseQuery<Menu, MenuListFinerDetailsMenuQueryResponse>
{
    public override Expression<Func<Menu, MenuListFinerDetailsMenuQueryResponse>> Map()
    {
        return e => new(
            e.Id,
            e.Name,
            e.Description,
            e.Parts.AsQueryable().Select(new MenuListMenuPartQuery().Map()).ToArray()
            );
    }
}

internal sealed class MenuListMenuPartQuery : BaseQuery<MenuPart, MenuListFinerDetailsMenuPartQueryResponse>
{
    public override Expression<Func<MenuPart, MenuListFinerDetailsMenuPartQueryResponse>> Map()
    {
        return e => new(e.Dish.Id, e.Amount, e.Price, e.Name);
    }
}
