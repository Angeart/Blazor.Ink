using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Ink.Core;

public static partial class RenderFragmentFactory
{
    internal static RenderFragment AsCascadingValue(
        Queue<(Parameter Parameter, Type Type)> entries,
        ParameterCollection parameters,
        Type insideComponentType)
    {
        void RenderFragment(RenderTreeBuilder builder)
        {
            var value = entries.Dequeue();
            builder.OpenComponent(0, value.Type);
            if (value.Parameter.Name is not null)
                builder.AddAttribute(1, nameof(CascadingValue<object>.Name), value.Parameter.Name);

            builder.AddAttribute(2, nameof(CascadingValue<object>.Value), value.Parameter.Value);
            builder.AddAttribute(3, nameof(CascadingValue<object>.IsFixed), true);

            if (entries.Count > 0)
                builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent),
                    AsCascadingValue(entries, parameters, insideComponentType));
            else
                builder.AddAttribute(4, nameof(CascadingValue<object>.ChildContent),
                    AsComponent(parameters, insideComponentType));

            builder.CloseComponent();
        }

        return RenderFragment;
    }

    private static Queue<(Parameter Parameter, Type Type)> GetCascadingValues(ParameterCollection parameters)
    {
        var cascadingParameters = parameters
            .Where(it => it.IsCascadingValue)
            .Select(it => (Parameter: it, Type: it.GetCascadingValueType()))
            .ToArray();

        // Detect unnamed duplicates types or same named
        for (var i = 0; i < cascadingParameters.Length; i++)
        for (var j = i + 1; j < cascadingParameters.Length; j++)
            if (cascadingParameters[i].Type == cascadingParameters[j].Type)
            {
                var name = cascadingParameters[i].Parameter.Name;
                if (name is null)
                {
                    var type = cascadingParameters[i].Type;
                    var valueType = type.GetGenericArguments()[0];
                    throw new ArgumentException(
                        $"""
                         Duplicate unnamed cascading value type detected: {valueType.Name}.
                         Ensure that you do not have multiple cascading values of the same type without a name.
                         """);
                }

                if (name.Equals(cascadingParameters[j].Parameter.Name))
                    throw new ArgumentException(
                        $"""
                         Duplicate same named with same type cascading value detected: {name}.
                         Ensure that you do not have multiple cascading values with the same name and type.
                         """);
            }

        return new Queue<(Parameter Parameter, Type Type)>(cascadingParameters);
    }
}