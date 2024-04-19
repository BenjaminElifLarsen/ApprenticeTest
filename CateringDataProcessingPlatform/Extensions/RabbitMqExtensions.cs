using RabbitMQ.Client.Events;
using Shared.Communication.Models;
using Shared.Patterns.CQRS.Commands;
using Shared.Patterns.ResultPattern;
using System.Text;
using System.Text.Json;

namespace CateringDataProcessingPlatform.Extensions;

internal static class RabbitMqExtensions
{
    public static string ToMessage(this BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        return message;
    }

    public static T ToCommand<T>(this string message) where T : ICommand
    {
        return JsonSerializer.Deserialize<T>(message)!;
    }

    public static Carrier ToCarrier(this Result result)
    {
        if (result)
            return new() { Data = string.Empty, Error = 0, Result = CarrierResult.Success };
        else
            return new() { Data = string.Empty, Error = result.Errors, Result = CarrierResult.Error };
    }

    public static Carrier ToCarrier<T>(this Result<T> result)
    {
        if (result)
        {
            var data = JsonSerializer.Serialize(result.Data);
            return new() { Data = data, Error = 0, Result = CarrierResult.Success };
        }
        else
        {
            return new() { Data = string.Empty, Error = result.Errors, Result = CarrierResult.Error };
        }
    }

    public static byte[] ToBody(this Result result)
    {
        var carrier = result.ToCarrier();
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(carrier));
    }

    public static byte[] ToBody<T>(this Result<T> result)
    {
        var carrier = result.ToCarrier();
        return Encoding.UTF8.GetBytes(JsonSerializer.Serialize(carrier));
    }
}
