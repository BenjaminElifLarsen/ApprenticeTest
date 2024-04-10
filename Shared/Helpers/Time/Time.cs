
namespace Shared.Helpers.Time;

public sealed class Time : ITime
{
    public DateTime GetCurrentTimeUtc()
    {
        return DateTime.UtcNow;
    }
}
