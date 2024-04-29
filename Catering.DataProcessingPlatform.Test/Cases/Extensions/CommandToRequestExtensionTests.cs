using Shared.Communication.Models.Dish;
using Catering.DataProcessingPlatform.Extensions;

namespace Catering.DataProcessingPlatform.Test.Cases.Extensions;

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
