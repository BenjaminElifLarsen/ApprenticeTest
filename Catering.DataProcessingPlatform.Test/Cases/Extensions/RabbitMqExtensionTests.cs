using Catering.DataProcessingPlatform.Extensions;
using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;

namespace Catering.DataProcessingPlatform.Test.Cases.Extensions;

public class RabbitMqExtensionTests
{
    [Theory]
    [MemberData(nameof(NonGenericCarrierModels))]
    internal void Does_To_Non_Generic_Carrier_Convert_Correctly(Result result, int expectedError, CarrierResult expectedCarrierResult)
    {
        // Arrange

        // Act
        var actualResult = result.ToCarrier();

        // Assert
        Assert.NotNull(actualResult);
        Assert.Equal(expectedError, actualResult.Error);
        Assert.Equal(expectedCarrierResult, actualResult.Result);
    }

    public static IEnumerable<object[]> NonGenericCarrierModels()
    {
        yield return new object[]
        {
            new SuccessNoDataResult(),
            0,
            CarrierResult.Success,
        };

        yield return new object[]
        {
            new BadRequestResult(new(1)),
            1,
            CarrierResult.Error,
        };

        yield return new object[]
        {
            new NotFoundResult(new(2)),
            2,
            CarrierResult.Error,
        };

        yield return new object[]
        {
            new UnhandledResult(new(4)),
            4,
            CarrierResult.Error,
        };
    }
}
