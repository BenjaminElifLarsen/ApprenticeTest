using Catering.Shared.DL.Models;
using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;

namespace Catering.Shared.DL.Factories.MenuFactory;

public sealed class MenuValidationData
{
    public IEnumerable<MenuData> UsedNames { get; private set; }
    public IEnumerable<MenuDishData> KnownDishes { get; private set; }

    public MenuValidationData(IEnumerable<MenuData> namesInUse, IEnumerable<MenuDishData> permittedDishIds)
    {
        UsedNames = namesInUse;
        KnownDishes = permittedDishIds;        
    }
}


public sealed class MenuData : BaseReadModel
{
    public string Name { get; private set; }

    public MenuData(string name)
    {
        Name = name;        
    }
}

public sealed class MenuDataQuery : BaseQuery<Menu, MenuData>
{
    public override Expression<Func<Menu, MenuData>> Map()
    {
        return e => new(e.Name);
    }
}

public sealed class MenuDishData : BaseReadModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public MenuDishData(Guid id, string name)
    {
        Id = id;
        Name = name;

    }
}

public sealed class MenuDishDataQuery : BaseQuery<Dish, MenuDishData>
{
    public override Expression<Func<Dish, MenuDishData>> Map()
    {
        return e => new(e.Id, e.Name);
    }
}
