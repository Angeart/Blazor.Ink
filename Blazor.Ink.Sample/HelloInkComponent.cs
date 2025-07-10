using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Ink.Sample;

public class HelloInkComponent : ComponentBase
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenElement(0, "box");
        builder.AddContent(1, "Blazor.Ink TUI from Blazor Component!");
        builder.CloseElement();
    }
}
