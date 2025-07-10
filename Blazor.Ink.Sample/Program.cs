using Blazor.Ink.Core;
using Blazor.Ink.Sample;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) => {
        services.AddLogging();
        services.AddSingleton<InkHost>();
    });
var app = builder.Build();

var host = app.Services.GetRequiredService<InkHost>();
host.RegisterComponents(new[] { typeof(HelloInk) });
var task = host.RunAsync();
host.Navigate<HelloInk>();
await task;