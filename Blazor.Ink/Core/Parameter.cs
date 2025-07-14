using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public readonly struct Parameter : IEquatable<Parameter>
{
    public string? Name { get; }
    public object? Value { get; }
    public bool IsCascadingValue { get; }

    private Parameter(string? name, object? value, bool isCascadingValue)
    {
        if (isCascadingValue && value is null)
            throw new ArgumentNullException(nameof(value), "Cascading value cannot be null.");

        if (!isCascadingValue && name is null)
            throw new ArgumentNullException(nameof(name), "Non-cascading value must have a name.");

        Name = name;
        Value = value;
        IsCascadingValue = isCascadingValue;
    }

    public static Parameter CreateCascadingValue(string? name, object? value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value), "Cascading value cannot be null.");
        return new Parameter(name, value, true);
    }

    public static Parameter CreateComponentParameter(string name, object? value)
    {
        if (name is null) throw new ArgumentNullException(nameof(name), "Non-cascading value must have a name.");
        return new Parameter(name, value, false);
    }

    public static bool operator ==(Parameter? left, Parameter? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Parameter? left, Parameter? right)
    {
        return !(left == right);
    }

    public bool Equals(Parameter other)
    {
        return string.Equals(Name, other.Name, StringComparison.Ordinal) &&
               Equals(Value, other.Value) &&
               IsCascadingValue == other.IsCascadingValue;
    }

    public override bool Equals(object? obj)
    {
        return obj is Parameter other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value, IsCascadingValue);
    }
}

public static class ParameterExtensions
{
    internal static Type CascadingValueType = typeof(CascadingValue<>);

    public static bool ValidateCascadingValue(this in Parameter parameter)
    {
        if (parameter is { IsCascadingValue: true, Value: null })
            throw new ArgumentNullException(nameof(parameter.Value), "Cascading value cannot be null.");
        return true;
    }

    public static bool ValidateComponentParameter(this in Parameter parameter)
    {
        if (parameter is { IsCascadingValue: false, Name: null })
            throw new ArgumentNullException(nameof(parameter.Name), "Non-cascading value must have a name.");
        return true;
    }

    internal static Type GetCascadingValueType(this in Parameter parameter)
    {
        if (parameter.IsCascadingValue && parameter.Value is not null)
            return CascadingValueType.MakeGenericType(parameter.Value.GetType());
        throw new InvalidOperationException("Parameter is not a cascading value or has no value.");
    }
}