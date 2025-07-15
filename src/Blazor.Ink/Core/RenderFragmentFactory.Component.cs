using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Ink.Core;

[SuppressMessage("Usage", "ASP0006:Do not use non-literal sequence numbers")]
public static partial class RenderFragmentFactory
{
    private static readonly MethodInfo MakeGenericRenderFragmentMethod =
        GetMakeGenericRenderFragmentMethod();

    internal static RenderFragment AsComponent(ParameterCollection parameters, Type componentType)
    {
        var componentParameters = GetComponentParameters(parameters);

        void RenderFragment(RenderTreeBuilder builder)
        {
            builder.OpenComponent(0, componentType);
            AddAttributes(builder, componentParameters);
#if NET9_0_OR_GREATER
            builder.AddComponentRenderMode(parameters.RenderMode);
#endif
            builder.CloseComponent();
        }

        return RenderFragment;
    }

    private static void AddAttributes(RenderTreeBuilder builder, Parameter[] parameters)
    {
        var sequence = 1;
        var grouped = parameters.GroupBy(p => p.Name);
        foreach (var item in grouped)
        {
            var group = item.ToArray();
            if (group.Length == 1)
            {
                var p = group[0];
                builder.AddAttribute(sequence++, p.Name!, p.Value);
                continue;
            }

            var nonNullObject = group.FirstOrDefault(p => p.Value is not null).Value;
            if (nonNullObject is RenderFragment)
            {
                // Add multiple RenderFragments as combined RenderFragment.
                var name = group[0].Name!;
                builder.AddAttribute(sequence++, name, (RenderFragment)(insideBuilder =>
                {
                    for (var i = 0; i < group.Length; i++)
                        if (group[i].Value is RenderFragment renderFragment)
                            insideBuilder.AddContent(i, renderFragment);
                }));

                continue;
            }

            var groupType = nonNullObject?.GetType();
            if (groupType is not null &&
                groupType.IsGenericType &&
                groupType.GetGenericTypeDefinition() == typeof(RenderFragment<>))
            {
                // Generic RenderFragment, e.g., RenderFragment<T>.
                var name = group[0].Name!;
                builder.AddAttribute(
                    sequence++,
                    name,
                    MakeGenericRenderFragment(groupType.GetGenericArguments()[0], group));

                continue;
            }

            throw new ArgumentException(
                $"Parameter '{item.Key}' has multiple values, this parameter must be a single.");
        }
    }

    private static object MakeGenericRenderFragment(Type genericParameterType, Parameter[] parameters)
    {
        var method = MakeGenericRenderFragmentMethod
            .MakeGenericMethod(genericParameterType);

        return method.Invoke(null, [parameters]) ??
               throw new InvalidOperationException(
                   $"Failed to invoke method '{nameof(InternalMakeGenericRenderFragment)}' with type '{genericParameterType.Name}'.");
    }

    private static MethodInfo GetMakeGenericRenderFragmentMethod()
    {
        var method = typeof(RenderFragmentFactory)
            .GetMethod(nameof(InternalMakeGenericRenderFragment), BindingFlags.NonPublic | BindingFlags.Static);
        return method ?? throw new InvalidOperationException(
            $"Method '{nameof(InternalMakeGenericRenderFragment)}' not found in '{nameof(RenderFragmentFactory)}'.");
    }

    private static RenderFragment<T> InternalMakeGenericRenderFragment<T>(Parameter[] parameters)
    {
        return input => builder =>
        {
            foreach (var parameter in parameters)
                if (parameter.Value is RenderFragment<T> renderFragment)
                    builder.AddContent(0, renderFragment(input));
                else
                    throw new InvalidOperationException(
                        $"Parameter '{parameter.Name}' is not a RenderFragment<{typeof(T).Name}>.");
        };
    }

    private static Parameter[] GetComponentParameters(ParameterCollection parameters)
    {
        return parameters
            .Where(it => !it.IsCascadingValue)
            .ToArray();
    }
}