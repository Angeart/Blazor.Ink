using Yoga;

namespace Blazor.Ink.Core.Layout.Value;

public struct PaddingValue
{
    public int? Value { get; set; }
    internal float ValueAsFloat => Value ?? float.NaN;
    private PaddingUnit _unit = PaddingUnit.Point;

    public PaddingUnit Unit
    {
        get => _unit;
        set
        {
            if (value is PaddingUnit.Unspecified)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Invalid unit for Padding. Use Point, Percent, or Auto.");
            }

            _unit = value;
        }
    }

    public PaddingValue()
    {
    }

    public static implicit operator PaddingValue(int value)
    {
        return new PaddingValue { Value = value, Unit = PaddingUnit.Point };
    }

    public static implicit operator PaddingValue(YGValue value)
    {
        return new PaddingValue
        {
            Value = float.IsNaN(value.value) ? null : (int)value.value,
            Unit = value.unit.ToPaddingUnit()
        };
    }
    
    public static implicit operator YGValue(PaddingValue position)
    {
        return new YGValue
        {
            value = position.ValueAsFloat,
            unit = position.Unit.ToYogaUnit()
        };
    }

    public static implicit operator LayoutValue(PaddingValue position)
    {
        return new LayoutValue
        {
            Value = position.Value,
            Unit = position.Unit.ToLayoutUnit()
        };
    }
}