using Blazor.Ink.Core.Layout;
using Spectre.Console;
using Spectre.Console.Rendering;
using Size = Blazor.Ink.Core.Layout.Value.Size;

namespace Blazor.Ink.Core.Renderer.Abstraction;

public interface IRenderableComponent
{
    IRenderable? RenderElement { get; }
    ref LayoutNode LayoutNode { get; }
    bool Inline { get; }
    void ApplyLayout();
    void BuildElement();
    Size Render(IAnsiConsole console);
}