using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Patterns.ResultPattern;
using Shared.Service;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using UserPlatform.Communication.Contracts;
using UserPlatform.Extensions;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Sys.Appsettings.Models;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Communication;

internal sealed class RabbitCommunication : BaseService, ICommunication, IDisposable
{
    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;
    private ILogger _logger;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<IEnumerable<MenuListQueryResponse>>> _callbackMapper = new();
    private string _replyQueueName;

    public RabbitCommunication(RabbitMqData rabbitMqData, ILogger logger)
    {
        _logger = logger;
        _connectionFactory = new ConnectionFactory { HostName = rabbitMqData.Url, Port = rabbitMqData.Port };
        _channel = null!;
        _connection = null!;
        _replyQueueName = null!;
    }

    public void Initialise()
    { // TODO: handle exceptions

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.BasicQos(0, 1, false);
        //_channel.ConfirmSelect();
        _replyQueueName = _channel.QueueDeclare().QueueName;

        DeclareQueueWithProducer(CommunicationQueueNames.ORDER_PLACEMENT);
        DeclareQueueWithProducer(CommunicationQueueNames.CUSTOMER_CREATION);
        DeclareQueueWithProducer(CommunicationQueueNames.MENU_QUERY);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var response = JsonSerializer.Deserialize<MenuListQueryResponse[]>(message)!;
            tcs.TrySetResult(response);
        };
        _channel.BasicConsume(consumer: consumer, queue: _replyQueueName, autoAck: true);

        _logger.Information("{Identifier}: Initialised", _identifier);
    }

    private void DeclareQueueWithProducer(string name)
    {
        _channel.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false);
    }

    public async Task<Result<IEnumerable<MenuListQueryResponse>>> ReceiveAllMenues(User user)
    {
        _logger.Information("{Identifier}: Querying for menues", _identifier);
        MenuListCommand mlc = new() { Id =  Guid.NewGuid() };
        IEnumerable<MenuListQueryResponse> result = null!;
        try
        {
            result = await CallAsync(mlc);
            return new SuccessResult<IEnumerable<MenuListQueryResponse>>(result);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at querying for menues - {MenuGetAllMessage}", _identifier, mlc);
            return new UnhandledResult<IEnumerable<MenuListQueryResponse>>(new BinaryFlag());
        }
    }

    private Task<IEnumerable<MenuListQueryResponse>> CallAsync(MenuListCommand command, CancellationToken cancellationToken = default)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = command.Id.ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replyQueueName;
        var message = JsonSerializer.Serialize(command);
        var messageBytes = Encoding.UTF8.GetBytes(message);
        var tcs = new TaskCompletionSource<IEnumerable<MenuListQueryResponse>>();
        _callbackMapper.TryAdd(correlationId, tcs);

        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.MENU_QUERY, basicProperties: props, body: messageBytes);

        cancellationToken.Register(() => _callbackMapper.TryRemove(correlationId, out _));
        return tcs.Task;
    }

    public Result TransmitUser(User user)
    {
        _logger.Information("{Identifier}: Transmitting user", _identifier);
        UserCreationCommand command = user.ToCommand();
        var message = JsonSerializer.Serialize(command);
        var body = Encoding.UTF8.GetBytes(message);
        try
        {
            _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.CUSTOMER_CREATION, basicProperties: null!, body: body);
            return new SuccessNoDataResult();
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at transmitting user - {UserCreateMessage}", _identifier, command);
            return new UnhandledResult(new BinaryFlag());
        }
    }

    public Result TransmitPlaceOrder(OrderPlacementRequest orderPlacementRequest)
    {
        _logger.Information("{Identifier}: Transmitting order placement", _identifier);
        OrderPlaceCommand command = orderPlacementRequest.ToCommand();
        var message = JsonSerializer.Serialize(command);
        var body = Encoding.UTF8.GetBytes(message);
        try
        {
            _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.ORDER_PLACEMENT, basicProperties: null!, body: body);
            return new SuccessNoDataResult();
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at transmitting order - {OrderPlaceMessage}", _identifier, command);
            return new UnhandledResult(new BinaryFlag());
        }
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
