namespace Catering.Shared.DL.Communication.Models;

public sealed class MenuCreationRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public IEnumerable<MenuPartCreationRequest> Parts { get; set; }
}

public sealed class MenuPartCreationRequest
{
    public Guid Id { get; set; }
    public float Price { get; set; }
    public ushort Amount { get; set; }
}
