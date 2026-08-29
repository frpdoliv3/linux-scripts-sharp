using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UI;
using UI.HomeScreen;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<Application>();

using var host = builder.Build();

var app = host.Services.GetRequiredService<Application>();
await app.RunAsync(new HomeScreen());
