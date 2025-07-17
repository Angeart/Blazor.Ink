using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value.Config;

public enum Errata
{
    None = YGErrata.YGErrataNone,
    StreatchFlexBasis = YGErrata.YGErrataStretchFlexBasis,

    AbsolutePositionWIthoutInsetsExcludesPadding =
        YGErrata.YGErrataAbsolutePositionWithoutInsetsExcludesPadding,

    AbsolutePercentAgainstInnerSize =
        YGErrata.YGErrataAbsolutePercentAgainstInnerSize,
    Classic = YGErrata.YGErrataClassic,
    All = YGErrata.YGErrataAll
}

public static class LayoutConfigErrataExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGErrata ToYogaErrata(this Errata errata)
        => errata.UnsafeCast<Errata, YGErrata>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Errata ToLayoutConfigErrata(this YGErrata errata)
        => errata.UnsafeCast<YGErrata, Errata>();
}