using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public sealed partial class ComponentParametersBuilder<TComponent>
    where TComponent : IComponent
{
    public ComponentParametersBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> selector,
        TValue value)
    {
#if NET8_0_OR_GREATER
        var (propertyName, cascadingValueName, isCascading) = GetParameterInfo(selector, value);
#else
        var (propertyName, cascadingValueName, isCascading) = GetParameterInfo(selector);
#endif
        return isCascading
            ? AddCascadingValue(cascadingValueName ?? propertyName, value)
            : AddComponentParameter(propertyName, value);
    }

    public ComponentParametersBuilder<TComponent> Add<TChildComponent>(
        Expression<Func<TComponent, RenderFragment>> selector,
        Action<ComponentParametersBuilder<TChildComponent>>? childParameterBuilder = null)
        where TChildComponent : IComponent =>
        Add(selector, GetRenderFragment(childParameterBuilder));

    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, RenderFragment>> selector,
        [StringSyntax("html")] string markup) =>
        Add(selector, markup.ToMarkupRenderFragment());

    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, RenderFragment<TValue>>> selector,
        Func<TValue, string> markupFactory) =>
        Add(selector, input => builder => builder.AddMarkupContent(0, markupFactory(input)));

    public ComponentParametersBuilder<TComponent> Add<TChildComponent, TValue>(
        Expression<Func<TComponent, RenderFragment<TValue>>> selector,
        Func<TValue, Action<ComponentParametersBuilder<TChildComponent>>> templateFactory)
        where TChildComponent : IComponent =>
        Add(selector, value => GetRenderFragment(templateFactory(value)));
    
}