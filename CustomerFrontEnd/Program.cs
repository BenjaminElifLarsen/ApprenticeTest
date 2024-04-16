using CustomerFrontEnd.Components;
using CustomerFrontEnd.Services;
using CustomerFrontEnd.Services.AuthenticationStorage;
using CustomerFrontEnd.Services.Contracts;
using CustomerFrontEnd.Services.UserService;
using CustomerFrontEnd.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.Configure<UrlEndpoint>(builder.Configuration.GetSection("url"));
//https://blazor-university.com/dependency-injection/dependency-lifetimes-and-scopes/singleton-dependencies/
// webassembly, lifetime of the current application in the current tab of the browser
//builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddSingleton<IAuthenticationStorage, AuthenticationStorage>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddAuthenticationCore();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
