using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading;

namespace Blazor.Ink;

public class InkApplication
{
    private readonly IServiceProvider _provider;
    public string[] Args { get; }
    public InkApplication(IServiceProvider provider, string[] args)
    {
        _provider = provider;
        Args = args;
    }

    public async Task RunAsync<TComponent>() where TComponent : IComponent
    {
        // Start the TUI event loop using InkHost
        var host = new InkHost(_provider);
        await host.RunAsync();
    }
}

public static class InkApplicationHost
{
    public static InkApplicationBuilder CreateBuilder(string[] args)
        => new InkApplicationBuilder(args);
}
