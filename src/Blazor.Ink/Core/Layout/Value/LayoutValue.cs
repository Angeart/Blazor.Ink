using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public struct LayoutValue
{
    public int? Value { get; set; }
    internal float ValueAsFloat => Value ?? float.NaN;
    
    public LayoutUnit Unit { get; set; }

    public static implicit operator LayoutValue(int value)
    {
        return new LayoutValue
        {
            Value = value,
            Unit = LayoutUnit.Point
        };
    }

    public static implicit operator LayoutValue(YGValue value)
    {
        return new LayoutValue
        {
            Value = float.IsNaN(value.value) ? null : (int)value.value,
            Unit = value.unit.ToLayoutUnit()
        };
    }

    public static implicit operator YGValue(LayoutValue value)
    {
        return new YGValue
        {
            value = value.Value ?? float.NaN,
            unit = value.Unit.ToYogaUnit()
        };
    }
}