namespace UserPlatform.Shared.DL.Factories.UserFactory;

public enum UserFactoryErrors
{
    RequestIsNull = 0b1,
    CompanyNameInUse = 0b10,
    NoContactInformationSat = 0b100,
    MissingLocation = 0b1000,
    MissingPassword = 0b1_0000,
    InvalidPassword = 0b10_0000,
    NotSamePassword = 0b100_0000,
    CompanyNameNotSat = 0b1000_0000,
    LocationInvalid = 0b1_0000_0000,
    PasswordToShort = 0b10_0000_0000,
    PasswordToLong = 0b100_0000_0000,
    PasswordMissingDigit = 0b1000_0000_0000,
    PasswordMissingSpecial = 0b1_0000_0000_0000,
    PasswordMissingSmall = 0b10_0000_0000_0000,
    PasswordMissingCapital = 0b100_0000_0000_0000,
}
