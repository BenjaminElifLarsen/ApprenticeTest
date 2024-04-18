using CustomerFrontEnd.Services.AuthenticationStorage;
using CustomerFrontEnd.Settings;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UserFrontend.Frontend;
using UserFrontend.Frontend.Services.Contracts;
using UserFrontend.Frontend.Services.OrderService;
using UserFrontend.Frontend.Services.UserAuthenticationStateProvider;
using UserFrontend.Frontend.Services.UserDataService;
using UserFrontend.Frontend.Services.UserService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var url = builder.Configuration.GetSection("url").Get<UrlEndpoint>()!;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(url.Url) });
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<UserAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(asp =>
    asp.GetRequiredService<UserAuthenticationStateProvider>());
//builder.Services.Configure<UrlEndpoint>(builder.Configuration.GetSection("url"));
//https://blazor-university.com/dependency-injection/dependency-lifetimes-and-scopes/singleton-dependencies/
// webassembly, lifetime of the current application in the current tab of the browser
//builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddSingleton<IAuthenticationStorage, AuthenticationStorage>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
