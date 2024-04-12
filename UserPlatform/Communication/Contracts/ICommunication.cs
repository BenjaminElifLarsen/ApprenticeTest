using Shared.Communication.Models;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Communication.Contracts;

public interface ICommunication
{
    public void TransmitUser(User user);
    public void ReceiveAllMenues(User user);
    public void TransmitPlaceOrder(OrderPlaceCommand command);
}
