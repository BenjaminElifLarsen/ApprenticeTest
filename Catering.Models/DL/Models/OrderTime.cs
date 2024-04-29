using Shared.DDD;

namespace Catering.Shared.DL.Models;

public sealed record OrderTime : ValueObject
{
    private DateTime _time; // UTC

    public DateTime Time { get => _time; set => _time = value; }

    private OrderTime()
    {
        
    }

    internal OrderTime(DateTime time)
    {
        _time = time;        
    }
}
