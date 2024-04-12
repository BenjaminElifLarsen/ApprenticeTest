using Shared.Patterns.CQRS.Commands;
using Shared.Patterns.CQRS.Queries;

namespace Shared.Communication.Models;

public sealed class MenuListQueryResponse : BaseReadModel, ICommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public IEnumerable<MenuListPartQueryResponse> Parts { get; set; }

    public MenuListQueryResponse()
    {
        
    }

    public MenuListQueryResponse(Guid id, string name, string description, float price, IEnumerable<MenuListPartQueryResponse> parts)
    {
        Id = id;
        Name = name;
        Description = description;
        Parts = parts;       
        Price = price;
    }
}

public sealed class MenuListPartQueryResponse : BaseReadModel
{
    public string Name { get; set; }
    public uint Amount { get; set; }

    public MenuListPartQueryResponse()
    {
        
    }

    public MenuListPartQueryResponse(string name, uint amount)
    {
        Name = name;
        Amount = amount;        
    }
}