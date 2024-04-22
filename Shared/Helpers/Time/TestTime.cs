
namespace Shared.Helpers.Time;

public class TestTime : ITime
{
    private DateTime _time;
    private int _utcDifference;

    public TestTime(DateTime time, int utcDifferent)
    {
        _time = time;
        
    }

    public DateTime GetCurrentTimeUtc()
    {
        return _time.ToUniversalTime();
    }

    public int GetDifferentBetweenUtcAndLocalTime()
    {
        return _utcDifference;
    }
}
