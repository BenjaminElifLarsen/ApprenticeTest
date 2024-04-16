namespace UserFrontend.Frontend.Models.Order.Responses;

public class MenuResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public float Price { get; set; }
    public IEnumerable<MenuPart> Parts { get; set; }
}

public class MenuPart
{
    public string Name { get; set; }
    public uint Amount { get; set; }
}
