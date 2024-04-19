using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Communication.Models.User;
using Shared.Patterns.ResultPattern;
using Shared.Service;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;
using UserPlatform.Communication.Contracts;
using UserPlatform.Extensions;
using UserPlatform.Models.Internal;
using UserPlatform.Models.Order.Requests;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Sys.Appsettings.Models;
using ILogger = Serilog.ILogger;

namespace UserPlatform.Communication;

internal sealed class RabbitCommunication : BaseService, ICommunication, IDisposable  // TOOD: partial
{
    private readonly ConnectionFactory _connectionFactory;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<IEnumerable<MenuListQueryResponse>>> _callbackMapperGetMenues = new();
    private readonly ConcurrentDictionary<string, TaskCompletionSource<GetOrdersQueryResponse>> _callbackMapperGetOrdersForUser = new();

    private IConnection _connection;
    private IModel _channel;

    private string _replyQueueNameGetMenues;
    private string _replyQueueNameGetOrdersForUser;

    public RabbitCommunication(RabbitMqData rabbitMqData, ILogger logger) : base(logger)
    {
        _connectionFactory = new ConnectionFactory { HostName = rabbitMqData.Url, Port = rabbitMqData.Port };
        _channel = null!;
        _connection = null!;
        _replyQueueNameGetMenues = null!;
        _replyQueueNameGetOrdersForUser = null!;
    }

    public void Initialise()
    { // TODO: handle exceptions

        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.BasicQos(0, 1, false);
        //_channel.ConfirmSelect();
        _replyQueueNameGetMenues = _channel.QueueDeclare().QueueName; // Need to be done like this, attempting to set it to the actual queue's name has failed badly. Took a check in the documentation and just how it nees to be done
        _replyQueueNameGetOrdersForUser = _channel.QueueDeclare().QueueName;

        DeclareQueueWithProducer(CommunicationQueueNames.ORDER_PLACEMENT, out _);
        DeclareQueueWithProducer(CommunicationQueueNames.CUSTOMER_CREATION, out _);
        DeclareQueueWithProducer(CommunicationQueueNames.MENU_QUERY_USER, out _);
        DeclareQueueWithProducer(CommunicationQueueNames.ORDER_GET_FOR_USER, out _);
        DeclareQueueWithProducer(CommunicationQueueNames.CUSTOMER_UPDATE, out _);

        SetGetMenuesConsumer();
        SetGetOrdersForUser();

        _logger.Information("{Identifier}: Initialised", _identifier);
    }

