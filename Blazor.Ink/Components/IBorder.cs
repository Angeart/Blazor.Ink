using YogaSharp;

namespace Blazor.Ink.Components;

public interface IBorder
{
    int? Border { get; }
    int? BorderX { get; }
    int? BorderY { get; }
    int? BorderTop { get; }
    int? BorderBottom { get; }
    int? BorderLeft { get; }
    int? BorderRight { get; }
}

public static class BorderExtensions
{
    public static unsafe void ApplyBorderTo(this IBorder self, YGNode* node)
    {
        if (self.BorderTop.HasValue)
        {
            node->SetBorder(YGEdge.Top, self.BorderTop.Value);
        }
        if (self.BorderBottom.HasValue)
        {
            node->SetBorder(YGEdge.Bottom, self.BorderBottom.Value);
        }
        if (self.BorderLeft.HasValue)
        {
            node->SetBorder(YGEdge.Left, self.BorderLeft.Value);
        }
        if (self.BorderRight.HasValue)
        {
            node->SetBorder(YGEdge.Right, self.BorderRight.Value);
        }
        if (self.BorderX.HasValue)
        {
            node->SetBorder(YGEdge.Horizontal, self.BorderX.Value);
        }
        if (self.BorderY.HasValue)
        {
            node->SetBorder(YGEdge.Vertical, self.BorderY.Value);
        }
        if (self.Border.HasValue)
        {
            node->SetBorder(YGEdge.All, self.Border.Value);
        }
    }
}
