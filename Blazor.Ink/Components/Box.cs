using Blazor.Ink.Value;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Spectre.Console;
using Value_Overflow = Blazor.Ink.Value.Overflow;

namespace Blazor.Ink.Components;

/// <summary>
///     Equivalent to Ink's Box. TUI element with border, padding, margin, etc.
/// </summary>
public class Box : ComponentBase, IInkComponent, IPadding, IMargin
{
    [Parameter] public Position Position { get; set; } = Position.Relative;
    [Parameter] public int RowGap { get; set; } = 0;
    [Parameter] public int Gap { get; set; } = 0;

    [Parameter] public Color? BorderColor { get; set; }

    [Parameter] public bool BorderDimColor { get; set; } = false;
    [Parameter] public bool BorderTopDimColor { get; set; } = false;
    [Parameter] public bool BorderBottomDimColor { get; set; } = false;
    [Parameter] public bool BorderLeftDimColor { get; set; } = false;
    [Parameter] public bool BorderRightDimColor { get; set; } = false;
    [Parameter] public BoxBorder? BorderStyle { get; set; }

    [Parameter] public int? Width { get; set; }
    [Parameter] public int? Height { get; set; }
    [Parameter] public int? MinWidth { get; set; }
    [Parameter] public int? MinHeight { get; set; }

    [Parameter] public Display Display { get; set; } = Display.Flex;
    [Parameter] public Value_Overflow Overflow { get; set; } = Value_Overflow.Visible;


    [Parameter] public int FlexGrow { get; set; } = 0;
    [Parameter] public int FlexShrink { get; set; } = 1;
    [Parameter] public int FlexBasis { get; set; } = 0;
    [Parameter] public FlexDirection FlexDirection { get; set; } = FlexDirection.Row;
    [Parameter] public FlexWrap FlexWrap { get; set; } = FlexWrap.NoWrap;
    [Parameter] public AlignItems AlignItems { get; set; } = AlignItems.Stretch;
    [Parameter] public AlignSelf AlignSelf { get; set; } = AlignSelf.Auto;
    [Parameter] public JustifyContent JustifyContent { get; set; } = JustifyContent.FlexStart;


    [Parameter] public RenderFragment? ChildContent { get; set; }

    [Parameter] public int? Margin { get; set; }
    [Parameter] public int? MarginX { get; set; }
    [Parameter] public int? MarginY { get; set; }
    [Parameter] public int? MarginTop { get; set; }
    [Parameter] public int? MarginBottom { get; set; }
    [Parameter] public int? MarginLeft { get; set; }
    [Parameter] public int? MarginRight { get; set; }
    [Parameter] public int? Padding { get; set; }
    [Parameter] public int? PaddingX { get; set; }
    [Parameter] public int? PaddingY { get; set; }
    [Parameter] public int? PaddingTop { get; set; }
    [Parameter] public int? PaddingBottom { get; set; }
    [Parameter] public int? PaddingLeft { get; set; }
    [Parameter] public int? PaddingRight { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent);
    }
}