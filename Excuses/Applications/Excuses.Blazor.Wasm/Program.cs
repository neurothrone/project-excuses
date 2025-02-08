using Excuses.Blazor.Wasm.Components;
using Excuses.Persistence.InMemory.Data;
using Excuses.Persistence.InMemory.Repositories;
using Excuses.Persistence.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<InMemoryDataStore>();
builder.Services.AddScoped<IExcuseRepository, ExcuseInMemoryRepository>();

await builder.Build().RunAsync();