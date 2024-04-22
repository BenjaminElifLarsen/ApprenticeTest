using Catering.Shared.DL.Models.Enums;
using Catering.Shared.Helpers;

namespace Catering.Shared.Test.Cases.Helpers;

public class EnumHelperTests
{
    [Theory]
    [InlineData(OrderState.Unknown, true)]
    [InlineData(OrderState.Received, true)]
    [InlineData(OrderState.Preparing, true)]
    [InlineData(OrderState.ReadyToDeliver, true)]
    [InlineData(OrderState.Delivered, true)]
    [InlineData(OrderState.FailedToBeDelivered, true)]
    [InlineData((OrderState)(-1), false)]
    [InlineData((OrderState)6, false)]
    internal void Does_Is_ENum_Value_Valid_For_Order_State_Work(OrderState order, bool expectedResult)
    {
        // Arrange

        // Act
        var actualResult = EnumHelper.IsEnumValueValid(order);

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }
}
