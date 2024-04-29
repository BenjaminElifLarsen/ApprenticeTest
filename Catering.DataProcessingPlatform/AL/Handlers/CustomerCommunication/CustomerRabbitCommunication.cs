using Catering.Shared.DL.Factories;
using Catering.DataProcessingPlatform.AL.Handlers.Communication.CustomerCommunication.DataProcessing;
using Catering.DataProcessingPlatform.Extensions;
using Catering.DataProcessingPlatform.IPL;
using Catering.DataProcessingPlatform.IPL.Appsettings;
using Catering.DataProcessingPlatform.IPL.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Communication.Models.User;
using Shared.Patterns.ResultPattern;
using Shared.Service;
using System.Text;
using System.Text.Json;

namespace Catering.DataProcessingPlatform.AL.Handlers.CustomerCommunication;

internal sealed class CustomerRabbitCommunication : BaseService, IDisposable // TOOD: partial
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private CustomerRabbitDataProcessing _processing;

    public CustomerRabbitCommunication(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, RabbitData rabbitData, ILogger logger) : base(logger)
    {
        _connectionFactory = new ConnectionFactory { HostName = rabbitData.Url, Port = rabbitData.Port };
        _processing = new(configurationManager, contextFactory, factoryCollection, logger);
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
        DeclareQueueWithConsumer(CommunicationQueueNames.MENU_QUERY_USER, ReceivedForMenuRPC);
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
            var request = message.ToCommand<OrderPlaceCommand>();
            if (request is null)
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
            var request = message.ToCommand<UserCreationCommand>();
            if (request is null)
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
        catch (Exception ex)
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

            var request = message.ToCommand<MenuListCommand>();

            if (request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {MenuListCommandType}", _identifier, message, typeof(MenuListCommand));
                SendReplyProcessingFailed(requestProps, replyProps);
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            Carrier carrier = new() { Data = JsonSerializer.Serialize(result), Error = 0, Result = CarrierResult.Success };
            var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(carrier));
            SendReply(requestProps, replyProps, responseBytes);
            //_channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, body: responseBytes);
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

            var request = message.ToCommand<GetOrdersCommand>();

            if (request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {GetOrdersCommandType}", _identifier, message, typeof(GetOrdersCommand));
                SendReplyProcessingFailed(requestProps, replyProps);
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            Carrier carrier = new() { Data = JsonSerializer.Serialize(result), Error = 0, Result = CarrierResult.Success };
            var responseBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(carrier));
            SendReply(requestProps, replyProps, responseBytes);
            //_channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, responseBytes);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);

    }

    private void SendReply(IBasicProperties requestProps, IBasicProperties replyProps, ReadOnlyMemory<byte> body)
    {
        _channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, body: body);
    }

    private void SendReplyProcessingFailed(IBasicProperties requestProps, IBasicProperties replyProps)
    {
        var result = new UnhandledResult(new(1));
        var body = result.ToBody();
        SendReply(requestProps, replyProps, body);
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
