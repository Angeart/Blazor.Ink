using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Ink.Components;

public class Text : ComponentBase, IInkComponent
{
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent);
    }
}