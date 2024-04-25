using Moq;
using Serilog;
using Shared.Patterns.ResultPattern;
using UserPlatform.Controllers;
using UserPlatform.Models.User.Responses;
using UserPlatform.Services.Contracts;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Test.Helpers;
using UserPlatform.Test.Helpers.Seri;
using Xunit.Abstractions;

namespace UserPlatform.Test.Cases.Controllers;

public class UserControllerUnitTests
{
    private readonly ITestOutputHelper _output;
    private readonly ILogger _logger;

    public UserControllerUnitTests(ITestOutputHelper output)
    {
        _output = output;
        _logger = SeriLoggerServiceTest.GenerateLogger(output, nameof(UserControllerUnitTests));
    }

    [Fact]
    internal async Task Does_User_Create_Async_Call_User_Service()
    {
        // Arrange
        var expectedStatusCode = 200;

        var userServiceMock = new Mock<IUserService>();
        var dummyUsername = "username";
        var dummyToken = "token";
        var dummyRefreshToken = "refreshToken";
        userServiceMock.Setup(us => us.CreateUserAsync(It.IsAny<UserCreationRequest>()))
            .Returns((UserCreationRequest ucr) => Task.FromResult<Result<UserAuthResponse>>(new SuccessResult<UserAuthResponse>(new UserAuthResponse(dummyUsername, dummyToken, dummyRefreshToken))));
        var request = new UserCreationRequest();
        var sut = new UserController(userServiceMock.Object, _logger);

        // Act
        var actualResult = await sut.CreateUserAsync(request);
        var actualStatusCode = ActionHelper.GetStatusCode(actualResult);
        var logs = LogHelper.GetLogs(_output);

        // Assert
        Assert.Equal(expectedStatusCode, actualStatusCode);
        Assert.Single(logs);
        Assert.Contains("UserController: Create user took ", logs[0]);
        userServiceMock.Verify(usm => usm.CreateUserAsync(request), Times.Once());
    }
}
