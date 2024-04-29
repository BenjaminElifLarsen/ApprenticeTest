using Catering.Shared.DL.Models;
using Shared.Communication.Models.Dish;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace Catering.DataProcessingPlatform.AL.Handlers.ApiCommunication.CQRS.Queries;

internal sealed class DishListQuery : BaseQuery<Dish, DishListPartQueryResponse>
{
    public override Expression<Func<Dish, DishListPartQueryResponse>> Map()
    {
        return e => new(e.Id, e.Name, e.Menues.Select(x => x.Id));
    }
}
