using CateringDataProcessingPlatform.DL.Models;
using Shared.Helpers.Time;

namespace Catering.Shared.Test.Cases.Models;

public class OrderTests
{
    public OrderTests()
    {
    }

    [Theory]
    [MemberData(nameof(PreparingModels))]
    internal void Does_Order_Handle_Set_Status_To_Preparing(Order sut, bool expectedResult)
    {
        // Arrange

        // Act
        var actualResult = sut.Preparing();

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [MemberData(nameof(DeliverReadyModels))]
    internal void Does_Order_Handle_Set_Status_To_Deliver_Ready(Order sut, bool expectedResult)
    {
        // Arrange

        // Act
        var actualResult = sut.DeliverReady();

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [MemberData(nameof(DeliveredModels))]
    internal void Does_Order_Handle_Set_Status_To_Delivered(Order sut, bool expectedResult)
    {
        // Arrange

        // Act
        var actualResult = sut.Delivered();

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    [Theory]
    [MemberData(nameof(DeliveredFailedModels))]
    internal void Does_Order_Handle_Set_Status_To_Delivery_Failed(Order sut, bool expectedResult)
    {
        // Arrange

        // Act
        var actualResult = sut.FailedToDeliver();

        // Assert
        Assert.Equal(expectedResult, actualResult);
    }

    //internal void Does_Order_Handle_Set_Status_To_

    public static IEnumerable<object[]> PreparingModels()
    {
        ITime time = new TestTime(new(2024, 1, 2), 1);
        OrderTime dummyTime = new(time.GetCurrentTimeUtc());
        Guid dummyCustomerId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        Guid dummyMenuId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606fecaA");

        Order order1 = new(dummyTime, dummyCustomerId, dummyMenuId);
        yield return new object[]
        {
            order1,
            true,
        };

        Order order5 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order5.Preparing();
        yield return new object[]
        {
            order5,
            false,
        };

        Order order2 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order2.Preparing();
        order2.DeliverReady();
        yield return new object[]
        {
            order2,
            false,
        };

        Order order3 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order3.Preparing();
        order3.DeliverReady();
        order3.Delivered();
        yield return new object[]
        {
            order3,
            false,
        };

        Order order4 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order4.Preparing();
        order4.DeliverReady();
        order4.FailedToDeliver(); // TODO: if done in more than just this class, create helper methods to set to specific states
        yield return new object[]
        {
            order4,
            false,
        };
    }

    public static IEnumerable<object[]> DeliverReadyModels()
    {
        ITime time = new TestTime(new(2024, 1, 2), 1);
        OrderTime dummyTime = new(time.GetCurrentTimeUtc());
        Guid dummyCustomerId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        Guid dummyMenuId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606fecaA");

        Order order1 = new(dummyTime, dummyCustomerId, dummyMenuId);
        yield return new object[]
        {
            order1,
            false,
        };

        Order order5 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order5.Preparing();
        yield return new object[]
        {
            order5,
            true,
        };


        Order order2 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order2.Preparing();
        order2.DeliverReady();
        yield return new object[]
        {
            order2,
            false,
        };

        Order order3 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order3.Preparing();
        order3.DeliverReady();
        order3.Delivered();
        yield return new object[]
        {
            order3,
            false,
        };

        Order order4 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order4.Preparing();
        order4.DeliverReady();
        order4.FailedToDeliver();
        yield return new object[]
        {
            order4,
            false,
        };
    }

    public static IEnumerable<object[]> DeliveredModels()
    {
        ITime time = new TestTime(new(2024, 1, 2), 1);
        OrderTime dummyTime = new(time.GetCurrentTimeUtc());
        Guid dummyCustomerId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        Guid dummyMenuId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606fecaA");

        Order order1 = new(dummyTime, dummyCustomerId, dummyMenuId);
        yield return new object[]
        {
            order1,
            false,
        };

        Order order5 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order5.Preparing();
        yield return new object[]
        {
            order5,
            false,
        };


        Order order2 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order2.Preparing();
        order2.DeliverReady();
        yield return new object[]
        {
            order2,
            true,
        };

        Order order3 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order3.Preparing();
        order3.DeliverReady();
        order3.Delivered();
        yield return new object[]
        {
            order3,
            false,
        };

        Order order4 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order4.Preparing();
        order4.DeliverReady();
        order4.FailedToDeliver();
        yield return new object[]
        {
            order4,
            false,
        };
    }

    public static IEnumerable<object[]> DeliveredFailedModels()
    {
        ITime time = new TestTime(new(2024, 1, 2), 1);
        OrderTime dummyTime = new(time.GetCurrentTimeUtc());
        Guid dummyCustomerId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        Guid dummyMenuId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606fecaA");

        Order order1 = new(dummyTime, dummyCustomerId, dummyMenuId);
        yield return new object[]
        {
            order1,
            false,
        };

        Order order5 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order5.Preparing();
        yield return new object[]
        {
            order5,
            false,
        };


        Order order2 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order2.Preparing();
        order2.DeliverReady();
        yield return new object[]
        {
            order2,
            true,
        };

        Order order3 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order3.Preparing();
        order3.DeliverReady();
        order3.Delivered();
        yield return new object[]
        {
            order3,
            false,
        };

        Order order4 = new(dummyTime, dummyCustomerId, dummyMenuId);
        order4.Preparing();
        order4.DeliverReady();
        order4.FailedToDeliver();
        yield return new object[]
        {
            order4,
            false,
        };
    }
}
