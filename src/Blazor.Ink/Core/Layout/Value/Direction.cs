using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

// ReSharper disable InconsistentNaming

namespace Blazor.Ink.Core.Layout.Value;

public enum Direction
{
    Inherit = YGDirection.YGDirectionInherit,
    LTR = YGDirection.YGDirectionLTR,
    RTL = YGDirection.YGDirectionRTL
}

internal static class InternalLayoutDirectionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGDirection ToYogaDirection(this Direction direction)
        => direction.UnsafeCast<Direction, YGDirection>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Direction ToLayoutDirection(this YGDirection direction)
        => direction.UnsafeCast<YGDirection, Direction>();
}