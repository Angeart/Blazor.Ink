using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum PositionType
{
    Static = YGPositionType.YGPositionTypeStatic,
    Relative = YGPositionType.YGPositionTypeRelative,
    Absolute = YGPositionType.YGPositionTypeAbsolute
}

public static class InternalPositionTypeExtensions
{
    public static PositionType ToLayoutPositionType(this YGPositionType value)
    {
        return value.UnsafeCast<YGPositionType, PositionType>();
    }

    public static YGPositionType ToYogaPositionType(this PositionType value)
    {
        return value.UnsafeCast<PositionType, YGPositionType>();
    }
}