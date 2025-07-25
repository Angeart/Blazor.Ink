using Blazor.Ink.Core;
using Blazor.Ink.Core.Components.Abstraction;
using Blazor.Ink.Core.Layout.Value;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Spectre.Console;
using Align = Blazor.Ink.Core.Layout.Value.Align;
using IRenderable = Spectre.Console.Rendering.IRenderable;
using Justify = Blazor.Ink.Core.Layout.Value.Justify;

namespace Blazor.Ink.Components;

/// <summary>
///     Equivalent to Ink's Box. TUI element with border, padding, margin, etc.
/// </summary>
public class Box : InkComponentBase, IHasPadding, IHasMargin, IHasFlex, IHasPosition
{
    [Parameter] public PositionType? Position { get; set; }
    [Parameter] public PositionValue? Top { get; set; }
    [Parameter] public PositionValue? Right { get; set; }
    [Parameter] public PositionValue? Bottom { get; set; }
    [Parameter] public PositionValue? Left { get; set; }
    [Parameter] public int RowGap { get; set; }
    [Parameter] public int Gap { get; set; }

    [Parameter] public Color? BorderColor { get; set; }

    [Parameter] public bool BorderDimColor { get; set; }
    [Parameter] public bool BorderTopDimColor { get; set; }
    [Parameter] public bool BorderBottomDimColor { get; set; }
    [Parameter] public bool BorderLeftDimColor { get; set; }
    [Parameter] public bool BorderRightDimColor { get; set; }
    [Parameter] public BoxBorder? BorderStyle { get; set; }

    [Parameter] public int? Width { get; set; }
    [Parameter] public int? Height { get; set; }
    [Parameter] public int? MinWidth { get; set; }
    [Parameter] public int? MinHeight { get; set; }

    public float? Flex { get; set; }
    [Parameter] public float? FlexGrow { get; set; }
    [Parameter] public float? FlexShrink { get; set; }
    [Parameter] public LayoutValue? FlexBasis { get; set; }
    [Parameter] public FlexDirection? FlexDirection { get; set; }
    [Parameter] public FlexWrap? FlexWrap { get; set; }
    [Parameter] public Align? Align { get; set; }
    [Parameter] public Align? AlignSelf { get; set; }
    [Parameter] public Justify? JustifyContent { get; set; }


    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public MarginValue? Margin { get; set; }
    [Parameter] public MarginValue? MarginX { get; set; }
    [Parameter] public MarginValue? MarginY { get; set; }
    [Parameter] public MarginValue? MarginTop { get; set; }
    [Parameter] public MarginValue? MarginBottom { get; set; }
    [Parameter] public MarginValue? MarginLeft { get; set; }
    [Parameter] public MarginValue? MarginRight { get; set; }
    [Parameter] public PaddingValue? Padding { get; set; }
    [Parameter] public PaddingValue? PaddingX { get; set; }
    [Parameter] public PaddingValue? PaddingY { get; set; }
    [Parameter] public PaddingValue? PaddingTop { get; set; }
    [Parameter] public PaddingValue? PaddingBottom { get; set; }
    [Parameter] public PaddingValue? PaddingLeft { get; set; }
    [Parameter] public PaddingValue? PaddingRight { get; set; }

    protected override void OnApplyLayout()
    {
        base.OnApplyLayout();
        FixedBoxBorder.Default.ApplyInto(ref LayoutNode);
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent ?? string.Empty.ToMarkupRenderFragment());
    }

    protected override IRenderable OnBuildElement()
    {
        var width = LayoutNode.Computed.Width;
        var height = LayoutNode.Computed.Height;
        return new Panel(string.Empty)
        {
            Border = BorderStyle ?? BoxBorder.Square,
            BorderStyle = new Style(BorderColor),
            Width = width == 0 ? null : width,
            Height = height == 0 ? null : height
        };
    }
    
    private struct FixedBoxBorder : IHasReadonlyBorder
    {
        public static FixedBoxBorder Default => new();
        public int? Border => 1;
        public int? BorderX => null;
        public int? BorderY => null;
        public int? BorderTop => null;
        public int? BorderBottom => null;
        public int? BorderLeft => null;
        public int? BorderRight => null;
    }
}