using Blazor.Ink.Core.Layout;
using Blazor.Ink.Core.Layout.Value;

namespace Blazor.Ink.Core.Components.Abstraction;

public interface IHasFlex
{
    public float? Flex { get; set; }
    public float? FlexGrow { get; set; }
    public float? FlexShrink { get; set; }
    public LayoutValue? FlexBasis { get; set; }
    public FlexDirection? FlexDirection { get; set; }
    public FlexWrap? FlexWrap { get; set; }
    public Align? Align { get; set; }
    public Align? AlignSelf { get; set; }
    public Justify? JustifyContent { get; set; }
}

public static class FlexExtensions
{
    public static void ApplyInto(this IHasFlex self, ref LayoutNode node)
    {
        if (self.FlexGrow.HasValue) node.FlexGrow = self.FlexGrow.Value;
        if (self.FlexShrink.HasValue) node.FlexShrink = self.FlexShrink.Value;
        if (self.FlexBasis.HasValue) node.FlexBasis = self.FlexBasis.Value;
        if (self.FlexDirection.HasValue) node.FlexDirection = self.FlexDirection.Value;
        if (self.FlexWrap.HasValue) node.FlexWrap = self.FlexWrap.Value;
        if (self.Align.HasValue) node.Align = self.Align.Value;
        if (self.AlignSelf.HasValue) node.AlignSelf = self.AlignSelf.Value;
        if (self.JustifyContent.HasValue) node.JustifyContent = self.JustifyContent.Value;
    }
}