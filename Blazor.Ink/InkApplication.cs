using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using System.Threading;
using Spectre.Console;

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

    public async Task RunAsync<TInitialPage>() where TInitialPage : IComponent
    {
        // Start the TUI event loop using InkHost
        var host = _provider.GetRequiredService<InkHost>();
        var task = host.RunAsync();
        host.Navigate<TInitialPage>();
        await task;
    }

    public void RegisterComponents(IEnumerable<Type> components)
    {
        var host = _provider.GetRequiredService<InkHost>();
        host.RegisterComponents(components);
    }
}