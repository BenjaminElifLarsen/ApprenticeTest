using Catering.Shared.IPL.UnitOfWork;
using CateringDataProcessingPlatform.AL.Handlers.Contracts;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using CateringDataProcessingPlatform.IPL.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Shared.Communication.Models;
using Shared.Service;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication;

internal sealed class RabbitCommunication : BaseService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private RabbitDataProcessing _processing;
    private IConfigurationManager _configurationManager;
    private IContextFactory _contextFactory;
    private ILogger _logger;

    public RabbitCommunication(IConfigurationManager configurationManager, IContextFactory contextFactory, RabbitData rabbitData, ILogger logger)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory { HostName = rabbitData.Url, Port = rabbitData.Port };
        _configurationManager = configurationManager;
        _contextFactory = contextFactory;
        _processing = new RabbitDataProcessing(configurationManager, _contextFactory, logger);
        _channel = null!;
        _connection = null!;    
    }

    public void Initialise()
    {
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.BasicQos(0, 1, false);

        DeclareQueueWithConsumer("OrderPlacement", ReceivedForOrderPlacement);
        DeclareQueueWithConsumer("CustomerCreation", ReceivedForCustomerCreation);
        DeclareQueueWithConsumer("MenuQuery", ReceivedForMenuRPC);

        _logger.Information("{Identifier}: Initialised", _identifier);
    }

    private void DeclareQueueWithConsumer(string name, EventHandler<BasicDeliverEventArgs> handler)
    {
        _channel.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += handler;
        _channel.BasicConsume(queue: name, autoAck: false, consumer: consumer);
    }

    private void ReceivedForOrderPlacement(object? sender, BasicDeliverEventArgs e)
    {

    }

    private void ReceivedForCustomerCreation(object? sender, BasicDeliverEventArgs e)
    {

    }

    private void ReceivedForMenuRPC(object? sender, BasicDeliverEventArgs e)
    {

    }

    //public OrderPlaceCommand ReceiveOrderPlaceCommand()
    //{
    //    throw new NotImplementedException();
    //}

    //public UserCreationCommand ReceiveUserCreationCommand()
    //{
    //    throw new NotImplementedException();
    //}
}
