using CustomerFrontEnd.Services.AuthenticationStorage;
using CustomerFrontEnd.Services.Contracts;
using CustomerFrontEnd.Services.UserService;
using CustomerFrontEnd.Settings;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using UserFrontend.Frontend;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7298/") });
builder.Services.AddScoped<IUserService, UserService>();
//builder.Services.Configure<UrlEndpoint>(builder.Configuration.GetSection("url"));
//https://blazor-university.com/dependency-injection/dependency-lifetimes-and-scopes/singleton-dependencies/
// webassembly, lifetime of the current application in the current tab of the browser
//builder.Services.AddScoped<ProtectedSessionStorage>();
builder.Services.AddSingleton<IAuthenticationStorage, AuthenticationStorage>();
//builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
//builder.Services.AddAuthenticationCore();

await builder.Build().RunAsync();
