using Catering.API.Communication.Contract;
using Catering.API.Extensions;
using Catering.API.Models.Dish.Request;
using Catering.API.Models.Menu.Request;
using Catering.API.Sys.Appsettings.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Communication;
using Shared.Communication.Models;
using Shared.Communication.Models.Dish;
using Shared.Communication.Models.Menu;
using Shared.Patterns.CQRS.Commands;
using Shared.Patterns.ResultPattern;
using Shared.Patterns.ResultPattern.BadRequest;
using Shared.Service;
using System.Collections.Concurrent;
using System.Text.Json;
using ILogger = Serilog.ILogger;

namespace Catering.API.Communication;

public sealed class RabbitCommunication : BaseService, ICommunication, IDisposable
{

    private readonly ConnectionFactory _connectionFactory;
    private IConnection _connection;
    private IModel _channel;

    private readonly ConcurrentDictionary<string, TaskCompletionSource<Result>> _callbackForNoData;

    private string _replayQueueNameCreationNoData;

    internal RabbitCommunication(RabbitMqData rabbitMqData, ILogger logger) : base(logger)
    {
        _connectionFactory = new ConnectionFactory { HostName = rabbitMqData.Url, Port = rabbitMqData.Port };
        _channel = null!;
        _connection = null!;
        _replayQueueNameCreationNoData = null!;
        _callbackForNoData = [];
    }

    public void Initialise()
    {
        _connection = _connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.BasicQos(0, 1, false);

        _replayQueueNameCreationNoData = _channel.QueueDeclare().QueueName;

        DeclareQueueWithProducer(CommunicationQueueNames.DISH_CREATION);
        DeclareQueueWithProducer(CommunicationQueueNames.MENU_CREATION);

        SetNoDataResultConsumer();
        //SetCreateMenuDishConsumer();

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
            { // TODO: consider if it is best to have a version for each creation request
                //_logger.Warning("{Identifier}: Failed at creating dish - {Errors}", _identifier, carrier.Error.ToBinary());
                tcs.SetResult(new BadRequestResult(new(carrier.Error)));
                return;
            }
            tcs.TrySetResult(new SuccessNoDataResult());
        };
        _channel.BasicConsume(consumer: consumer, queue: _replayQueueNameCreationNoData, autoAck: true);
    }

    //private void SetCreateMenuDishConsumer()
    //{

    //}


    private void DeclareQueueWithProducer(string name)
    {
        _channel.QueueDeclare(queue: name, durable: true, exclusive: false, autoDelete: false);
        _logger.Information("{Identifer}: Declared queue {QueueName}", _identifier);
    }

    public async Task<Result> TransmitCreateDishAsync(DishCreateRequest request)
    { // TODO: get errors and such from the other side
        _logger.Information("{Identifier}: Transmitting dish", _identifier);
        DishCreationCommand dcc = request.ToCommand();
        try
        {
            var result = await CallAsync(CommunicationQueueNames.DISH_CREATION, dcc);
            if (!result)
                _logger.Warning("{Identifier}: Dish creation failed - {DishError}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at create dish - {@DishCreatinMessage}", _identifier, dcc);
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
                _logger.Warning("{Identifier}: Menu creation failed - {MenuError}", result);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "{Identifier}: Failed at create Menu - {@MenuCreatinMessage}", _identifier, mcc);
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

    public void Dispose()
    {
        _connection.Close();
    }
}
