using Catering.Shared.DL.Factories;
using CateringDataProcessingPlatform.AL.Handlers.Communication.DataProcessing;
using CateringDataProcessingPlatform.Extensions;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using CateringDataProcessingPlatform.IPL.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Communication.Models.User;
using Shared.Service;
using System.Text;
using System.Text.Json;

namespace CateringDataProcessingPlatform.AL.Handlers.Communication;

internal sealed class RabbitCommunication : BaseService // TOOD: partial
{
    private readonly ConnectionFactory _connectionFactory; // TODO: if making a rest api for catering, the rest api should contact the database though rabbitmq. Consider another rabbitcommuniation class for that part, so this is UserRabbitCommunication and the other is Employee... or something like that name
    private IConnection _connection;
    private IModel _channel;
    private RabbitDataProcessing _processing;
    private IContextFactory _contextFactory;

    public RabbitCommunication(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, RabbitData rabbitData, ILogger logger) : base(logger)
    {
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
        DeclareQueueWithConsumer(CommunicationQueueNames.ORDER_GET_FOR_USER, ReceivedForMenuUserRPC);
        DeclareQueueWithConsumer(CommunicationQueueNames.CUSTOMER_UPDATE, ReceivedForCustomerUpdate);

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
        var message = e.ToMessage();
        try
        {
            var request = message.ToCommand<OrderPlaceCommand>();//JsonSerializer.Deserialize<OrderPlaceCommand>(message);
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
        var message = e.ToMessage();
        try
        {
            var request = message.ToCommand<UserCreationCommand>(); //JsonSerializer.Deserialize<UserCreationCommand>(message);
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

    private void ReceivedForCustomerUpdate(object? sender, BasicDeliverEventArgs e)
    {
        var message = e.ToMessage();
        try
        {
            var request = message.ToCommand<UserUpdateCommand>();
            if (request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {UserUpdateCommandType}", _identifier, message, typeof(UserUpdateCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            _processing.Process(request);
        }
        catch(Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processsing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);
    }

    private void ReceivedForMenuRPC(object? sender, BasicDeliverEventArgs e)
    {
        var message = e.ToMessage();
        try
        {
            var requestProps = e.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = requestProps.CorrelationId;

            var request = message.ToCommand<MenuListCommand>();//JsonSerializer.Deserialize<MenuListCommand>(message);

            if(request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {MenuListCommandType}", _identifier, message, typeof(MenuListCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            Carrier carrier = new() { Data = JsonSerializer.Serialize(result), Error = 0, Result = CarrierResult.Success };
            var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(carrier));
            _channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, body: responseBytes);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);
    }

    private void ReceivedForMenuUserRPC(object? sender, BasicDeliverEventArgs e)
    {
        var message = e.ToMessage();
        try
        {
            var requestProps = e.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = requestProps.CorrelationId;

            var request = message.ToCommand<GetOrdersCommand>();//JsonSerializer.Deserialize<GetOrdersCommand>(message);

            if (request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {GetOrdersCommandType}", _identifier, message, typeof(GetOrdersCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            Carrier carrier = new() { Data = JsonSerializer.Serialize(result), Error = 0, Result = CarrierResult.Success };
            var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(carrier));
            _channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, responseBytes);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);

    }
}
