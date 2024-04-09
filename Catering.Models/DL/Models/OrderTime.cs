using Shared.DDD;

namespace CateringDataProcessingPlatform.DL.Models;

internal sealed record OrderTime : ValueObject
{
    private DateTime _time; // UTC

    public DateTime Time { get => _time; set => _time = value; }

    private OrderTime()
    {
        
    }

    public OrderTime(DateTime time)
    {
        _time = time;        
    }
}
