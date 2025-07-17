using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum Edge
{
    Left = YGEdge.YGEdgeLeft,
    Top = YGEdge.YGEdgeTop,
    Right = YGEdge.YGEdgeRight,
    Bottom = YGEdge.YGEdgeBottom,
    Start = YGEdge.YGEdgeStart,
    End = YGEdge.YGEdgeEnd,
    Horizontal = YGEdge.YGEdgeHorizontal,
    Vertical = YGEdge.YGEdgeVertical,
    All = YGEdge.YGEdgeAll
}

internal static class InternalLayoutEdgeExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGEdge ToYogaEdge(this Edge edge)
        => edge.UnsafeCast<Edge, YGEdge>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Edge ToLayoutEdge(this YGEdge edge)
        => edge.UnsafeCast<YGEdge, Edge>();
}