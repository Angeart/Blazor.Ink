using System.Collections;
using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public class ParameterCollection : ICollection<Parameter>, IReadOnlyCollection<Parameter>
{
    private List<Parameter>? _parameters;

#if NET9_0_OR_GREATER
    public IComponentRenderMode? RenderMode { get; set; }
#endif

    public int Count => _parameters?.Count ?? 0;
    public bool IsReadOnly => false;

    public void Add(Parameter item)
    {
        _parameters ??= new List<Parameter>();
        _parameters.Add(item);
    }

    public bool Contains(Parameter item)
    {
        return _parameters?.Contains(item) ?? false;
    }

    public void Clear()
    {
        _parameters?.Clear();
    }

    public void CopyTo(Parameter[] array, int arrayIndex)
    {
        _parameters?.CopyTo(array, arrayIndex);
    }

    public bool Remove(Parameter item)
    {
        return _parameters?.Remove(item) ?? false;
    }

    public IEnumerator<Parameter> GetEnumerator()
    {
        if (_parameters is null)
            yield break;
        foreach (var parameter in _parameters) yield return parameter;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void AddRange(IEnumerable<Parameter> items)
    {
        _parameters ??= new List<Parameter>();
        _parameters.AddRange(items);
    }

    public RenderFragment AsRenderFragment<TComponent>()
        where TComponent : IComponent
    {
        return RenderFragmentFactory.CreateRenderFragment<TComponent>(this);
    }

    public RenderFragment AsRenderFragment(Type componentType)
    {
        return RenderFragmentFactory.CreateRenderFragment(this, componentType);
    }
}