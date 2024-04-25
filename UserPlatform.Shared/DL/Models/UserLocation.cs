using Shared.DDD;

namespace UserPlatform.Shared.DL.Models;

public sealed record UserLocation : ValueObject
{
    private string _city;
    private string _street;
    
    public string City { get => _city; private set => _city = value; }
    public string Street { get => _street; private set => _street = value; }

    private UserLocation()
    {
        
    }

    internal UserLocation(string city, string street)
    {
        _city = city;
        _street = street;
    }
}
