using Catering.API.Communication;
using Catering.API.Communication.Contract;
using Catering.API.Services.Contracts;
using Catering.API.Services.DishService;
using Catering.API.Services.MenuService;
using Catering.API.Services.OrderService;
using Catering.API.Sys.Appsettings.Models;
using Shared.Serilogger;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var communicationData = builder.Configuration.GetSection("rabbit").Get<RabbitMqData>()!;
var key = builder.Configuration.GetConnectionString("logKey")!;
ILogger logger = SeriLoggerService.GenerateLogger(key);
RabbitCommunication communication = new(communicationData, logger);
communication.Initialise();
builder.Services.AddSingleton(logger);
builder.Services.AddSingleton<ICommunication>(communication);
builder.Services.AddScoped<IDishService, DishService>();
builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
