namespace Shared.Helpers.Time;

/*
 * Exists to help with automatisk testing. Being able to prevent random time data
 */

public interface ITime
{
    public DateTime GetCurrentTimeUtc();
    public int GetDifferentBetweenUtcAndLocalTime();
}
