using CateringDataProcessingPlatform.DL.Models;
using Shared.Communication.Models.Menu;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication.CQRS.Queries;

internal class MenuListQuery : BaseQuery<Menu, MenuListQueryResponse>
{
    public override Expression<Func<Menu, MenuListQueryResponse>> Map()
    {
        return e => new(
            e.Id,
            e.Name,
            e.Description,
            e.Parts.Select(x => x.Price).Sum(),
            e.Parts.AsQueryable().Select(new MenuListPartQuery().Map()).ToArray()
        );
    }
}

internal class MenuListPartQuery : BaseQuery<MenuPart, MenuListPartQueryResponse>
{
    public override Expression<Func<MenuPart, MenuListPartQueryResponse>> Map()
    {
        return e => new(e.Name, e.Amount);
    }
}


