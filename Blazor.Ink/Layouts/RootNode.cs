using Microsoft.AspNetCore.Components;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Blazor.Ink.Layouts;

public class RootNode : NodeBase
{
    public RootNode(IAnsiConsole ansiConsole) : base(ansiConsole)
    {
    }

    public override Size Render(IRenderable renderable, bool inline = false)
    {
        // The root node does not render anything itself.
        // It is just a container for other nodes.
        return new Size(0, 0);
    }

    public override IInkNode ApplyComponent(IComponent component)
    {
        return this;
    }

    protected override void ApplyLayoutImpl()
    {
    }

    public override RenderTree BuildRenderTree()
    {
        var childrenRenderTree = Children.Select(it => it.BuildRenderTree()).ToList();
        return new RenderTree(() => new Size(0, 0), childrenRenderTree);
    }
}