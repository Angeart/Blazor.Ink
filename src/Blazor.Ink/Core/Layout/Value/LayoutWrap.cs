using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout;

public enum LayoutWrap
{
    NoWrap = YGWrap.YGWrapNoWrap,
    Wrap = YGWrap.YGWrapWrap,
    WrapReverse = YGWrap.YGWrapWrapReverse
}

public static class LayoutWrapExtensions
{
    public static LayoutWrap ToLayoutWrap(this YGWrap value)
    {
        return value.UnsafeCast<YGWrap, LayoutWrap>();
    }

    public static YGWrap ToYogaWrap(this LayoutWrap value)
    {
        return value.UnsafeCast<LayoutWrap, YGWrap>();
    }
}