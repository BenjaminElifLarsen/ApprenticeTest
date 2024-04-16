namespace UserFrontend.Frontend.Models.Order.Requests;

public class OrderRequest
{   
    public Guid MenuId { get; set; }
    public DateTime OrderedTo { get; set; }

    public OrderRequest()
    {
        
    }

    public OrderRequest(Guid menuId, DateTime orderedTo)
    {
        MenuId = menuId;
        OrderedTo = orderedTo;
    }
}
