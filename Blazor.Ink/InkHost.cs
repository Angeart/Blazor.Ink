using Blazor.Ink.Core;
using Blazor.Ink.Core.Renderer;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace Blazor.Ink;

public class InkHost : IHost
{
    private readonly CancellationTokenSource _cts = new();
    private InkRenderer? _renderer; // Added: InkRenderer as a field.

    public InkHost(IServiceProvider provider)
    {
        Services = provider;
    }

    public IServiceProvider Services { get; }


    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        _cts.Cancel();
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _cts.Dispose();
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        var loggerFactory = Services.GetRequiredService<ILoggerFactory>();
        var console = Services.GetRequiredService<IAnsiConsole>();
        var dispatcher = Services.GetRequiredService<Dispatcher>();
        _renderer = new InkRenderer(Services, loggerFactory, console, dispatcher); // Instantiate only once here.
        await StartAsync(cancellationToken);
        // Main TUI event loop (waits until process ends, e.g., Ctrl+C).
        try
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            /* Ended by Ctrl+C, etc. */
        }
        // _dispatcher.Stop(); ← Call only on explicit shutdown.
    }

    public async Task Navigate<TComponent>(ComponentParametersBuilder<TComponent>.BuilderFunction? parameterBuilder = null) where TComponent : IComponent
    {
        if (_renderer == null)
            throw new InvalidOperationException("Call Navigate after initializing Renderer with InkHost.RunAsync().");
        var type = typeof(TComponent);
        var logger = Services.GetRequiredService<ILogger<InkHost>>();
        parameterBuilder ??= b => b;
        var parameters = parameterBuilder.Invoke(new()).Build();
        try
        {
            await _renderer.Dispatcher.InvokeAsync(() => RenderComponent(type, parameters));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "InkHost.Navigate<{Component}>: Exception occurred in Dispatcher.InvokeAsync.",
                type.Name);
        }
    }

    private Task RenderComponent(Type componentType, ParameterCollection parameters)
    {
        if (_renderer == null)
            return Task.CompletedTask;
        var fragment = parameters.AsRenderFragment(componentType);
        var wrapper = new RootComponent(fragment);
        return _renderer.RenderPageAsync(wrapper);
    }
}