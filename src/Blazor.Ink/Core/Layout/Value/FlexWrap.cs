using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum FlexWrap
{
    NoWrap = YGWrap.YGWrapNoWrap,
    Wrap = YGWrap.YGWrapWrap,
    WrapReverse = YGWrap.YGWrapWrapReverse
}

public static class LayoutWrapExtensions
{
    public static FlexWrap ToLayoutWrap(this YGWrap value)
    {
        return value.UnsafeCast<YGWrap, FlexWrap>();
    }

    public static YGWrap ToYogaWrap(this FlexWrap value)
    {
        return value.UnsafeCast<FlexWrap, YGWrap>();
    }
}