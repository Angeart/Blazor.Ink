using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum Overflow
{
    Visible = YGOverflow.YGOverflowVisible,
    Hidden = YGOverflow.YGOverflowHidden,
    Scroll = YGOverflow.YGOverflowScroll
}

public static class LayoutOverflowExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Overflow ToLayoutOverflow(this YGOverflow value)
        => value.UnsafeCast<YGOverflow, Overflow>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGOverflow ToYogaOverflow(this Overflow value)
        => value.UnsafeCast<Overflow, YGOverflow>();
}