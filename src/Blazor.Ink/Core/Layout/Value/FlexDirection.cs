using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum FlexDirection
{
    Column = YGFlexDirection.YGFlexDirectionColumn,
    ColumnReverse = YGFlexDirection.YGFlexDirectionColumnReverse,
    Row = YGFlexDirection.YGFlexDirectionRow,
    RowReverse = YGFlexDirection.YGFlexDirectionRowReverse
}

internal static class InternalLayoutFlexDirectionExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGFlexDirection ToYogaFlexDirection(this FlexDirection flexDirection)
        => flexDirection.UnsafeCast<FlexDirection, YGFlexDirection>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static FlexDirection ToLayoutFlexDirection(this YGFlexDirection flexDirection)
        => flexDirection.UnsafeCast<YGFlexDirection, FlexDirection>();
}