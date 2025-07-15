using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public sealed partial class ComponentParametersBuilder<TComponent>
    where TComponent : IComponent
{
    public ComponentParametersBuilder<TComponent> AddCascadingValue<TValue>(TValue cascadingValue) =>
        AddCascadingValueParameter(null, cascadingValue);

    public ComponentParametersBuilder<TComponent> AddCascadingValue<TValue>(string name, TValue cascadingValue) =>
        AddCascadingValueParameter(name, cascadingValue);
}