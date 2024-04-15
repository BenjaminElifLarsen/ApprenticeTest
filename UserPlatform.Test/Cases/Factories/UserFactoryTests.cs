using Moq;
using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Shared.DL.Factories.UserFactory;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.Helpers;
using Xunit.Abstractions;

namespace UserPlatform.Test.Cases.Factories;

public class UserFactoryTests
{
    private UserFactory _sut;
    private readonly string HashPart = "Hashed";
    public UserFactoryTests(ITestOutputHelper outputHelper)
    {
        var mock = new Mock<IPasswordHasher>();
        mock.Setup(pass => pass.Hash(It.IsAny<User>(), It.IsAny<string>()))
            .Returns((User u, string s) => HashPart + s);

        _sut = new(mock.Object);
    }

    [Fact]
    internal void Does_Valid_User_Creation_Go_Through()
    {
        // Arrange
        string dummyCity = "city";
        string dummyCompanyName = "companyName";
        string dummyStreet = "street";
        string dummyEmail = "email";
        string dummyPassword = "Test123!";
        string dummyPasswordReentered = "Test123!";
        string dummyPhone = "12345678";
        IEnumerable<UserData> dummyUsers = [];
        UserValidationData uvd = new(dummyUsers);
        UserCreationRequest usr = new()
        {
            City = dummyCity,
            CompanyName = dummyCompanyName,
            Street = dummyStreet,
            Email = dummyEmail,
            Password = dummyPassword,
            PasswordReentered = dummyPasswordReentered,
            Phone = dummyPhone,
        };

        // Act
        var actualResult = _sut.Build(usr, uvd);

        // Assert
        Assert.IsType<SuccessResult<User>>(actualResult);
        var user = actualResult.Data;
        Assert.Equal(dummyCity, user.Location.City);
        Assert.Equal(dummyStreet, user.Location.Street);
        Assert.Equal(dummyCompanyName, user.CompanyName);
        Assert.Equal(dummyEmail, user.Contact.Email);
        Assert.Equal(dummyPhone, user.Contact.Phone);
        Assert.Equal(HashPart + dummyPassword, user.Password);
    }

}
