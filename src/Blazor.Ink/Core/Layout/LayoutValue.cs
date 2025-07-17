using Yoga;

namespace Blazor.Ink.Core.Layout;

public struct LayoutValue
{
    public float Value { get; set; }

    public float? SafeValue
    {
        get => float.IsNaN(Value) ? null : Value;
        set => Value = value ?? YG.YGUndefined;
    }
    
    public LayoutUnit Unit { get; set; }

    public static implicit operator LayoutValue(YGValue value)
    {
        return new LayoutValue
        {
            Value = value.value,
            Unit = value.unit.ToLayoutUnit()
        };
    }

    public static implicit operator YGValue(LayoutValue value)
    {
        return new YGValue
        {
            value = value.Value,
            unit = value.Unit.ToYogaUnit()
        };
    }
}