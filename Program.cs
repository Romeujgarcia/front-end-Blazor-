using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskManagerClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Adiciona o HttpClient com a URL base da sua API
builder.Services.AddScoped(sp => new HttpClient 
{
    BaseAddress = new Uri("http://localhost:5223/") // URL da sua API
});

// Adiciona o AuthService
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<TaskService>();

builder.Services.AddBlazoredLocalStorage();


await builder.Build().RunAsync();
