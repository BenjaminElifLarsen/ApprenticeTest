using Catering.Shared.DL.Factories;
using CateringDataProcessingPlatform.AL.Handlers.ApiCommunication.DataProcessing;
using CateringDataProcessingPlatform.Extensions;
using CateringDataProcessingPlatform.IPL;
using CateringDataProcessingPlatform.IPL.Appsettings;
using CateringDataProcessingPlatform.IPL.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Communication.Models.Dish;
using Shared.Communication.Models.Menu;
using Shared.Patterns.ResultPattern;
using Shared.Service;
using System.Threading;

namespace CateringDataProcessingPlatform.AL.Handlers.ApiCommunication;

internal sealed class ApiRabbitCommunication : BaseService
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private ApiRabbitDataProcessing _processing;

    public ApiRabbitCommunication(IConfigurationManager configurationManager, IContextFactory contextFactory, IFactoryCollection factoryCollection, RabbitData rabbitData, ILogger logger) : base(logger)
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

        DeclareQueueWithConsumer(CommunicationQueueNames.DISH_CREATION, ReceivedForDishCreationRPC);
        DeclareQueueWithConsumer(CommunicationQueueNames.MENU_CREATION, ReceivedForMenuCreationRPC);

        _logger.Information("{Identifier}: Initialised", _identifier);
    }

    private void DeclareQueueWithConsumer(string name, EventHandler<BasicDeliverEventArgs> handler)
    {
        _channel.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += handler;
        _channel.BasicConsume(queue: name, autoAck: false, consumer: consumer);
    }

    private void ReceivedForMenuCreationRPC(object? sender, BasicDeliverEventArgs e)
    {
        var message = e.ToMessage();
        try
        {
            var requestProps = e.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = requestProps.CorrelationId;

            var request = message.ToCommand<MenuCreationCommand>();

            if(request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {MenuCreationCOmmandType}", _identifier, message, typeof(MenuCreationCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            var body = result.ToBody();
            _channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, body: body);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        _channel.BasicAck(e.DeliveryTag, false);
    }

    private void ReceivedForDishCreationRPC(object? sender, BasicDeliverEventArgs e)
    {
        var message = e.ToMessage();
        try
        {
            var requestProps = e.BasicProperties;
            var replyProps = _channel.CreateBasicProperties();
            replyProps.CorrelationId = requestProps.CorrelationId;

            var request = message.ToCommand<DishCreationCommand>();

            if (request is null)
            {
                _logger.Error("{Identifier}: {Message} could not be parsed to {DishCreationCOmmandType}", _identifier, message, typeof(DishCreationCommand));
                _channel.BasicAck(e.DeliveryTag, false);
                return;
            }
            var result = _processing.Process(request);
            var body = result.ToBody();
            _channel.BasicPublish(exchange: string.Empty, routingKey: requestProps.ReplyTo, basicProperties: replyProps, body: body);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "{Identifier}: Error processing {Message}", _identifier, message);
        }
        throw new NotImplementedException();
    }
}
