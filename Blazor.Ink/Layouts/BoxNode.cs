using Blazor.Ink.Components;
using Blazor.Ink.Value;
using Spectre.Console;
using Spectre.Console.Rendering;
using YogaSharp;

namespace Blazor.Ink.Layouts;

public class BoxNode : NodeBase<Box>
{
    public BoxNode(IAnsiConsole ansiConsole) : base(ansiConsole) { }

    private struct FixedBoxBorder : IBorder
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
    protected override void ApplyLayoutImpl()
    {
        // --- Apply layout information to YogaSharp node ---
        // Padding
        unsafe
        {
            Component?.ApplyPaddingTo(Node);
            Component?.ApplyMarginTo(Node);
            FixedBoxBorder.Default.ApplyBorderTo(Node);
            
            // Gap
            Node->SetGap(YGGutter.All, Component!.Gap);
            Node->SetGap(YGGutter.Row, Component.RowGap);
            // Size
            if (Component.Width is not null)
            {
                Node->SetWidth(Component.Width.Value);
            }
            else
            {
                Node->SetWidthAuto();
            }

            if (Component.Height is not null)
            {
                Node->SetHeight(Component.Height.Value);
            }
            else
            {
                Node->SetHeightAuto();
            }

            if (Component.MinWidth is not null)
            {
                Node->SetMinWidth(Component.MinWidth.Value);
            }

            if (Component.MinHeight is not null)
            {
                Node->SetMinHeight(Component.MinHeight.Value);
            }
            // Flex
            Node->SetFlexGrow(Component.FlexGrow);
            Node->SetFlexShrink(Component.FlexShrink);
            Node->SetFlexBasis(Component.FlexBasis);
            Node->SetFlexDirection((YGFlexDirection)Component.FlexDirection);
            Node->SetFlexWrap((YGWrap)Component.FlexWrap);
            Node->SetAlignItems((YGAlign)Component.AlignItems);
            Node->SetAlignSelf((YGAlign)Component.AlignSelf);
            Node->SetJustifyContent((YGJustify)Component.JustifyContent);
            // Position
            Node->SetPositionType((YGPositionType)Component.Position);
            // Display
            Node->SetDisplay(Component.Display == Display.Flex ? YGDisplay.Flex : YGDisplay.None);
            // Overflow
            Node->SetOverflow((YGOverflow)Component.Overflow);
            // --- End of layout application ---
        }
    }

    public override RenderTree BuildRenderTree()
    {
        var size = GetSize();
        // var padding = GetPadding();
        var panel = new Panel(string.Empty)
        {
            Border = Component!.BorderStyle ?? BoxBorder.Square,
            BorderStyle = new Style(Component.BorderColor),
            Width = size.Width == 0 ? null : size.Width,
            Height = size.Height == 0 ? null : size.Height,
            // Padding = new Spectre.Console.Padding(padding.Top - 1, padding.Right, padding.Bottom, padding.Left)
        };
        // var margin = GetMargin();
        // var marginPanel = new Padder(panel, new Spectre.Console.Padding(
        //     margin.Top, margin.Right, margin.Bottom, margin.Left));
        var childrenRenderTree = Children.Select(it => it.BuildRenderTree()).ToList();
        
        return new RenderTree(() => Render(panel), childrenRenderTree);
    }
}