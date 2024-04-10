namespace Catering.Shared.DL.Factories.CustomerFactory;

public enum CustomerFactoryErrors
{
    CustomerIdUse = 0b1,
    LocationIsNull = 0b10,
    StreetIsInvalid = 0b100,
    CityIsInvalid = 0b1000,
    RequestIsNull = 0b10000,
    CustomerIdInvalid = 0b100000,
}
