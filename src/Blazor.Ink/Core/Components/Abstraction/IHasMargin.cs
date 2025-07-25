using Blazor.Ink.Core.Layout;
using Blazor.Ink.Core.Layout.Value;
using YogaSharp;

namespace Blazor.Ink.Core.Components.Abstraction;

public interface IHasMargin
{
    MarginValue? Margin { get; set; }
    MarginValue? MarginX { get; set; }
    MarginValue? MarginY { get; set; }
    MarginValue? MarginTop { get; set; }
    MarginValue? MarginBottom { get; set; }
    MarginValue? MarginLeft { get; set; }
    MarginValue? MarginRight { get; set; }
}

public static class MarginExtensions
{
    public static unsafe void ApplyMarginTo(this IHasMargin self, YGNode* node)
    {
        if (self.Margin.HasValue) node->SetMargin(YGEdge.All, self.Margin.Value.ValueAsFloat);
        if (self.MarginX.HasValue) node->SetMargin(YGEdge.Horizontal, self.MarginX.Value.ValueAsFloat);
        if (self.MarginY.HasValue) node->SetMargin(YGEdge.Vertical, self.MarginY.Value.ValueAsFloat);
        if (self.MarginTop.HasValue) node->SetMargin(YGEdge.Top, self.MarginTop.Value.ValueAsFloat);
        if (self.MarginBottom.HasValue) node->SetMargin(YGEdge.Bottom, self.MarginBottom.Value.ValueAsFloat);
        if (self.MarginLeft.HasValue) node->SetMargin(YGEdge.Left, self.MarginLeft.Value.ValueAsFloat);
        if (self.MarginRight.HasValue) node->SetMargin(YGEdge.Right, self.MarginRight.Value.ValueAsFloat);
    }

    public static void ApplyInto(this IHasMargin self, ref LayoutNode node)
    {
        if (self.Margin.HasValue) node.Margin = self.Margin.Value;
        if (self.MarginX.HasValue) node.MarginX = self.MarginX.Value;
        if (self.MarginY.HasValue) node.MarginY = self.MarginY.Value;
        if (self.MarginTop.HasValue) node.MarginTop = self.MarginTop.Value;
        if (self.MarginBottom.HasValue) node.MarginBottom = self.MarginBottom.Value;
        if (self.MarginLeft.HasValue) node.MarginLeft = self.MarginLeft.Value;
        if (self.MarginRight.HasValue) node.MarginRight = self.MarginRight.Value;
    }
}