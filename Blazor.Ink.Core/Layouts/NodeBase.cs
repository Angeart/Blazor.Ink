using Blazor.Ink.Core.Components;
using Microsoft.AspNetCore.Components;
using Spectre.Console;
using Spectre.Console.Rendering;
using YogaSharp;

namespace Blazor.Ink.Core.Layouts;

public readonly record struct Size(int Width, int Height)
{
    public static Size Max(in Size a, in Size b)
    {
        return new Size(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
    }
};

public readonly record struct Margin(int Top, int Right, int Bottom, int Left);

public readonly record struct Padding(int Top, int Right, int Bottom, int Left);

public interface IInkNode : IDisposable
{
    void CalculateLayout();
    Size Render(IRenderable renderable);
    void AppendChild(IInkNode? child);
    IInkNode ApplyComponent(IComponent component);
    IInkNode ApplyText(string text);
    RenderTree BuildRenderTree();
}

public abstract unsafe class NodeBase : IInkNode
{
    public readonly YGNode* Node = YGNode.New();
    public readonly YGConfig* Config = YGConfig.GetDefault();
    protected List<NodeBase> Children { get; } = new();

    public void Dispose()
    {
        for (var i = 0; i < Children.Count; i++)
        {
            Children[i].Dispose();
        }

        Node->Dispose();
    }

    public void CalculateLayout()
    {
        ApplyLayout();
        UpdateNodeTree();
        Node->CalculateLayout();
    }

    private void UpdateNodeTree()
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.UpdateNodeTree();
            Node->InsertChild(child.Node, i);
        }
    }

    public abstract Size Render(IRenderable renderable);

    public void AppendChild(IInkNode? child)
    {
        if (child is not NodeBase node)
        {
            return;
        }

        Children.Add(node);
    }

    protected abstract void ApplyLayoutImpl();

    protected void ApplyLayout()
    {
        ApplyLayoutImpl();
        foreach (var child in Children)
        {
            child.ApplyLayout();
        }
    }

    public abstract IInkNode ApplyComponent(IComponent component);
    // public abstract void ApplyLayout();

    public virtual IInkNode ApplyText(string text)
    {
        return this;
    }

    /// <summary>
    /// Builds the render tree for the specified child element.
    /// </summary>
    /// <remarks>
    /// Should be called CalculateLayout() before this method to ensure the layout is calculated.
    /// </remarks>
    public abstract RenderTree BuildRenderTree();
}

public abstract unsafe class NodeBase<TInkComponent> : NodeBase, IDisposable, IInkNode
    where TInkComponent : class, IInkComponent
{
    protected TInkComponent? Component { get; private set; } = null!;

    protected Size GetSize()
    {
        return new Size(
            (int)Node->GetComputedWidth(),
            (int)Node->GetComputedHeight());
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

    public override IInkNode ApplyComponent(IComponent component)
    {
        Component = component as TInkComponent;
        if (Component is null)
        {
            AnsiConsole.WriteException(new NullReferenceException($"Component is null, actual parameter:{component}"));
            return this;
        }

        return this;
    }

    public override Size Render(IRenderable renderable)
    {
        var left = (int)Node->GetComputedLeft();
        var top = (int)Node->GetComputedTop();
        var width = (int)Node->GetComputedWidth();
        var height = (int)Node->GetComputedHeight();
        AnsiConsole.Cursor.MoveRight(left);
        AnsiConsole.Cursor.MoveDown(top);
        // AnsiConsole.Cursor.SetPosition(left, top);
        AnsiConsole.Write(renderable);
        AnsiConsole.Cursor.MoveLeft(left + width);
        AnsiConsole.Cursor.MoveUp(top + height);
        return new Size(width, height);
    }
}