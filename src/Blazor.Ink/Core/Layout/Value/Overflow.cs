using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

/// <summary>
/// Overflow behavior
/// </summary>
public enum Overflow
{
    Visible = YGOverflow.YGOverflowVisible,
    Hidden = YGOverflow.YGOverflowHidden,
}

internal static class InternalOverflowExtensions
{
    /// <summary>
    /// Converts the Overflow enum to its corresponding Yoga value.
    /// </summary>
    /// <param name="overflow">The Overflow enum value.</param>
    /// <returns>The corresponding YogaOverflow value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGOverflow ToYogaOverflow(this Overflow overflow)
        => overflow.UnsafeCast<Overflow, YGOverflow>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Overflow ToLayoutOverflow(this YGOverflow overflow)
    {
        return overflow switch
        {
            YGOverflow.YGOverflowVisible => Overflow.Visible,
            YGOverflow.YGOverflowHidden => Overflow.Hidden,
            YGOverflow.YGOverflowScroll => throw new NotSupportedException("YGOverflowScroll is not supported in Ink."),
            _ => throw new ArgumentOutOfRangeException(nameof(overflow), overflow, null)
        };
    }
}