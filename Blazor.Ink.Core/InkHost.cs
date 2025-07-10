using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Blazor.Ink.Core;

public class InkHost : IHost
{
    private readonly IServiceProvider _provider;
    private readonly CancellationTokenSource _cts = new();
    private readonly HashSet<Type> _registeredComponents = new();
    private Type? _currentComponentType;
    private InkRenderer? _renderer; // 追加: InkRendererをフィールド化
    public IServiceProvider Services => _provider;
    public InkHost(IServiceProvider provider)
    {
        _provider = provider;
    }
    public void RegisterComponents(IEnumerable<Type> components)
    {
        foreach (var t in components) _registeredComponents.Add(t);
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        var loggerFactory = _provider.GetRequiredService<ILoggerFactory>();
        _renderer = new InkRenderer(_provider, loggerFactory); // ここで1度だけ生成
        await StartAsync(cancellationToken);
        // メインTUIイベントループ（Ctrl+C等でプロセス終了まで待機）
        try
        {
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        catch (OperationCanceledException) { /* Ctrl+C等で終了 */ }
        // _dispatcher.Stop(); ← 明示的な終了時のみ呼ぶ
    }
    public void Navigate<TComponent>() where TComponent : IComponent
    {
        if (_renderer == null)
            throw new InvalidOperationException("InkHost.RunAsync()でRendererを初期化してからNavigateを呼んでください。");
        var type = typeof(TComponent);
        if (!_registeredComponents.Contains(type))
            throw new InvalidOperationException($"Component {type.Name} is not registered.");
        _currentComponentType = type;
        var logger = _provider.GetRequiredService<ILogger<InkHost>>();
        try
        {
            _renderer.Dispatcher.InvokeAsync(RenderCurrentComponent);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "InkHost.Navigate<{Component}>: Dispatcher.InvokeAsyncで例外が発生しました。", type.Name);
        }
    }
    private Task RenderCurrentComponent()
    {
        if (_currentComponentType == null || _renderer == null) return Task.CompletedTask;
        var componentInstance = Activator.CreateInstance(_currentComponentType) as IComponent;
        return _renderer.RenderPageAsync(componentInstance!); // 使い回し
    }
    public Task StartAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
    public Task StopAsync(CancellationToken cancellationToken = default) { _cts.Cancel(); return Task.CompletedTask; }
    public void Dispose() { _cts.Dispose(); }
}
