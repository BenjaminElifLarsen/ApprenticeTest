namespace Catering.Shared.DL.Factories.DishFactory;

public enum DishCreationErrors
{
    RequestIsNull = 0b1,
    NameIsInvalid = 0b10,
    NameInUse = 0b100,
}
