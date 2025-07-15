using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public sealed partial class ComponentParametersBuilder<TComponent>
    where TComponent : IComponent
{
    private static readonly bool HasUnmatchedCaptureParameter =
        typeof(TComponent).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(it => it.GetCustomAttribute<ParameterAttribute>())
            .Any(it => it is { CaptureUnmatchedValues: true });
    
    public ComponentParametersBuilder<TComponent> AddUnmatched(
        string name,
        object? value)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException("Parameter name cannot be null or empty.", nameof(name));
        }
        
        if (!HasUnmatchedCaptureParameter)
        {
            throw new InvalidOperationException(
                $"The component {typeof(TComponent).Name} does not support unmatched parameters. " +
                "Ensure that the component has a property with [Parameter(CaptureUnmatchedValues = true)] attribute.");
        }

        return AddComponentParameter(name, value);
    }
}