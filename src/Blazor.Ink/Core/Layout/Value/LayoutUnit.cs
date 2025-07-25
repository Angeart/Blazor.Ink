using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum LayoutUnit
{
    Unspecified = YGUnit.YGUnitUndefined,
    Point = YGUnit.YGUnitPoint,
    Percent = YGUnit.YGUnitPercent,
    Auto = YGUnit.YGUnitAuto,
    MaxContent = YGUnit.YGUnitMaxContent,
    FitContent = YGUnit.YGUnitFitContent,
    Stretch = YGUnit.YGUnitStretch
}

internal static class InternalLayoutUnitExtensions
{
    public static YGUnit ToYogaUnit(this LayoutUnit unit)
    {
        return unit.UnsafeCast<LayoutUnit, YGUnit>();
    }

    public static LayoutUnit ToLayoutUnit(this YGUnit unit)
    {
        return unit.UnsafeCast<YGUnit, LayoutUnit>();
    }
}