using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Helper;
using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public enum Display
{
    Flex = YGDisplay.YGDisplayFlex,
    None = YGDisplay.YGDisplayNone,
    Contents = YGDisplay.YGDisplayContents
}

internal static class InternalLayoutDisplayExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static YGDisplay ToYogaDisplay(this Display display)
        => display.UnsafeCast<Display, YGDisplay>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Display ToLayoutDisplay(this YGDisplay display)
        => display.UnsafeCast<YGDisplay, Display>();
}