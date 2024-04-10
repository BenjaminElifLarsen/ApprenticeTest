namespace Catering.Shared.DL.Factories.OrderFactory;

public enum OrderFactoryErrors
{
    RequestIsNull = 0b1,
    CustomerInvalid = 0b10,
    MenuInvalid = 0b100,
    TimeInvalid = 0b1000,
}
