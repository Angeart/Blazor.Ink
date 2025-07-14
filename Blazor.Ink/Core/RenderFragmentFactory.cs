using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public static partial class RenderFragmentFactory
{
    public static RenderFragment CreateRenderFragment<TComponent>(ParameterCollection parameters)
        where TComponent : IComponent
    {
        var cascadingValues = GetCascadingValues(parameters);
        var type = typeof(TComponent);

        return cascadingValues.Count > 0
            ? AsCascadingValue(cascadingValues, parameters, type)
            : AsComponent(parameters, type);
    }

    public static RenderFragment CreateRenderFragment(ParameterCollection parameters, Type componentType)
    {
        var cascadingValues = GetCascadingValues(parameters);

        return cascadingValues.Count > 0
            ? AsCascadingValue(cascadingValues, parameters, componentType)
            : AsComponent(parameters, componentType);
    }
}