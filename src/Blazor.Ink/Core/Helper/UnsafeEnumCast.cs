using System.Runtime.CompilerServices;

namespace Blazor.Ink.Core.Helper;

public static class UnsafeEnumCast
{
    public static TTo UnsafeCast<TFrom, TTo>(this TFrom value)
        where TFrom : struct, Enum
        where TTo : struct, Enum
    {
        return Unsafe.As<TFrom, TTo>(ref value);
    }
}