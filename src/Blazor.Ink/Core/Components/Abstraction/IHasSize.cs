using Blazor.Ink.Core.Layout;
using Blazor.Ink.Core.Layout.Value;

namespace Blazor.Ink.Core.Components.Abstraction;

public interface IHasSize
{
    LayoutValue? Width { get; set; }
    LayoutValue? Height { get; set; }
    AspectRatio? AspectRatio { get; set; }
}

public static class SizeExtensions
{
    public static void ApplyInto(this IHasSize self, ref LayoutNode node)
    {
        if (self.Width.HasValue) node.Width = self.Width.Value;
        if (self.Height.HasValue) node.Height = self.Height.Value;
        if (self.AspectRatio.HasValue) node.AspectRatio = self.AspectRatio.Value;
    }
}