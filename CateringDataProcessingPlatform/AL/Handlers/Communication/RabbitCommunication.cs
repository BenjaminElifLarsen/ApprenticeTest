using Catering.Shared.DL.Factories;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using CateringDataProcessingPlatform.IPL.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Service;
using System.Text;
using System.Text.Json;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication;

internal sealed class RabbitCommunication : BaseService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private RabbitDataProcessing _processing;
    private IContextFactory _contextFactory;
    private ILogger _logger;

    public RabbitCommunication(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, RabbitData rabbitData, ILogger logger)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory { HostName = rabbitData.Url, Port = rabbitData.Port };
        _contextFactory = contextFactory;
        _processing = new RabbitDataProcessing(configurationManager, _contextFactory, factoryCollection, logger);
        _channel = null!;
        _connection = null!;    
    }

    public void Initialise()
    {
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.BasicQos(0, 1, false);

        DeclareQueueWithConsumer(CommunicationQueueNames.ORDER_PLACEMENT, ReceivedForOrderPlacement);
        DeclareQueueWithConsumer(CommunicationQueueNames.CUSTOMER_CREATION, ReceivedForCustomerCreation);
        DeclareQueueWithConsumer(CommunicationQueueNames.MENU_QUERY, ReceivedForMenuRPC);

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
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        try
        {
            var request = JsonSerializer.Deserialize<OrderPlaceCommand>(message);
            if(request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {OrderPlaceCommandType}", _identifier, message, typeof(OrderPlaceCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            _processing.Process(request);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);
    }

    private void ReceivedForCustomerCreation(object? sender, BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        try
        {
            var request = JsonSerializer.Deserialize<UserCreationCommand>(message);
            if(request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {UserCreationCommandType}", _identifier, message, typeof(UserCreationCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            _processing.Process(request);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);
    }

    private void ReceivedForMenuRPC(object? sender, BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        try
        {
            var requestProps = e.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = requestProps.CorrelationId;

            var request = JsonSerializer.Deserialize<MenuListCommand>(message);

            if(request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {MenuListCommandType}", _identifier, message, typeof(MenuListCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result));
            _channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo/*requestId.ToString()*/, basicProperties: replyProps, body: responseBytes);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);
    }
}
