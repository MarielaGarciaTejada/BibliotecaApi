using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorClient;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    // Local
   //BaseAddress = new Uri("https://localhost:7093/")
   // App service azure
    BaseAddress = new Uri("https://web-api-biblioteca-fhakc4h4erh6ajcj.westus3-01.azurewebsites.net/")

});

await builder.Build().RunAsync();
