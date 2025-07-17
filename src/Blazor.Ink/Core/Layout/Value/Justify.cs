using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum Justify
{
    FlexStart = YGJustify.YGJustifyFlexStart,
    Center = YGJustify.YGJustifyCenter,
    FlexEnd = YGJustify.YGJustifyFlexEnd,
    SpaceBetween = YGJustify.YGJustifySpaceBetween,
    SpaceAround = YGJustify.YGJustifySpaceAround,
    SpaceEvenly = YGJustify.YGJustifySpaceEvenly
}

internal static class InternalLayoutJustifyExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGJustify ToYogaJustify(this Justify justify)
        => justify.UnsafeCast<Justify, YGJustify>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Justify ToLayoutJustify(this YGJustify justify)
        => justify.UnsafeCast<YGJustify, Justify>();
}