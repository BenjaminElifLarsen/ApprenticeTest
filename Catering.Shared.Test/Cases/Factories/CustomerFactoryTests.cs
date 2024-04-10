using Catering.Shared.DL.Factories.CustomerFactory;
using CateringDataProcessingPlatform.DL.Models;
using Shared.Communication.Models;
using Shared.Patterns.ResultPattern.BadRequest;

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
		UserCreationCommand request = new()
		{
			City = dummyCity,
			Street = dummyStreet,
			UserId = default!,
		};

		IEnumerable<CustomerData> dummyCustomerData = Array.Empty<CustomerData>();
		CustomerValidationData dummyValidationData = new(dummyCustomerData);

		// Act
		var actualResult = _sut.Build(request, dummyValidationData);
		var actualFlag = (long)actualResult.Errors;

		// Assert
		Assert.IsType<BadRequestResult<Customer>>(actualResult);
		Assert.Equal((long)CustomerFactoryErrors.CustomerIdInvalid, actualFlag);
	}
}
