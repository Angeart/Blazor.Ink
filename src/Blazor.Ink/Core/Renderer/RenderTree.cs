using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using Blazor.Ink.Core.Renderer.Abstraction;
using Spectre.Console;
using Yoga;
using Size = Blazor.Ink.Core.Layout.Value.Size;
using Text = Blazor.Ink.Components.Text;

namespace Blazor.Ink.Core.Renderer;

public struct RenderTree(IRenderableComponent? component) : IDisposable
{
    private const int MinimumRentSize = 32;
    private static readonly ArrayPool<RenderTree> Pool = ArrayPool<RenderTree>.Shared;
    private RenderTree[]? _children = Pool.Rent(MinimumRentSize);
    private int _childCount = 0;
    private IRenderableComponent? _component = component;

    internal bool TryApplyText(string text)
    {
        if (_component is not Text textComponent)
        {
            return false;
        }

        textComponent.ApplyText(text);
        return true;
    }

    public void AddChild(RenderTree child)
    {
        DoValidate();

        if (_childCount >= _children.Length)
        {
            var newSize = _children.Length * 2;
            var newChildren = Pool.Rent(newSize);
            Array.Copy(_children, newChildren, _children.Length);
            Pool.Return(_children);
            _children = newChildren;
        }

        if (child._component is not null)
        {
            _component?.LayoutNode.InsertChild(child._component.LayoutNode, _childCount);
        }

        _children[_childCount++] = child;
    }

    public void Clear()
    {
        DoValidate();

        var span = _children.AsSpan(0, _childCount);
        foreach (ref var child in span)
        {
            child = default;
        }

        _childCount = 0;
    }

    public void Dispose()
    {
        if (_children != null)
        {
            Pool.Return(_children);
            _children = null;
        }

        _childCount = 0;
        _component = null;
    }

    public Size Execute(IAnsiConsole console)
    {
        ApplyLayout();
        CalculateLayout();
        BuildElement();
        return Render(console);
    }

    private void CalculateLayout()
    {
        DoValidate();

        _component?.LayoutNode.CalculateLayout(float.NaN, float.NaN, YGDirection.YGDirectionLTR);
        if (_component is null)
        {
            // NOTE: Directly apply layout to children if no component is set.
            var span = _children.AsSpan(0, _childCount);
            foreach (ref var child in span)
            {
                child.CalculateLayout();
            }
        }
    }

    private void ApplyLayout()
    {
        DoValidate();

        _component?.ApplyLayout();
        var span = _children.AsSpan(0, _childCount);
        foreach (ref var child in span)
        {
            child.ApplyLayout();
        }
    }

    private void BuildElement()
    {
        DoValidate();

        _component?.BuildElement();
        var span = _children.AsSpan(0, _childCount);
        foreach (ref var child in span)
        {
            child.BuildElement();
        }
    }

    private Size Render(IAnsiConsole console)
    {
        DoValidate();

        var size = _component?.Render(console) ?? Size.Zero;
        var span = _children.AsSpan(0, _childCount);
        foreach (ref var child in span)
        {
            var childSize = child.Render(console);
            size = Size.Max(size, childSize);
        }

        return size;
    }

    [MemberNotNull(nameof(_children))]
    private void DoValidate()
    {
        if (_children == null)
        {
            throw new InvalidOperationException("RenderTree has been disposed.");
        }
    }
}

public static class RenderTreeExtensions
{
    public static ref RenderTree AddText(ref this RenderTree self, string text)
    {
        if (self.TryApplyText(text)) return ref self;
        var textComponent = Text.Create(text);
        textComponent.ApplyText(text);
        self.AddChild(new RenderTree(textComponent));
        return ref self;
    }
}