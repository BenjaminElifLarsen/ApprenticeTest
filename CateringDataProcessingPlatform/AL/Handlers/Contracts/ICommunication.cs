using Shared.Communication.Models;

namespace CateringDataProcessingPlatform.AL.Handlers.Contracts;

internal interface ICommunication
{
    public OrderPlaceCommand ReceiveOrderPlaceCommand();
    public UserCreationCommand ReceiveUserCreationCommand();
}
