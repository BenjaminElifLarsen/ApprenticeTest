using Shared.Patterns.CQRS.Commands;

namespace Shared.Communication.Models;

public sealed class MenuListQuery : ICommand
{
    public string Name { get; set; }
    public string Description { get; set; }
    public Guid Id { get; set; }
    public IEnumerable<MenuListPartQuery> Parts { get; set; }
}

public sealed class MenuListPartQuery
{
    public string Name { get; set; }
    public uint Amount { get; set; }
}