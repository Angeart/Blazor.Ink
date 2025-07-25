using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum Align
{
    Auto = YGAlign.YGAlignAuto,
    FlexStart = YGAlign.YGAlignFlexStart,
    Center = YGAlign.YGAlignCenter,
    FlexEnd = YGAlign.YGAlignFlexEnd,
    Stretch = YGAlign.YGAlignStretch,
    Baseline = YGAlign.YGAlignBaseline,
    SpaceBetween = YGAlign.YGAlignSpaceBetween,
    SpaceAround = YGAlign.YGAlignSpaceAround
}

internal static class InternalAlignItemsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGAlign ToYogaAlign(this Align align)
        => align.UnsafeCast<Align, YGAlign>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Align ToLayoutAlign(this YGAlign align)
        => align.UnsafeCast<YGAlign, Align>();
}