using Shared.Communication.Models.Order;
using Shared.Communication.Models.User;

namespace CateringDataProcessingPlatform.AL.Handlers.Contracts;

internal interface ICommunication
{
    public OrderPlaceCommand ReceiveOrderPlaceCommand();
    public UserCreationCommand ReceiveUserCreationCommand();
}
