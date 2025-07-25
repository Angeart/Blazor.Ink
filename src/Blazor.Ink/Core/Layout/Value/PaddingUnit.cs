using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum PaddingUnit
{
    Unspecified = YGUnit.YGUnitUndefined,
    Point = YGUnit.YGUnitPoint,
    Percent = YGUnit.YGUnitPercent,
}

internal static class InternalPaddingUnitExtensions
{
    public static YGUnit ToYogaUnit(this PaddingUnit unit)
    {
        return unit.UnsafeCast<PaddingUnit, YGUnit>();
    }

    public static PaddingUnit ToPaddingUnit(this YGUnit unit)
    {
        return unit.UnsafeCast<YGUnit, PaddingUnit>();
    }

    public static LayoutUnit ToLayoutUnit(this PaddingUnit unit)
    {
        return unit switch
        {
            PaddingUnit.Unspecified => LayoutUnit.Unspecified,
            PaddingUnit.Point => LayoutUnit.Point,
            PaddingUnit.Percent => LayoutUnit.Percent,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }
}