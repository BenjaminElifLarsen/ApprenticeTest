using Moq;
using Serilog;
using Shared.Patterns.ResultPattern;
using System.Diagnostics;
using UserPlatform.Communication.Contracts;
using UserPlatform.Models.User.Requests;
using UserPlatform.Models.User.Responses;
using UserPlatform.Services.Contracts;
using UserPlatform.Services.UserService;
using UserPlatform.Shared.DL.Factories.UserFactory;
using UserPlatform.Shared.IPL.UnitOfWork;
using Xunit.Abstractions;
namespace UserPlatform.Test.Cases.ServiceTests.User;

public class UserServiceTests
{
    public UserServiceTests(ITestOutputHelper outputHelper)
    {
        
    }

    [Fact]
    public async Task Does_User_Login_Async_Spend_Minimum_Time()
    {
        // Arrange
        TimeSpan expectedTimespan = TimeSpan.FromSeconds(0.5);
        var securityMock = new Mock<ISecurityService>();
        var dummyUsername = "username";
        var dummyToken = "token";
        var dummyRefreshToken = "refreshToken";
        securityMock.Setup(sec => sec.AuthenticateAsync(It.IsAny<UserLoginRequest>()))
            .Returns((UserLoginRequest ulr) => Task.FromResult<Result<UserAuthResponse>>(new SuccessResult<UserAuthResponse>(new UserAuthResponse(dummyUsername, dummyToken, dummyRefreshToken)))); 
        var dummyCommunicationMock = new Mock<ICommunication>();
        var dummyUserFactoryMock = new Mock<IUserFactory>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();
        var dummyLoggerMock = new Mock<ILogger>();
        UserService sut = new(securityMock.Object, dummyCommunicationMock.Object, dummyUserFactoryMock.Object, unitOfWorkMock.Object, dummyLoggerMock.Object);
        UserLoginRequest request = new();

        // Act
        Stopwatch sw = Stopwatch.StartNew();
        _ = await sut.UserLoginAsync(request);
        sw.Stop();

        // Assert
        Assert.True(expectedTimespan <= sw.Elapsed);
    }
}
