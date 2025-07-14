using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

internal class RootComponent : IComponent
{
    private readonly RenderFragment _renderFragment;
    private RenderHandle _renderHandle;
    
    public RootComponent(RenderFragment renderFragment)
    {
        _renderFragment = renderFragment;
    }
    
    public void Attach(RenderHandle renderHandle)
    {
        _renderHandle = renderHandle;
    }
    
    public Task SetParametersAsync(ParameterView parameters)
    {
        _renderHandle.Render(_renderFragment);
        return Task.CompletedTask;
    }
}