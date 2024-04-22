using Catering.API.Communication.Contract;
using Catering.API.Extensions;
using Catering.API.Models.Dish.Request;
using Catering.API.Models.Menu.Request;
using Catering.API.Models.Order.Request;
using Catering.API.Models.Order.Response;
using Catering.API.Sys.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Communication.Models.Dish;
using Shared.Communication.Models.Menu;
using Shared.Communication.Models.Order;
using Shared.Patterns.CQRS.Commands;
using Shared.Patterns.ResultPattern;
using Shared.Service;
using System.Collections.Concurrent;
using ILogger = Serilog.ILogger;

namespace Catering.API.Communication;

public sealed class RabbitCommunication : BaseService, ICommunication, IDisposable
{

    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<Result>> _callbackForNoData;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<Result<DishListQueryResponse>>> _callbackDishList;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<Result<GetOrdersFututureQueryResponse>>> _callbackOrdersFuture;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<Result<MenuListFinerDetailsQueryResponse>>> _callbackMenuList;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<Result<GetOrderOverviewQueryResponse>>> _callbackOrderOverview;

    private string _replayQueueNameCreationNoData;
    private string _replayQueueNameDishList;
    private string _replayQueueNameMenuList;
    private string _replayQueueNameOrderFuture;
    private string _replayQueueNameOrderOverview;

    internal RabbitCommunication(RabbitMqData rabbitMqData, ILogger logger) : base(logger)
    {
        _connectionFactory = new ConnectionFactory { HostName = rabbitMqData.Url, Port = rabbitMqData.Port };
        _channel = null!;
        _connection = null!;
        _replayQueueNameCreationNoData = null!;
        _replayQueueNameDishList = null!;
        _replayQueueNameMenuList = null!;
        _replayQueueNameOrderFuture = null!;
        _replayQueueNameOrderOverview = null!;
        _callbackForNoData = [];
        _callbackDishList = [];
        _callbackMenuList = [];
        _callbackOrdersFuture = [];
        _callbackOrderOverview = [];
    }

    public void Initialise()
    {
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.BasicQos(0, 1, false);

        _replayQueueNameCreationNoData = _channel.QueueDeclare().QueueName;
        _replayQueueNameDishList = _channel.QueueDeclare().QueueName;
        _replayQueueNameMenuList = _channel.QueueDeclare().QueueName;
        _replayQueueNameOrderFuture = _channel.QueueDeclare().QueueName;
        _replayQueueNameOrderOverview = _channel.QueueDeclare().QueueName;

        DeclareQueueWithProducer(CommunicationQueueNames.DISH_CREATION);
        DeclareQueueWithProducer(CommunicationQueueNames.MENU_CREATION);
        DeclareQueueWithProducer(CommunicationQueueNames.MENU_QUERY_EMPLOYEE);
        DeclareQueueWithProducer(CommunicationQueueNames.ORDER_QUERY_FUTURE);
        DeclareQueueWithProducer(CommunicationQueueNames.DISH_QUERY);
        DeclareQueueWithProducer(CommunicationQueueNames.ORDER_STATUS);
        DeclareQueueWithProducer(CommunicationQueueNames.ORDER_QUERY_OVERVIEW);

        SetNoDataResultConsumer(); 
        SetGetDishesConsumer();
        SetGetFutureOrdersConsumer();
        SetGetMenuesConsumer();
        SetGetOverviewOrdersConsumer();

        _logger.Information("{Identifier}: Initialised", _identifier);
    }

