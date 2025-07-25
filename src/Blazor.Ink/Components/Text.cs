using Blazor.Ink.Core.Components.Abstraction;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Spectre.Console;
using IRenderable = Spectre.Console.Rendering.IRenderable;

namespace Blazor.Ink.Components;

public class Text : InkComponentBase, IInkComponent
{
    public override bool Inline => true;
    private Markup? _markup;
    [Parameter] public RenderFragment ChildContent { get; set; } = null!;

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, ChildContent);
    }
    
    protected override void OnApplyLayout()
    {
        // No specific layout to apply for text, as it is inline.
        // This method can be overridden if needed for custom behavior.
        LayoutNode.Width = _markup?.Length ?? 0;
        LayoutNode.Height = _markup?.Lines ?? 1;
    }

    protected override IRenderable OnBuildElement()
    {
        return _markup ?? new Markup(string.Empty);
    }
    
    public void ApplyText(string text)
    {
        _markup = new Markup(text);
    }
    
    public static Text Create(string text)
    {
        var textComponent = new Text();
        textComponent.ApplyText(text);
        return textComponent;
    }
}