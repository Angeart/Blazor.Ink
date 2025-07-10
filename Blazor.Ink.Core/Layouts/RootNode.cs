using Microsoft.AspNetCore.Components;
using Spectre.Console.Rendering;

namespace Blazor.Ink.Core.Layouts;

public class RootNode : NodeBase
{

    public override Size Render(IRenderable renderable)
    {
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
        return new RenderTree(() => { }, childrenRenderTree);
    }
}