    private void SetGetMenuesConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackMapperGetMenues.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var fullMessage = JsonSerializer.Deserialize<Carrier>(message)!;
            if(fullMessage.Result is not CarrierResult.Success)
            {
                _logger.Warning("{Identifier}: Failed at getting menues - {Errors}", _identifier, Convert.ToString(fullMessage.Error, 2));
                tcs.SetResult([]);
                return;
            }
            var response = JsonSerializer.Deserialize<MenuListQueryResponse[]>(fullMessage.Data!)!;
            tcs.TrySetResult(response);
        };
        _channel.BasicConsume(consumer: consumer, queue: _replyQueueNameGetMenues, autoAck: true);
    }

    private void SetGetOrdersForUser()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackMapperGetOrdersForUser.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var fullMessage = JsonSerializer.Deserialize<Carrier>(message)!;
            if (fullMessage.Result is not CarrierResult.Success)
            {
                _logger.Warning("{Identifier}: Failed at getting orders - {Errors}", _identifier, Convert.ToString(fullMessage.Error, 2));
                tcs.SetResult(new GetOrdersQueryResponse() { Orders = [] });
                return;
            }
            var response = JsonSerializer.Deserialize<GetOrdersQueryResponse>(fullMessage.Data!)!;
            tcs.TrySetResult(response);
        };
        _channel.BasicConsume(consumer: consumer, queue: _replyQueueNameGetOrdersForUser, autoAck: true);
    }

    private void DeclareQueueWithProducer(string name, out  string queueReplyName)
    {
        queueReplyName = _channel.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false);
        _logger.Information("{Identifer}: Declared queue {QueueName}", _identifier, queueReplyName);
    }

    public async Task<Result<IEnumerable<MenuListQueryResponse>>> ReceiveAllMenuesAsync(User user)
    {
        _logger.Information("{Identifier}: Querying for menues", _identifier);
        MenuListCommand mlc = new() { Id =  Guid.NewGuid() };
        try
        {
            IEnumerable<MenuListQueryResponse> result = await CallAsync(mlc);
            return new SuccessResult<IEnumerable<MenuListQueryResponse>>(result);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at querying for menues - {@MenuGetAllMessage}", _identifier, mlc);
            return new UnhandledResult<IEnumerable<MenuListQueryResponse>>(new());
        }
    }

    private Task<IEnumerable<MenuListQueryResponse>> CallAsync(MenuListCommand command, CancellationToken cancellationToken = default)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = command.Id.ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replyQueueNameGetMenues;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<IEnumerable<MenuListQueryResponse>>();
        _callbackMapperGetMenues.TryAdd(correlationId, tcs);

        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.MENU_QUERY_USER, basicProperties: props, body: body);

        cancellationToken.Register(() => _callbackMapperGetMenues.TryRemove(correlationId, out _));
        return tcs.Task;
    }

    public async Task<Result<GetOrdersQueryResponse>> GetOrdersForUserAsync(User user)
    {
        _logger.Information("{Identifier}: Querying for user's orders", _identifier);
        GetOrdersCommand gom = new() { UserId = user.Id };
        try
        {
            GetOrdersQueryResponse result = await CallAsync(gom);
            return new SuccessResult<GetOrdersQueryResponse>(result);
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at querying for orders - {@OrdersGetForUser}", _identifier, gom);
            return new UnhandledResult<GetOrdersQueryResponse>(new BinaryFlag());
        }
    }

    private Task<GetOrdersQueryResponse> CallAsync(GetOrdersCommand command)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = command.UserId.ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replyQueueNameGetOrdersForUser;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<GetOrdersQueryResponse>();
        _callbackMapperGetOrdersForUser.TryAdd(correlationId, tcs);

        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.ORDER_GET_FOR_USER, basicProperties: props, body: body);

        return tcs.Task;
    }

    public Result TransmitUser(User user)
    {
        _logger.Information("{Identifier}: Transmitting user", _identifier);
        UserCreationCommand command = user.ToCommand();
        var body = command.ToBody();
        try
        {
            _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.CUSTOMER_CREATION, basicProperties: null!, body: body);
            return new SuccessNoDataResult();
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at transmitting user - {@UserCreateMessage}", _identifier, command);
            return new UnhandledResult(new());
        }
    }

    public Result TransmitUserChanges(UserUpdateDTO changes)
    {
        _logger.Information("{Identifier}: Transmitting user changes", _identifier);
        UserUpdateCommand command = changes.ToCommand();
        var body = command.ToBody();
        try
        {
            _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.CUSTOMER_UPDATE, basicProperties: null!, body: body);
            return new SuccessNoDataResult();
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at transmitting user changes - {@UserUpdateMessage}", _identifier, command);
            return new UnhandledResult(new());
        }
    }

    public Result TransmitPlaceOrder(OrderPlacementRequest orderPlacementRequest)
    {
        _logger.Information("{Identifier}: Transmitting order placement", _identifier);
        OrderPlaceCommand command = orderPlacementRequest.ToCommand();
        var body = command.ToBody();
        try
        {
            _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.ORDER_PLACEMENT, basicProperties: null!, body: body);
            return new SuccessNoDataResult();
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at transmitting order - {@OrderPlaceMessage}", _identifier, command);
            return new UnhandledResult(new());
        }
    }

    public void Dispose()
    {
        _connection.Close();
    }
}
