using Blazor.Ink.Core.Layout;
using Blazor.Ink.Core.Layout.Value;
using YogaSharp;

namespace Blazor.Ink.Core.Components.Abstraction;

public interface IHasPadding
{
    PaddingValue? Padding { get; set; }
    PaddingValue? PaddingX { get; set; }
    PaddingValue? PaddingY { get; set; }
    PaddingValue? PaddingTop { get; set; }
    PaddingValue? PaddingBottom { get; set; }
    PaddingValue? PaddingLeft { get; set; }
    PaddingValue? PaddingRight { get; set; }
}

public static class PaddingExtensions
{
    public static unsafe void ApplyPaddingTo(this IHasPadding self, YGNode* node)
    {
        if (self.Padding.HasValue) node->SetPadding(YGEdge.All, self.Padding.Value.ValueAsFloat);
        if (self.PaddingX.HasValue) node->SetPadding(YGEdge.Horizontal, self.PaddingX.Value.ValueAsFloat);
        if (self.PaddingY.HasValue) node->SetPadding(YGEdge.Vertical, self.PaddingY.Value.ValueAsFloat);
        if (self.PaddingTop.HasValue) node->SetPadding(YGEdge.Top, self.PaddingTop.Value.ValueAsFloat);
        if (self.PaddingBottom.HasValue) node->SetPadding(YGEdge.Bottom, self.PaddingBottom.Value.ValueAsFloat);
        if (self.PaddingLeft.HasValue) node->SetPadding(YGEdge.Left, self.PaddingLeft.Value.ValueAsFloat);
        if (self.PaddingRight.HasValue) node->SetPadding(YGEdge.Right, self.PaddingRight.Value.ValueAsFloat);
    }
    
    public static void ApplyInto(this IHasPadding self, ref LayoutNode node)
    {
        if (self.Padding.HasValue) node.Padding = self.Padding.Value;
        if (self.PaddingX.HasValue) node.PaddingX = self.PaddingX.Value;
        if (self.PaddingY.HasValue) node.PaddingY = self.PaddingY.Value;
        if (self.PaddingTop.HasValue) node.PaddingTop = self.PaddingTop.Value;
        if (self.PaddingBottom.HasValue) node.PaddingBottom = self.PaddingBottom.Value;
        if (self.PaddingLeft.HasValue) node.PaddingLeft = self.PaddingLeft.Value;
        if (self.PaddingRight.HasValue) node.PaddingRight = self.PaddingRight.Value;
    }
}