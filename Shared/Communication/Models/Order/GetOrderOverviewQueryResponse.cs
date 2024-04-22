using Shared.Patterns.CQRS.Queries;

namespace Shared.Communication.Models.Order;

public sealed class GetOrderOverviewQueryResponse
{
    public IEnumerable<GetOrderOverviewPartQueryResponse> Orders { get; set; }

    public GetOrderOverviewQueryResponse()
    {
        
    }

    public GetOrderOverviewQueryResponse(IEnumerable<GetOrderOverviewPartQueryResponse> orders)
    {
        Orders = orders;
    }
}

public sealed class GetOrderOverviewPartQueryResponse : BaseReadModel
{
    public Guid OrderId { get; set; }
    public int Status { get; set; }

    public GetOrderOverviewPartQueryResponse()
    {
        
    }

    public GetOrderOverviewPartQueryResponse(Guid id, int status)
    {
        OrderId = id;
        Status = status;        
    }
}
