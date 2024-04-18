using Catering.API.Models.Dish.Request;
using Shared.Communication.Models.Dish;
using System.Text.Json;
using System.Text;
using Shared.Patterns.CQRS.Commands;
using Shared.Communication.Models.Menu;
using Catering.API.Models.Menu.Request;
using RabbitMQ.Client.Events;

namespace Catering.API.Extensions;

public static class RabbitMqHelper
{
    public static DishCreationCommand ToCommand(this DishCreateRequest request)
    {
        DishCreationCommand command = new()
        {
            Name = request.Name,
        };
        return command;
    }

    public static MenuCreationCommand ToCommand(this MenuCreateRequest request)
    {
        MenuCreationCommand command = new()
        {
            Name = request.Name,
            Description = request.Description,
            Dishes = request.Dishes.Select(x => new MenuPartCreationCommand()
            {
                DishId = x.DishId,
                Amount = x.Amount,
                Price = x.Price,
                //Name = x.Name,
            }),
        };
        return command;
    }

    public static byte[] ToBody<T>(this T command) where T : ICommand
    {
        var message = JsonSerializer.Serialize(command);
        var body = Encoding.UTF8.GetBytes(message);
        return body;
    }

    public static string ToMessage(this BasicDeliverEventArgs e)
    {
        var body = e.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        return message;
    }

    public static T ToModel<T>(this string message) where T : class
    {
        return JsonSerializer.Deserialize<T>(message)!;
    }

    public static string ToBinary(this long errors)
    {
        return Convert.ToString(errors, 2);
    }
}
