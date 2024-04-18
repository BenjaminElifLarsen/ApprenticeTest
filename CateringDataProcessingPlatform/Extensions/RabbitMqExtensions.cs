using RabbitMQ.Client.Events;
using Shared.Patterns.CQRS.Commands;
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
}
