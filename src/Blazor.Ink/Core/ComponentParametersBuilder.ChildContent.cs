using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using IComponent = Microsoft.AspNetCore.Components.IComponent;

namespace Blazor.Ink.Core;

public sealed partial class ComponentParametersBuilder<TComponent>
    where TComponent : IComponent
{
    private const string ChildContent = nameof(ChildContent);
    private static readonly PropertyInfo? ChildContentPropertyInfo =
        typeof(TComponent).GetProperty(ChildContent, BindingFlags.Public | BindingFlags.Instance);

    public ComponentParametersBuilder<TComponent> AddChildContent(RenderFragment childContent)
    {
        if (!HasChildContentProperty())
        {
            throw new InvalidOperationException($"The child content '{ChildContent}' is not present.");
        }

        if (HasGenericChildContentProperty())
        {
            throw new InvalidOperationException(
                $"The child content '{ChildContent}' is a generic type and cannot be set directly. " +
                "Use the `Add(p => p.ChildContent, {{content}})` method instead.");
        }
        
        return AddComponentParameter(ChildContent, childContent);
    }

    public ComponentParametersBuilder<TComponent> AddChildContent(
        [StringSyntax("html")] string markup) =>
        AddChildContent(markup.ToMarkupRenderFragment());

    public ComponentParametersBuilder<TComponent> AddChildContent<TChildContent>(
        Action<ComponentParametersBuilder<TChildContent>>? childContentBuilder = null)
        where TChildContent : IComponent =>
        AddChildContent(GetRenderFragment(childContentBuilder));

    private static bool HasChildContentProperty()
    {
        return ChildContentPropertyInfo is { } info &&
               info.GetCustomAttribute<ParameterAttribute>(false) is not null;
    }
    
    private static bool HasGenericChildContentProperty()
    {
        return ChildContentPropertyInfo is { PropertyType.IsGenericType: true };
    }
}