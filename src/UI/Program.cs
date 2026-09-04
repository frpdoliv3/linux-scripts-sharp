using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UI;
using UI.Screens;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<Application>();
builder.Services.AddSingleton<INavigator, Navigator>();

#region Screens
builder.Services.AddScoped<HomeScreen>();
builder.Services.AddScoped<SetupTorrentScreen>();
#endregion

using var host = builder.Build();

var app = host.Services.GetRequiredService<Application>();
await app.RunAsync(ScreenType.Of<HomeScreen>());
