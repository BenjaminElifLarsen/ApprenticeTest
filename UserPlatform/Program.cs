using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Serilogger;
using System.Text;
using UserPlatform.Communication;
using UserPlatform.Communication.Contracts;
using UserPlatform.Helpers;
using UserPlatform.Services.Contracts;
using UserPlatform.Services.OrderService;
using UserPlatform.Services.Security;
using UserPlatform.Services.UserService;
using UserPlatform.Shared.DL.Factories.RefreshTokenFactory;
using UserPlatform.Shared.DL.Factories.UserFactory;
using UserPlatform.Shared.Helpers;
using UserPlatform.Shared.IPL.Context;
using UserPlatform.Shared.IPL.UnitOfWork;
using UserPlatform.Sys;
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
ILogger logger = SeriLoggerService.GenerateLogger(key);
RabbitCommunication communication = new(communicationData, logger);
communication.Initialise();
builder.Services.AddSingleton(logger);
builder.Services.AddSingleton<ICommunication>(communication);
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRefreshTokenFactory, RefreshTokenFactory>();
builder.Services.AddScoped<IUserFactory, UserFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEFCore>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddSwaggerGen(
    c =>
    {
        var securitySchema = new OpenApiSecurityScheme
        {
            Description = "JWT Authorisation using bearer token gained from Login endpoint. <br> 'Bearer token'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Scheme = "Bearer",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        };
        c.AddSecurityDefinition("Bearer", securitySchema);
        var securityRequirement = new OpenApiSecurityRequirement
        {
            { securitySchema, new[]{ "Bearer" } }
        };
        c.AddSecurityRequirement(securityRequirement);
    }
);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(j =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
    j.SaveToken = true;
    j.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
    };
});
builder.Services.AddAuthorization(x =>
{
    x.AddPolicy(AccessLevels.DEFAULT_USER, policy => policy.RequireClaim("level").ToString());
});

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
