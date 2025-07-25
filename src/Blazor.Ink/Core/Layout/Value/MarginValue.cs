using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public struct MarginValue
{
    public int? Value { get; set; }
    internal float ValueAsFloat => Value ?? float.NaN;
    private MarginUnit _unit = MarginUnit.Point;

    public MarginUnit Unit
    {
        get => _unit;
        set
        {
            if (value is MarginUnit.Unspecified)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Invalid unit for Margin. Use Point, Percent, or Auto.");
            }

            _unit = value;
        }
    }

    public MarginValue()
    {
    }

    public static implicit operator MarginValue(int value)
    {
        return new MarginValue { Value = value, Unit = MarginUnit.Point };
    }

    public static implicit operator MarginValue(YGValue value)
    {
        return new MarginValue
        {
            Value = float.IsNaN(value.value) ? null : (int)value.value,
            Unit = value.unit.ToMarginUnit()
        };
    }
    
    public static implicit operator YGValue(MarginValue position)
    {
        return new YGValue
        {
            value = position.ValueAsFloat,
            unit = position.Unit.ToYogaUnit()
        };
    }

    public static implicit operator LayoutValue(MarginValue position)
    {
        return new LayoutValue
        {
            Value = position.Value,
            Unit = position.Unit.ToLayoutUnit()
        };
    }
}