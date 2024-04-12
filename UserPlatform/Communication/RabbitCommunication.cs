using RabbitMQ.Client;
using Shared.Communication.Models;
using Shared.Service;
using UserPlatform.Communication.Contracts;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Sys.Appsettings.Models;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Communication;

internal sealed class RabbitCommunication : BaseService, ICommunication
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private ILogger _logger;

    public RabbitCommunication(RabbitMqData rabbitMqData, ILogger logger)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory { HostName = rabbitMqData.Url, Port = rabbitMqData.Port };
        _channel = null!;
        _connection = null!;        
    }

    public void Initialise()
    { // TODO: handle exceptions

        _logger.Information("{Identifier}: Initialised", _identifier);
    }

    public void ReceiveAllMenues(User user)
    {
        throw new NotImplementedException();
    }

    public void TransmitPlaceOrder(OrderPlaceCommand command)
    {
        throw new NotImplementedException();
    }

    public void TransmitUser(User user)
    {
        throw new NotImplementedException();
    }
}
