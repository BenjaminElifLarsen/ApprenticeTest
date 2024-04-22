using CateringDataProcessingPlatform.Extensions;
using Shared.Communication.Models.Dish;

namespace CateringDataProcessingPlatform.Test.Cases.Extensions;

public class CommandToRequestExtensionTests
{
    [Fact]
    internal void Does_Dish_Creation_Command_Map_Correctly()
    {
        // Arrange
        string name = "name";
        DishCreationCommand command = new() { Name = name };

        // Act
        var actualResult = command.ToRequest();

        // Assert
        Assert.Equal(name, actualResult.Name);
    }
}
