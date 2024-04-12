using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Shared.Serilogger;
using UserPlatform.Communication;
using UserPlatform.Communication.Contracts;
using UserPlatform.Services.Contracts;
using UserPlatform.Services.OrderService;
using UserPlatform.Shared.IPL.Context;
using UserPlatform.Sys.Appsettings.Models;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
var dbConnection = builder.Configuration.GetConnectionString("database");
builder.Services.AddDbContext<UserContext>(options => options.UseSqlServer(dbConnection)); // TODO: make this look pretty at some point
var communicationData = builder.Configuration.GetSection("rabbit").Get<RabbitMqData>()!;
var key = builder.Configuration.GetConnectionString("logKey")!;
ILogger logger =  SeriLoggerService.GenerateLogger(key);
RabbitCommunication communication = new(communicationData, logger);
communication.Initialise();
builder.Services.AddSingleton<ICommunication>(communication);
builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddSwaggerGen(c => // TODO: look more into this
//{
//    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
//    {
//        Description = "JWT Authorisation using bearer token gained from Login endpoint",
//        Name = "Authorization",
//        In = ParameterLocation.Header,
//        Type = SecuritySchemeType.Http,
//        Scheme = "Bearer"
//    });
//    c.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "Bearer",
//                },
//            },
//            new List<string>()
//        }
//    });
//});


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
