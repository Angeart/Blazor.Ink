using Blazor.Ink.Core.Layout;
using YogaSharp;

namespace Blazor.Ink.Core.Components.Abstraction;

public interface IHasReadonlyBorder
{
    int? Border { get; }
    int? BorderX { get; }
    int? BorderY { get; }
    int? BorderTop { get; }
    int? BorderBottom { get; }
    int? BorderLeft { get; }
    int? BorderRight { get; }
}

public interface IHasBorder : IHasReadonlyBorder
{
    new int? Border { get; set; }
    new int? BorderX { get; set; }
    new int? BorderY { get; set; }
    new int? BorderTop { get; set; }
    new int? BorderBottom { get; set; }
    new int? BorderLeft { get; set; }
    new int? BorderRight { get; set; }
}

public static class BorderExtensions
{
    public static unsafe void ApplyBorderTo(this IHasReadonlyBorder self, YGNode* node)
    {
        if (self.Border.HasValue) node->SetBorder(YGEdge.All, self.Border.Value);
        if (self.BorderX.HasValue) node->SetBorder(YGEdge.Horizontal, self.BorderX.Value);
        if (self.BorderY.HasValue) node->SetBorder(YGEdge.Vertical, self.BorderY.Value);
        if (self.BorderTop.HasValue) node->SetBorder(YGEdge.Top, self.BorderTop.Value);
        if (self.BorderBottom.HasValue) node->SetBorder(YGEdge.Bottom, self.BorderBottom.Value);
        if (self.BorderLeft.HasValue) node->SetBorder(YGEdge.Left, self.BorderLeft.Value);
        if (self.BorderRight.HasValue) node->SetBorder(YGEdge.Right, self.BorderRight.Value);
    }

    public static void ApplyInto(this IHasReadonlyBorder self, ref LayoutNode node)
    {
        if (self.Border.HasValue) node.Border = self.Border.Value;
        if (self.BorderX.HasValue) node.BorderX = self.BorderX.Value;
        if (self.BorderY.HasValue) node.BorderY = self.BorderY.Value;
        if (self.BorderTop.HasValue) node.BorderTop = self.BorderTop.Value;
        if (self.BorderBottom.HasValue) node.BorderBottom = self.BorderBottom.Value;
        if (self.BorderLeft.HasValue) node.BorderLeft = self.BorderLeft.Value;
        if (self.BorderRight.HasValue) node.BorderRight = self.BorderRight.Value;
    }
}