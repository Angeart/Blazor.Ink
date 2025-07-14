using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public sealed partial class ComponentParametersBuilder<TComponent>
    where TComponent : IComponent
{
    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, EventCallback>> selector,
        Action callback) =>
        Add(selector, EventCallback.Factory.Create(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, EventCallback?>> selector,
        Action? callback) =>
        Add(selector, EventCallback.Factory.Create(callback!.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, EventCallback>> selector,
        Action<object> callback) =>
        Add(selector, EventCallback.Factory.Create(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, EventCallback?>> selector,
        Action<object>? callback) =>
        Add(selector, EventCallback.Factory.Create(callback!.Target!, callback));

    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, EventCallback>> selector,
        Func<Task> callback) =>
        Add(selector, EventCallback.Factory.Create(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add(
        Expression<Func<TComponent, EventCallback?>> selector,
        Func<Task>? callback) =>
        Add(selector, EventCallback.Factory.Create(callback!.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>>> selector,
        Action callback) =>
        Add(selector, EventCallback.Factory.Create<TValue>(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>?>> selector,
        Action? callback) =>
        Add(selector, EventCallback.Factory.Create<TValue>(callback!.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>>> selector,
        Action<TValue> callback) =>
        Add(selector, EventCallback.Factory.Create(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>?>> selector,
        Action<TValue>? callback) =>
        Add(selector, EventCallback.Factory.Create(callback!.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>>> selector,
        Func<Task> callback) =>
        Add(selector, EventCallback.Factory.Create<TValue>(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>?>> selector,
        Func<Task>? callback) =>
        Add(selector, EventCallback.Factory.Create<TValue>(callback!.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>>> selector,
        Func<TValue, Task> callback) =>
        Add(selector, EventCallback.Factory.Create(callback.Target!, callback));
    
    public ComponentParametersBuilder<TComponent> Add<TValue>(
        Expression<Func<TComponent, EventCallback<TValue>?>> selector,
        Func<TValue, Task>? callback) =>
        Add(selector, EventCallback.Factory.Create(callback!.Target!, callback));

}