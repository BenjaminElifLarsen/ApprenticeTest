using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Serilogger;
using UserPlatform.Communication;
using UserPlatform.Shared.IPL.Context;
using UserPlatform.Sys.Appsettings.Models;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var dbConnection = builder.Configuration.GetConnectionString("database");
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(dbConnection)); // TODO: make this look pretty at some point
var communicationData = builder.Configuration.GetSection("rabbit").Get<RabbitMqData>()!;
var key = builder.Configuration.GetConnectionString("logKey")!;
ILogger logger =  SeriLoggerService.GenerateLogger(key);
RabbitCommunication communication = new(communicationData, logger);
communication.Initialise();
builder.Services.AddSingleton(communication);

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
