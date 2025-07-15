using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using IComponent = Microsoft.AspNetCore.Components.IComponent;

namespace Blazor.Ink.Core;

public sealed partial class ComponentParametersBuilder<TComponent>
    where TComponent : IComponent
{
    private static readonly Type ComponentType = typeof(TComponent);
    private readonly ParameterCollection _parameters = new();
    public delegate ComponentParametersBuilder<TComponent> BuilderFunction(ComponentParametersBuilder<TComponent> builder);

    public ParameterCollection Build() => _parameters;

#if NET9_0_OR_GREATER
    public ComponentParametersBuilder<TComponent> SetRenderMode(IComponentRenderMode renderMode)
    {
        _parameters.RenderMode = renderMode;
        return this;
    }
#endif

    private ComponentParametersBuilder<TComponent> AddComponentParameter<TValue>(string name, TValue value)
    {
        _parameters.Add(Parameter.CreateComponentParameter(name, value));
        return this;
    }

    private ComponentParametersBuilder<TComponent> AddCascadingValueParameter<TValue>(string? name, TValue value)
    {
        _parameters.Add(Parameter.CreateCascadingValue(name, value));
        return this;
    }

    private static (string PropertyName, string? CascadingValueName, bool IsCascading) GetParameterInfo<TValue>(
        Expression<Func<TComponent, TValue>> selector
#if NET8_0_OR_GREATER
        , object? value
#endif
    )
    {
        if (selector.Body is not MemberExpression { Member: PropertyInfo propertyInfo })
        {
            throw new ArgumentException(
                $"The selector has must be a public property on the component of {nameof(TComponent)}.");
        }

        var parameterAttr = propertyInfo.GetCustomAttribute<ParameterAttribute>();
#if !NET8_0_OR_GREATER
        var cascadingParameterAttr = propertyInfo.GetCustomAttribute<CascadingParameterAttribute>();

        if (parameterAttr is null && cascadingParameterAttr is null)
        {
            throw new ArgumentException(
                $"The property '{propertyInfo.Name}' on component '{ComponentType.Name}' must be decorated with either [Parameter] or [CascadingParameter] attribute.");
        }

        return (propertyInfo.Name, cascadingParameterAttr?.Name, cascadingParameterAttr is not null);
#else
        var cascadingParameterAttrBase = propertyInfo.GetCustomAttribute<CascadingParameterAttributeBase>();

        if (parameterAttr is null && cascadingParameterAttrBase is null)
        {
            throw new ArgumentException(
                $"The property '{propertyInfo.Name}' on component '{ComponentType.Name}' must be decorated with either [Parameter] or [CascadingParameter] attribute.");
        }

        if (cascadingParameterAttrBase is null)
        {
            return (propertyInfo.Name, null, false);
        }

        var cascadingParameterName = cascadingParameterAttrBase switch
        {
            CascadingParameterAttribute a => a.Name,
            SupplyParameterFromQueryAttribute a => throw new ArgumentException(
                $"""
                 To pass a value to a SupplyParameterFromQuery parameter,
                 use the NavigationManager and navigate via it to the URI.
                 Example:

                 var uri = NavigationManager.GetUriWithQueryParameters("{a.Name ?? propertyInfo.Name}", "{value}");
                    NavigationManager.NavigateTo(uri);
                 """),
            _ => throw new NotSupportedException(
                $"Unsupported cascading parameter attribute type: {cascadingParameterAttrBase.GetType().Name}.")
        };

        return (propertyInfo.Name, cascadingParameterName, true);
#endif
    }

    private static RenderFragment GetRenderFragment<TChildComponent>(
        Action<ComponentParametersBuilder<TChildComponent>>? childParameterBuilder)
        where TChildComponent : IComponent
    {
        var builder = new ComponentParametersBuilder<TChildComponent>();
        childParameterBuilder?.Invoke(builder);
        return builder.Build().AsRenderFragment<TChildComponent>();
    }
}