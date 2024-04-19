
namespace Shared.Helpers.Time;

public sealed class Time : ITime
{
    private readonly TimeZoneInfo _timeZoneInfo;

    public Time(string timeZone)
    {
        var timeZones = TimeZoneInfo.GetSystemTimeZones();
        _timeZoneInfo = timeZones.First(x => string.Equals(x.Id, timeZone));
    }


    public DateTime GetCurrentTimeUtc()
    {
        return DateTime.UtcNow;
    }

    public int GetDifferentBetweenUtcAndLocalTime()
    {
        return _timeZoneInfo.BaseUtcOffset.Hours;
    }
}
