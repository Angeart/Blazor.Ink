using YogaSharp;

namespace Blazor.Ink.Core.Components;

public interface IMargin
{
    int? Margin { get; }
    int? MarginX { get; }
    int? MarginY { get; }
    int? MarginTop { get; }
    int? MarginBottom { get; }
    int? MarginLeft { get; }
    int? MarginRight { get; }
}

public static class MarginExtensions
{
    public static unsafe void ApplyMarginTo(this IMargin self, YGNode* node)
    {
        if (self.MarginTop.HasValue)
        {
            node->SetMargin(YGEdge.Top, self.MarginTop.Value);
        }
        if (self.MarginBottom.HasValue)
        {
            node->SetMargin(YGEdge.Bottom, self.MarginBottom.Value);
        }
        if (self.MarginLeft.HasValue)
        {
            node->SetMargin(YGEdge.Left, self.MarginLeft.Value);
        }
        if (self.MarginRight.HasValue)
        {
            node->SetMargin(YGEdge.Right, self.MarginRight.Value);
        }
        if (self.MarginX.HasValue)
        {
            node->SetMargin(YGEdge.Horizontal, self.MarginX.Value);
        }
        if (self.MarginY.HasValue)
        {
            node->SetMargin(YGEdge.Vertical, self.MarginY.Value);
        }
        if (self.Margin.HasValue)
        {
            node->SetMargin(YGEdge.All, self.Margin.Value);
        }
    }
}
