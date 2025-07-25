using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum MarginUnit
{
    Unspecified = YGUnit.YGUnitUndefined,
    Point = YGUnit.YGUnitPoint,
    Percent = YGUnit.YGUnitPercent,
    Auto = YGUnit.YGUnitAuto
}

internal static class InternalMarginUnitExtensions
{
    public static YGUnit ToYogaUnit(this MarginUnit unit)
    {
        return unit.UnsafeCast<MarginUnit, YGUnit>();
    }

    public static MarginUnit ToMarginUnit(this YGUnit unit)
    {
        return unit.UnsafeCast<YGUnit, MarginUnit>();
    }

    public static LayoutUnit ToLayoutUnit(this MarginUnit unit)
    {
        return unit switch
        {
            MarginUnit.Unspecified => LayoutUnit.Unspecified,
            MarginUnit.Point => LayoutUnit.Point,
            MarginUnit.Percent => LayoutUnit.Percent,
            MarginUnit.Auto => LayoutUnit.Auto,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }
}