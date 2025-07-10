using Blazor.Ink.Core.Components;
using Spectre.Console.Rendering;
using YogaSharp;

namespace Blazor.Ink.Core.Layouts;

public interface ILayoutNode {}
public abstract unsafe class NodeBase : IDisposable, ILayoutNode
{
    protected readonly record struct Size(int Width, int Height);

    protected readonly record struct Margin(int Top, int Right, int Bottom, int Left);

    protected readonly record struct Padding(int Top, int Right, int Bottom, int Left);


    public readonly YGNode* Node = YGNode.New();
    public readonly YGConfig* Config = YGConfig.GetDefault();
    protected NodeBase? Child { get; set; }

    public void Dispose()
    {
        Node->Dispose();
    }

    protected Size GetSize()
    {
        return new Size(
            (int)Node->GetWidth().Value,
            (int)Node->GetHeight().Value);
    }

    protected Margin GetMargin()
    {
        return new Margin(
            (int)Node->GetComputedMargin(YGEdge.Top),
            (int)Node->GetComputedMargin(YGEdge.Right),
            (int)Node->GetComputedMargin(YGEdge.Bottom),
            (int)Node->GetComputedMargin(YGEdge.Left));
    }

    protected Padding GetPadding()
    {
        return new Padding(
            (int)Node->GetComputedPadding(YGEdge.Top),
            (int)Node->GetComputedPadding(YGEdge.Right),
            (int)Node->GetComputedPadding(YGEdge.Bottom),
            (int)Node->GetComputedPadding(YGEdge.Left));
    }

    public abstract void ApplyComponent(IInkComponent component);

    protected abstract IRenderable BuildRenderTree(IRenderable child);
}