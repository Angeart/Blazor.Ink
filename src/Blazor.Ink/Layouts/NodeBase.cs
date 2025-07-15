using Blazor.Ink.Components;
using Microsoft.AspNetCore.Components;
using Spectre.Console;
using Spectre.Console.Rendering;
using YogaSharp;

namespace Blazor.Ink.Layouts;

public readonly record struct Size(int Width, int Height)
{
    public static Size Max(in Size a, in Size b)
    {
        return new Size(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
    }
}

public readonly record struct Margin(int Top, int Right, int Bottom, int Left);

public readonly record struct Padding(int Top, int Right, int Bottom, int Left);

public interface IInkNode : IDisposable
{
    void CalculateLayout();
    Size Render(IRenderable renderable, bool inline = false);
    void AppendChild(IInkNode? child);
    IInkNode ApplyComponent(IComponent component);
    IInkNode ApplyText(string text);
    RenderTree BuildRenderTree();
}

public abstract unsafe class NodeBase : IInkNode
{
    protected readonly IAnsiConsole _ansiConsole;
    public readonly YGConfig* Config = YGConfig.GetDefault();
    public readonly YGNode* Node = YGNode.New();

    protected NodeBase(IAnsiConsole ansiConsole)
    {
        _ansiConsole = ansiConsole;
    }

    protected List<NodeBase> Children { get; } = new();

    public void Dispose()
    {
        for (var i = 0; i < Children.Count; i++) Children[i].Dispose();

        Node->Dispose();
    }

    public void CalculateLayout()
    {
        ApplyLayout();
        UpdateNodeTree();
        Node->CalculateLayout();
    }

    public abstract Size Render(IRenderable renderable, bool inline = false);

    public void AppendChild(IInkNode? child)
    {
        if (child is not NodeBase node) return;

        Children.Add(node);
    }

    public abstract IInkNode ApplyComponent(IComponent component);
    // public abstract void ApplyLayout();

    public virtual IInkNode ApplyText(string text)
    {
        var textNode = new TextNode(_ansiConsole);
        textNode.ApplyText(text);
        AppendChild(textNode);
        return this;
    }

    /// <summary>
    ///     Builds the render tree for the specified child element.
    /// </summary>
    /// <remarks>
    ///     Should call CalculateLayout() before this method to ensure the layout is calculated.
    /// </remarks>
    public abstract RenderTree BuildRenderTree();

    private void UpdateNodeTree()
    {
        for (var i = 0; i < Children.Count; i++)
        {
            var child = Children[i];
            child.UpdateNodeTree();
            Node->InsertChild(child.Node, i);
        }
    }

    protected abstract void ApplyLayoutImpl();

    protected void ApplyLayout()
    {
        ApplyLayoutImpl();
        foreach (var child in Children) child.ApplyLayout();
    }
}

public abstract unsafe class NodeBase<TInkComponent> : NodeBase, IInkNode
    where TInkComponent : class, IInkComponent
{
    protected NodeBase(IAnsiConsole ansiConsole) : base(ansiConsole)
    {
    }

    protected TInkComponent? Component { get; private set; }

    public override IInkNode ApplyComponent(IComponent component)
    {
        Component = component as TInkComponent;
        if (Component is null)
        {
            _ansiConsole.WriteException(new NullReferenceException($"Component is null, actual parameter:{component}"));
            return this;
        }

        return this;
    }

    public override Size Render(IRenderable renderable, bool inline = false)
    {
        var left = (int)Node->GetComputedLeft();
        var top = (int)Node->GetComputedTop();
        var width = (int)Node->GetComputedWidth();
        var height = (int)Node->GetComputedHeight();
        _ansiConsole.Cursor.MoveRight(left);
        _ansiConsole.Cursor.MoveDown(top);
        _ansiConsole.Write(renderable);
        _ansiConsole.Cursor.MoveLeft(left + width);
        _ansiConsole.Cursor.MoveUp(top + height - (inline ? 1 : 0));
        return new Size(width, height);
    }

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
}