using YogaSharp;

namespace Blazor.Ink.Components;

public interface IPadding
{
    int? Padding { get; }
    int? PaddingX { get; }
    int? PaddingY { get; }
    int? PaddingTop { get; }
    int? PaddingBottom { get; }
    int? PaddingLeft { get; }
    int? PaddingRight { get; }
}

public static class PaddingExtensions
{
    public static unsafe void ApplyPaddingTo(this IPadding self, YGNode* node)
    {
        if (self.PaddingTop.HasValue) node->SetPadding(YGEdge.Top, self.PaddingTop.Value);

        if (self.PaddingBottom.HasValue) node->SetPadding(YGEdge.Bottom, self.PaddingBottom.Value);

        if (self.PaddingLeft.HasValue) node->SetPadding(YGEdge.Left, self.PaddingLeft.Value);

        if (self.PaddingRight.HasValue) node->SetPadding(YGEdge.Right, self.PaddingRight.Value);

        if (self.PaddingX.HasValue) node->SetPadding(YGEdge.Horizontal, self.PaddingX.Value);

        if (self.PaddingY.HasValue) node->SetPadding(YGEdge.Vertical, self.PaddingY.Value);

        if (self.Padding.HasValue) node->SetPadding(YGEdge.All, self.Padding.Value);
    }
}