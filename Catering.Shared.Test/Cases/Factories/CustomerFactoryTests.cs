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
}
