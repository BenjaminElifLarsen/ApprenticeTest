using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Shared.Helpers.Time;
using Shared.Patterns.ResultPattern;
using UserPlatform.Communication.Contracts;
using UserPlatform.Controllers;
using UserPlatform.Models.User.Responses;
using UserPlatform.Services.Security;
using UserPlatform.Services.UserService;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Shared.DL.Factories.RefreshTokenFactory;
using UserPlatform.Shared.DL.Factories.UserFactory;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.Helpers;
using UserPlatform.Shared.IPL.Repository.Interfaces;
using UserPlatform.Shared.IPL.UnitOfWork;
using UserPlatform.Sys.Appsettings.Models;
using UserPlatform.Test.Helpers;
using UserPlatform.Test.Helpers.Seri;
using Xunit.Abstractions;

namespace UserPlatform.Test.Cases.Controllers;

public class UserControllerIntregationTests
{
    private readonly ITestOutputHelper _output;
    private readonly ILogger _logger;
    private readonly ITime _time;

    public UserControllerIntregationTests(ITestOutputHelper output)
    {
        _output = output;
        _logger = SeriLoggerServiceTest.GenerateLogger(output, nameof(UserControllerUnitTests));
        _time = new TestTime(new(2024, 1, 2), 1);
    }

    [Fact]
    internal async Task Does_User_Create_Async_Call_User_Service() // Tests everything but the ICommunication implementation
    {
        // Arrange
        var expectedStatusCode = 200;

        string expectedCompanyName = "name";
        var dummyEmail = "email";
        var dummyCity = "city";
        var dummyPhone = "phone";
        var dummyStreet = "street";
        UserContact dummmyContact = new(dummyEmail, dummyPhone);
        UserLocation dummyLocation = new(dummyCity, dummyStreet);
        User dummyUser = new(expectedCompanyName, dummmyContact, dummyLocation);

        var communicationServiceMock = new Mock<ICommunication>();
        communicationServiceMock.Setup(cs => cs.TransmitUser(It.IsAny<User>()))
            .Returns(new SuccessNoDataResult());
        
        var refreshTokenRepo = new Mock<IRefreshTokenRepository>();
        refreshTokenRepo.Setup(rfr => rfr.GetTokenAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult<RefreshToken>(null!));
        refreshTokenRepo.Setup(rfr => rfr.Create(It.IsAny<RefreshToken>()));

        var userRepoMock = new Mock<IUserRepository>();
        userRepoMock.Setup(ur => ur.GetSingleAsync(It.IsAny<Guid>()))
            .Returns(Task.FromResult(dummyUser));
        userRepoMock.Setup(ur => ur.Update(dummyUser));
        userRepoMock.Setup(ur => ur.Create(It.IsAny<User>()));
        IEnumerable<UserData> userData = [];
        userRepoMock.Setup(ur => ur.AllAsync(It.IsAny<UserDataQuery>()))
            .Returns(Task.FromResult(userData));

        var unitOfWorkMock = new Mock<IUnitOfWork>();
        unitOfWorkMock.Setup(uow => uow.RefreshTokenRepository)
            .Returns(refreshTokenRepo.Object);
        unitOfWorkMock.Setup(uwo => uwo.UserRepository).
            Returns(userRepoMock.Object);
        unitOfWorkMock.Setup(uow => uow.UserRepository.GetSingleAsync(It.IsAny<string>()))
            .Returns(Task.FromResult(dummyUser));
        
        var passwordHasherMock = new Mock<IPasswordHasher>();
        passwordHasherMock.Setup(phm => phm.Hash(It.IsAny<User>(), It.IsAny<string>()))
            .Returns("HashedPassword");
        passwordHasherMock.Setup(phm => phm.VerifyPassword(It.IsAny<User>(), It.IsAny<string>()))
            .Returns(true);
        
        JwtSettings dummyOptions = new()
        {
            Audience = "DummyAudience",
            ExpireDurationMinutes = 15,
            RefreshExpireDurationMinutes = 30,
            Issuer = "DummyIssuer",
            Key = "A474A26b71490437AA024E4FADD5B497FDFF1A8EA6FF12F6FB65AF2720B59CCF!",
            RefreshKey = "A474A26A71490447AA024E4FADD5B497FDFF1A8EA6FF12F6FB65AF2720B59CCF!",
        };
        IOptions<JwtSettings> jwtSettings = new OptionsWrapper<JwtSettings>(dummyOptions);

        var userFactory = new UserFactory(passwordHasherMock.Object);
        var tokenFactory = new RefreshTokenFactory();
        var securityService = new SecurityService(jwtSettings, unitOfWorkMock.Object, passwordHasherMock.Object, tokenFactory, _time, _logger);
        var userService = new UserService(securityService, communicationServiceMock.Object, userFactory, unitOfWorkMock.Object, _logger); ;
        var dummyPassword = "Test123!";
        var request = new UserCreationRequest()
        {
            City = dummyCity,
            CompanyName = expectedCompanyName,
            Email = dummyEmail,
            Password = dummyPassword,
            PasswordReentered = dummyPassword,
            Phone = dummyPhone,
            Street = dummyStreet,
        };
        var sut = new UserController(userService, _logger);

        // Act
        var actualResult = await sut.CreateUser(request);
        var actualStatusCode = ActionHelper.GetStatusCode(actualResult);
        var logs = LogHelper.GetLogs(_output);
        var uarResult = ActionHelper.GetData<UserAuthResponse>(actualResult);

        // Assert
        Assert.Equal(expectedStatusCode, actualStatusCode);
        Assert.NotNull(uarResult);
        Assert.Equal(expectedCompanyName, uarResult.UserName);
        Assert.Equal(2, logs.Length);
        Assert.Contains($"SecurityService: User logged in at", logs[0]);
        Assert.Contains("UserController: Create user took", logs[1]);
    }
}