    private void SetNoDataResultConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackForNoData.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var message = ea.ToMessage();
            var carrier = message.ToModel<Carrier>();
            if(carrier.Result is not CarrierResult.Success)
            {
                tcs.SetResult(new BadRequestResult(new(carrier.Error)));
                return;
            }
            tcs.TrySetResult(new SuccessNoDataResult());
        };
        _channel.BasicConsume(consumer: consumer, queue: _replayQueueNameCreationNoData, autoAck: true);
    }

    private void SetGetMenuesConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackMenuList.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var message = ea.ToMessage();
            var carrier = message.ToModel<Carrier>();
            if (carrier.Result is not CarrierResult.Success)
            {
                tcs.SetResult(new BadRequestResult<MenuListFinerDetailsQueryResponse>(new(carrier.Error)));
                return;
            }
            var data = carrier.Data!.ToModel<MenuListFinerDetailsQueryResponse>();
            tcs.TrySetResult(new SuccessResult<MenuListFinerDetailsQueryResponse>(data));
        };
        _channel.BasicConsume(consumer: consumer, queue: _replayQueueNameMenuList, autoAck: true);
    }

    private void SetGetDishesConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackDishList.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var message = ea.ToMessage();
            var carrier = message.ToModel<Carrier>();
            if (carrier.Result is not CarrierResult.Success)
            {
                tcs.SetResult(new BadRequestResult<DishListQueryResponse>(new(carrier.Error)));
                return;
            }
            var data = carrier.Data!.ToModel<DishListQueryResponse>();
            tcs.TrySetResult(new SuccessResult<DishListQueryResponse>(data));
        };
        _channel.BasicConsume(consumer: consumer, queue: _replayQueueNameDishList, autoAck: true);
    }

    private void SetGetFutureOrdersConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackOrdersFuture.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var message = ea.ToMessage();
            var carrier = message.ToModel<Carrier>();
            if (carrier.Result is not CarrierResult.Success)
            {
                tcs.SetResult(new BadRequestResult<GetOrdersFututureQueryResponse>(new(carrier.Error)));
                return;
            }
            var data = carrier.Data!.ToModel<GetOrdersFututureQueryResponse>();
            tcs.TrySetResult(new SuccessResult<GetOrdersFututureQueryResponse>(data));
        };
        _channel.BasicConsume(consumer: consumer, queue: _replayQueueNameOrderFuture, autoAck: true);
    }

    private void SetGetOverviewOrdersConsumer()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            if (!_callbackOrderOverview.TryRemove(ea.BasicProperties.CorrelationId, out var tcs))
                return;
            var message = ea.ToMessage();
            var carrier = message.ToModel<Carrier>();
            if(carrier.Result is not CarrierResult.Success)
            {
                tcs.SetResult(new BadRequestResult<GetOrderOverviewQueryResponse>(new(carrier.Error)));
                return;
            }
            var data = carrier.Data!.ToModel<GetOrderOverviewQueryResponse>();
            tcs.SetResult(new SuccessResult<GetOrderOverviewQueryResponse>(data));
        };
        _channel.BasicConsume(consumer: consumer, queue: _replayQueueNameOrderOverview, autoAck: true);
    }

    //private void SetCreateMenuDishConsumer()
    //{

    //}

    private void DeclareQueueWithProducer(string name)
    {
        var createQueueName = _channel.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false);
        _logger.Information("{Identifer}: Declared queue {QueueName}", _identifier, createQueueName.QueueName);
    }

    public async Task<Result> TransmitCreateDishAsync(DishCreateRequest request)
    {
        _logger.Information("{Identifier}: Transmitting dish", _identifier);
        DishCreationCommand dcc = request.ToCommand();
        try
        {
            var result = await CallAsync(CommunicationQueueNames.DISH_CREATION, dcc);
            if (!result)
                _logger.Warning("{Identifier}: Dish creation failed - {DishCreationError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at create dish - {@DishCreatingMessage}", _identifier, dcc);
            return new UnhandledResult(new());
        }
    }

    public async Task<Result> TransmitCreateMenuAsync(MenuCreateRequest request)
    {
        MenuCreationCommand mcc = request.ToCommand();
        try
        {
            var result = await CallAsync(CommunicationQueueNames.MENU_CREATION, mcc);
            if (!result)
                _logger.Warning("{Identifier}: Menu creation failed - {MenuCreationError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at create Menu - {@MenuCreatingMessage}", _identifier, mcc);
            return new UnhandledResult(new());
        }
    }

    public async Task<Result> TransmitOrderStatusAsync(SetOrderStatusRequest request)
    {
        SetOrderStatusCommand sosc = request.ToCommand();
        try
        {
            var result = await CallAsync(CommunicationQueueNames.ORDER_STATUS, sosc);
            if (!result)
                _logger.Warning("{Identifier}: Order status failed - {OrderStatusError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at setting order status - {@OrderStatusMessage}", _identifier, sosc);
            return new UnhandledResult(new());
        }
    }

    private Task<Result> CallAsync<T>(string routingKey, T command) where T : ICommand
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replayQueueNameCreationNoData;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<Result>();
        _callbackForNoData.TryAdd(correlationId, tcs);
        _channel.BasicPublish(exchange: string.Empty, routingKey: routingKey, basicProperties: props, body: body);
        return tcs.Task;
    }

    public async Task<Result<GetOrdersFututureQueryResponse>> GetFutureOrdersAsync()
    {
        GetFutureOrdersCommand gfoc = new();
        try
        {
            var result = await CallAsync(gfoc);
            if (!result)
                _logger.Warning("{Identifier}: Getting future orders failed - {FutureOrderError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at getting future orders - {@FutureOrderMessage}", _identifier, gfoc);
            return new UnhandledResult<GetOrdersFututureQueryResponse>(new());
        }
    }

    private Task<Result<GetOrdersFututureQueryResponse>> CallAsync(GetFutureOrdersCommand command)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replayQueueNameOrderFuture;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<Result<GetOrdersFututureQueryResponse>>();
        _callbackOrdersFuture.TryAdd(correlationId, tcs);
        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.ORDER_QUERY_FUTURE, basicProperties: props, body: body);
        return tcs.Task;
    }

    public async Task<Result<MenuListFinerDetailsQueryResponse>> GetMenuesAsync()
    {
        MenuListFinerDetailsCommand mlfdc = new();
        try
        {
            var result = await CallAsync(mlfdc);
            if (!result)
                _logger.Warning("{Identifier}: Getting menu list failed - {MenuListError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at getting menues - {@MenuListMessage}", _identifier, mlfdc);
            return new UnhandledResult<MenuListFinerDetailsQueryResponse>(new());
        }
    }

    private Task<Result<MenuListFinerDetailsQueryResponse>> CallAsync(MenuListFinerDetailsCommand command)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replayQueueNameMenuList;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<Result<MenuListFinerDetailsQueryResponse>>();
        _callbackMenuList.TryAdd(correlationId, tcs);
        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.MENU_QUERY_EMPLOYEE, basicProperties: props, body: body);
        return tcs.Task;
    }

    public async Task<Result<DishListQueryResponse>> GetDishesAsync()
    {
        DishListCommand dlc = new();
        try
        {
            var result = await CallAsync(dlc);
            if (!result)
                _logger.Warning("{Identifier}: Getting dish list failed - {DishListError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at getting dishes - {@DishListMessage}", _identifier, dlc);
            return new UnhandledResult<DishListQueryResponse>(new());
        }
    }

    private Task<Result<DishListQueryResponse>> CallAsync(DishListCommand command)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replayQueueNameDishList;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<Result<DishListQueryResponse>>();
        _callbackDishList.TryAdd(correlationId, tcs);
        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.DISH_QUERY, basicProperties: props, body: body);
        return tcs.Task;
    }

    public async Task<Result<GetOrderOverviewQueryResponse>> GetOrdersOverviewAsync()
    {
        GetOrderOverviewQueryCommand gooqc = new();
        try
        {
            var result = await CallAsync(gooqc);
            if (!result)
                _logger.Warning("{Identifier}: Getting order overview failed - {OrderOverviewError}", _identifier, result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at getting order overview - {@OrderOverviewMessage}", _identifier, gooqc);
            return new UnhandledResult<GetOrderOverviewQueryResponse>(new());
        }
        throw new NotImplementedException();
    }

    private Task<Result<GetOrderOverviewQueryResponse>> CallAsync(GetOrderOverviewQueryCommand command)
    {
        IBasicProperties props = _channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = _replayQueueNameOrderOverview;
        var body = command.ToBody();
        var tcs = new TaskCompletionSource<Result<GetOrderOverviewQueryResponse>>();
        _callbackOrderOverview.TryAdd(correlationId, tcs);
        _channel.BasicPublish(exchange: string.Empty, routingKey: CommunicationQueueNames.ORDER_QUERY_OVERVIEW, basicProperties: props, body: body);
        return tcs.Task;

    }

    public void Dispose()
    {
        _connection.Close();
    }
}
