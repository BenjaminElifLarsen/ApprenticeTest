namespace Catering.Shared.DL.Factories.MenuFactory;

public enum MenuFactoryErrors
{
    RequestIsNull = 0b1,
    NameInUse = 0b10,
    NameIsNotValid = 0b100,
    OneOrMoreDishesDoNotExist = 0b1000,
    OneOrMoreDishesDoNotHaveAPrice = 0b10000,
    OneOrMoreDishesHaveInvalidAmount = 0b100000,
    NoDishes = 0b1000000,
}
