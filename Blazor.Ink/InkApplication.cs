using Blazor.Ink.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Ink;

public class InkApplication
{
    private readonly IServiceProvider _provider;

    public InkApplication(IServiceProvider provider, string[] args)
    {
        _provider = provider;
        Args = args;
    }

    public string[] Args { get; }

    public async Task RunAsync<TInitialPage>(CancellationToken cancellationToken = default)
        where TInitialPage : IComponent
    {
        var host = _provider.GetRequiredService<InkHost>();
        var task = host.RunAsync(cancellationToken);
        await host.Navigate<TInitialPage>();
        await task;
    }

    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        var host = _provider.GetRequiredService<InkHost>();
        var task = host.RunAsync(cancellationToken);
        return task;
    }

    public Task Navigate<TPage>() where TPage : IComponent
    {
        var host = _provider.GetRequiredService<InkHost>();
        return host.Navigate<TPage>();
    }

    public Task Navigate<TPage>(ComponentParametersBuilder<TPage>.BuilderFunction parametersBuilder)
        where TPage : IComponent
    {
        var host = _provider.GetRequiredService<InkHost>();
        return host.Navigate(parametersBuilder);
    }
}