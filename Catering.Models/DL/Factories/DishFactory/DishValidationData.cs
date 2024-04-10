using CateringDataProcessingPlatform.DL.Models;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace Catering.Shared.DL.Factories.DishFactory;

public sealed class DishValidationData(IEnumerable<DishData> namesInUse)
{
    public IEnumerable<DishData> NamesInUse { get; private set; } = namesInUse;
}

public sealed class DishData : BaseReadModel
{
    public string Name { get; private set; }

    public DishData(string name)
    {
        Name = name;        
    }
}

public sealed class DishDataQuery : BaseQuery<Dish, DishData>
{
    public override Expression<Func<Dish, DishData>> Map()
    {
        return e => new(e.Name);
    }
}
