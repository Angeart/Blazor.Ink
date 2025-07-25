using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Size = Blazor.Ink.Core.Layout.Value.Size;

namespace Blazor.Ink.Core.Renderer;

/// <summary>
///     Template for a custom Renderer that converts Blazor's render tree to a TUI.
/// </summary>
public partial class InkRenderer : Microsoft.AspNetCore.Components.RenderTree.Renderer
{
    private readonly IAnsiConsole _ansiConsole;
    private readonly ILogger<InkRenderer> _logger;
    private Size _lastRenderedMaxSize = new(0, 0);

    public InkRenderer(
        IServiceProvider serviceProvider,
        ILoggerFactory loggerFactory,
        IAnsiConsole ansiConsole,
        Dispatcher dispatcher) : base(
        serviceProvider, loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<InkRenderer>();
        _ansiConsole = ansiConsole;
        Dispatcher = dispatcher;
        Console.CancelKeyPress += (_, _) =>
        {
            // Move the cursor to the bottom of the console before exiting.
            _ansiConsole.Cursor.MoveDown(_lastRenderedMaxSize.Height);
        };
    }

    // Implementation of abstract members for Blazor Renderer.
    public override Dispatcher Dispatcher { get; }

    protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
    {
        _logger.LogDebug("ReferenceFrames.Count: {Count}", renderBatch.ReferenceFrames.Count);
        // Render the Root node first.
        var ctx = RenderContext.Empty;
        var rootCtx = BuildSpectreRenderable(0, ref ctx);
        _lastRenderedMaxSize = rootCtx.RenderTree.Execute(_ansiConsole);
        rootCtx.Dispose();

        return Task.CompletedTask;
    }

    protected override void HandleException(Exception exception)
    {
        _ansiConsole.WriteException(exception);
    }

    public Task RenderPageAsync(IComponent component)
    {
        var id = AssignRootComponentId(component);
        if (id < 0) throw new InvalidOperationException("Failed to assign component ID.");

        return RenderRootComponentAsync(id);
    }
}