using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

namespace Blazor.Ink;

public class InkApplicationBuilder
{
    public IServiceCollection Services { get; }
    public string[] Args { get; }

    public InkApplicationBuilder(string[] args)
    {
        Args = args;
        Services = new ServiceCollection();
    }

    public InkApplication Build()
    {
        return new InkApplication(
            Services.BuildServiceProvider(),
            Args);
    }

    public InkApplicationBuilder UseDefaultServices()
    {
        Services.AddSingleton<IAnsiConsole>(
            _ => AnsiConsole.Create(new()));
        Services.AddLogging();
        Services.AddSingleton<InkHost>();
        Services.AddSingleton<Dispatcher, InkDispatcher>();
        return this;
    }

    public InkApplicationBuilder ConfigureServices(
        Action<IServiceCollection> configure)
    {
        configure(Services);
        return this;
    }
}