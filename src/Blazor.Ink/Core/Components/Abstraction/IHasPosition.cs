using Blazor.Ink.Core.Layout;
using Blazor.Ink.Core.Layout.Value;

namespace Blazor.Ink.Core.Components.Abstraction;

public interface IHasPosition
{
    PositionType? Position { get; set; }
    PositionValue? Top { get; set; }
    PositionValue? Right { get; set; }
    PositionValue? Bottom { get; set; }
    PositionValue? Left { get; set; }
}

public static class PositionExtensions
{
    public static void ApplyInto(this IHasPosition self, ref LayoutNode node)
    {
        if (self.Position.HasValue) node.Position = self.Position.Value;
        if (self.Top.HasValue) node.Top = self.Top.Value;
        if (self.Right.HasValue) node.Right = self.Right.Value;
        if (self.Bottom.HasValue) node.Bottom = self.Bottom.Value;
        if (self.Left.HasValue) node.Left = self.Left.Value;
    }
}