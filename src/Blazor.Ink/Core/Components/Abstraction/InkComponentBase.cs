using Blazor.Ink.Components;
using Blazor.Ink.Core.Layout;
using Blazor.Ink.Core.Layout.Value;
using Blazor.Ink.Core.Renderer.Abstraction;
using Microsoft.AspNetCore.Components;
using static Spectre.Console.CursorExtensions;
using IAnsiConsole = Spectre.Console.IAnsiConsole;
using IRenderable = Spectre.Console.Rendering.IRenderable;

// ReSharper disable SuspiciousTypeConversion.Global

namespace Blazor.Ink.Core.Components.Abstraction;

public abstract class InkComponentBase : ComponentBase, IRenderableComponent, IDisposable, IInkComponent
{
    public IRenderable? RenderElement { get; private set; }
    /// <summary>
    /// The output of the component will be marked as Inline.
    /// This is used to correctly calculate the amount to move the cursor to reset its position after rendering.
    /// </summary>
    public virtual bool Inline => false;
    private LayoutNode _layoutNode = new();
    public ref LayoutNode LayoutNode => ref _layoutNode;

    #region Basic Parameters

    [Parameter] public Overflow? Overflow { get; set; } = Layout.Value.Overflow.Visible;

    [Parameter] public Display? Display { get; set; } = Layout.Value.Display.Flex;

    #endregion

    public void ApplyLayout()
    {
        OnApplyLayout();
    }

    protected virtual void OnApplyLayout()
    {
        if (Overflow.HasValue) _layoutNode.Overflow = Overflow.Value;
        if (Display.HasValue) _layoutNode.Display = Display.Value;
        if (this is IHasSize size) size.ApplyInto(ref _layoutNode);
        if (this is IHasPosition position) position.ApplyInto(ref _layoutNode);
        if (this is IHasMargin margin) margin.ApplyInto(ref _layoutNode);
        if (this is IHasPadding padding) padding.ApplyInto(ref _layoutNode);
        if (this is IHasBorder border) border.ApplyInto(ref _layoutNode);
        if (this is IHasFlex flex) flex.ApplyInto(ref _layoutNode);
    }

    public void BuildElement()
    {
        RenderElement = OnBuildElement();
    }
    
    /// <summary>
    /// Build the renderable element for this component.
    /// </summary>
    /// <remarks>
    /// This method is called during the rendering process after the component layout has been applied and calculated.
    /// </remarks>
    protected abstract IRenderable OnBuildElement();

    public Size Render(IAnsiConsole console)
    {
        if (RenderElement is null) return Size.Zero;
        var computed = _layoutNode.Computed;
        var left = computed.Left;
        var top = computed.Top;
        var width = computed.Width;
        var height = computed.Height;
        var cursor = console.Cursor;
        cursor.MoveRight(left);
        cursor.MoveDown(top);
        console.Write(RenderElement);
        cursor.MoveLeft(left + width);
        cursor.MoveUp(top + height - (Inline ? 1 : 0));
        return new Size(width, height);
    }

    public void Dispose()
    {
        _layoutNode.Dispose();
    }
}