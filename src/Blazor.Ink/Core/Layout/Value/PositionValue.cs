using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public struct PositionValue
{
    public int? Value { get; set; }
    internal float ValueAsFloat => Value ?? float.NaN;
    private LayoutUnit _unit = LayoutUnit.Point;

    public LayoutUnit Unit
    {
        get => _unit;
        set
        {
            if (value is LayoutUnit.Unspecified
                or LayoutUnit.MaxContent
                or LayoutUnit.FitContent
                or LayoutUnit.Stretch)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Invalid unit for Position. Use Point, Percent, or Auto.");
            }

            _unit = value;
        }
    }

    public PositionValue()
    {
    }

    public static implicit operator PositionValue(int value)
    {
        return new PositionValue { Value = value, Unit = LayoutUnit.Point };
    }

    public static implicit operator PositionValue(YGValue value)
    {
        return new PositionValue
        {
            Value = float.IsNaN(value.value) ? null : (int)value.value,
            Unit = value.unit.ToLayoutUnit()
        };
    }
    
    public static implicit operator YGValue(PositionValue position)
    {
        return new YGValue
        {
            value = position.ValueAsFloat,
            unit = position.Unit.ToYogaUnit()
        };
    }

    public static implicit operator LayoutValue(PositionValue position)
    {
        return new LayoutValue
        {
            Value = position.Value,
            Unit = position.Unit
        };
    }
}