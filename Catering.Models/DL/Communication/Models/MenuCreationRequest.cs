namespace Catering.Shared.DL.Communication.Models;

public sealed class MenuCreationRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<MenuPartCreation> Parts { get; set; }
}

public sealed class MenuPartCreation
{
    public Guid Id { get; set; }
    public float Price { get; set; }
    public int Amount { get; set; }
}
