using Catering.Shared.DL.Factories.CustomerFactory;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Communication.Models.User;
using Shared.Patterns.ResultPattern;

namespace Catering.Shared.Test.Cases.Factories;

public class CustomerFactoryTests
{
    private readonly ICustomerFactory _sut;

	public CustomerFactoryTests()
	{
		_sut = new CustomerFactory();
	}

	[Fact]
	internal void Does_Factory_Handle_Missing_CustomerId()
	{
		// Arrange
		string dummyCity = "City"!;
		string dummyStreet = "Street"!;
		string dummyName = "Name";
		UserCreationCommand request = new()
		{
			City = dummyCity,
			Street = dummyStreet,
			UserName = dummyName,
			UserId = default!,
		};

        IEnumerable<CustomerData> dummyCustomerData = [];
		CustomerValidationData dummyValidationData = new(dummyCustomerData);

		// Act
		var actualResult = _sut.Build(request, dummyValidationData);
		var actualFlag = (long)actualResult.Errors;

		// Assert
		Assert.IsType<BadRequestResult<Customer>>(actualResult);
		Assert.Equal((long)CustomerFactoryErrors.CustomerIdInvalid, actualFlag);
	}

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null!)]
	internal void Does_Factory_Handle_Missing_Street(string street)
	{
        // Arrange
        Guid dummyUserId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        string dummyCity = "City"!;
        string dummyName = "Name";
        UserCreationCommand request = new()
        {
            City = dummyCity,
            Street = street,
            UserName = dummyName,
            UserId = dummyUserId!,
        };

        IEnumerable<CustomerData> dummyCustomerData = [];
        CustomerValidationData dummyValidationData = new(dummyCustomerData);

        // Act
        var actualResult = _sut.Build(request, dummyValidationData);
        var actualFlag = (long)actualResult.Errors;

        // Assert
        Assert.IsType<BadRequestResult<Customer>>(actualResult);
        Assert.Equal((long)CustomerFactoryErrors.StreetIsInvalid, actualFlag);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null!)]
    internal void Does_Factory_Handle_Missing_City(string city)
    {
        // Arrange
        Guid dummyUserId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        string dummyStreet = "Street"!;
        string dummyName = "Name";
        UserCreationCommand request = new()
        {
            City = city,
            Street = dummyStreet,
            UserName = dummyName,
            UserId = dummyUserId!,
        };

        IEnumerable<CustomerData> dummyCustomerData = [];
        CustomerValidationData dummyValidationData = new(dummyCustomerData);

        // Act
        var actualResult = _sut.Build(request, dummyValidationData);
        var actualFlag = (long)actualResult.Errors;

        // Assert
        Assert.IsType<BadRequestResult<Customer>>(actualResult);
        Assert.Equal((long)CustomerFactoryErrors.CityIsInvalid, actualFlag);
    }

    [Fact]
    internal void Does_Factory_Handle_CustomerId_In_Use()
    {
        // Arrange
        Guid userId = Guid.Parse("dfeadf31-b99f-43a0-7cde-08dc606feca9");
        string dummyCity = "City"!;
        string dummyStreet = "Street"!;
        string dummyName = "Name";
        UserCreationCommand request = new()
        {
            City = dummyCity,
            Street = dummyStreet,
            UserName = dummyName,
            UserId = userId!,
        };

        IEnumerable<CustomerData> dummyCustomerData = [new(userId)];
        CustomerValidationData dummyValidationData = new(dummyCustomerData);

        // Act
        var actualResult = _sut.Build(request, dummyValidationData);
        var actualFlag = (long)actualResult.Errors;

        // Assert
        Assert.IsType<BadRequestResult<Customer>>(actualResult);
        Assert.Equal((long)CustomerFactoryErrors.CustomerIdUse, actualFlag);
    }
}
