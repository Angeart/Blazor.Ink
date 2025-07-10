using Blazor.Ink.Core.Components;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Blazor.Ink.Core.Layouts;

public class BoxNode : NodeBase
{
    public BoxBorder Border { get; set; } = BoxBorder.None;
    public Color? BorderForegroundColor { get; set; }
    public Color? BorderBackgroundColor { get; set; }
    public Decoration? Decoration { get; set; }


    public override void ApplyComponent(IInkComponent component)
    {
        var box = component as Box;
        if (box is null)
        {
            return;
        }
        
        
    }

    protected override IRenderable BuildRenderTree(IRenderable child)
    {
        var panel = new Panel(child);
        panel.Border = Border;
        panel.BorderStyle = new Style(BorderForegroundColor, BorderBackgroundColor, Decoration);
        var size = GetSize();
        panel.Width = size.Width;
        panel.Height = size.Height;
        var padding = GetPadding();
        panel.Padding = new Spectre.Console.Padding(padding.Top, padding.Right, padding.Bottom, padding.Left);
        var margin = GetMargin();
        var marginPanel = new Padder(panel, new Spectre.Console.Padding(
            margin.Top, margin.Right, margin.Bottom, margin.Left));
        
        return marginPanel;
    }
}