using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Spectre.Console;
using Blazor.Ink.Core.Components;
using Spectre.Console.Rendering;

namespace Blazor.Ink.Core;

/// <summary>
/// BlazorのレンダーツリーをTUIに変換するカスタムRendererの雛形
/// </summary>
public partial class InkRenderer : Renderer
{
  private readonly ILogger<InkRenderer> _logger;
  public InkRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory) : base(serviceProvider, loggerFactory)
  {
    _logger = loggerFactory.CreateLogger<InkRenderer>();
  }

  protected override Task UpdateDisplayAsync(in RenderBatch renderBatch)
  {
    _logger.LogDebug("ReferenceFrames.Count: {Count}", renderBatch.ReferenceFrames.Count);
    // ひとまずRootをレンダーする
    var root = BuildSpectreRenderable(0);
    if (root.Renderable is not null)
    {
        AnsiConsole.Write(root.Renderable);
    }
    return Task.CompletedTask;
  }


  // Blazor Rendererの抽象メンバ実装
  private readonly InkDispatcher _dispatcher = new InkDispatcher();
  public override Dispatcher Dispatcher => _dispatcher;
  protected override void HandleException(Exception exception)
  {
    AnsiConsole.WriteException(exception);
  }

  public Task RenderPageAsync(IComponent component)
  {
    var id = base.AssignRootComponentId(component);
    if (id < 0)
    {
      throw new InvalidOperationException("Failed to assign component ID.");
    }
    return base.RenderRootComponentAsync(id);
  }
}
