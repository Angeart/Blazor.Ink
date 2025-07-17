using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout;

public enum LayoutPositionType
{
    Static = YGPositionType.YGPositionTypeStatic,
    Relative = YGPositionType.YGPositionTypeRelative,
    Absolute = YGPositionType.YGPositionTypeAbsolute
}

public static class LayoutPositionTypeExtensions
{
    public static LayoutPositionType ToLayoutPositionType(this YGPositionType value)
    {
        return value.UnsafeCast<YGPositionType, LayoutPositionType>();
    }

    public static YGPositionType ToYogaPositionType(this LayoutPositionType value)
    {
        return value.UnsafeCast<LayoutPositionType, YGPositionType>();
    }
}