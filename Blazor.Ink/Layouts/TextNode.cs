using Spectre.Console;
using YogaSharp;
using Components_Text = Blazor.Ink.Components.Text;

namespace Blazor.Ink.Layouts;

public class TextNode : NodeBase<Components_Text>
{
    public TextNode(IAnsiConsole ansiConsole) : base(ansiConsole)
    {
    }

    private string Text { get; set; } = string.Empty;
    private int Width { get; set; }
    private int Height { get; set; }

    protected override void ApplyLayoutImpl()
    {
        unsafe
        {
            Node->SetNodeType(YGNodeType.Text);
            Node->SetWidth(Width);
            Node->SetHeight(Height);
        }
    }

    public override RenderTree BuildRenderTree()
    {
        return new RenderTree(() => Render(new Markup(Text), true));
    }

    public override IInkNode ApplyText(string text)
    {
        var markup = new Markup(text);
        Text = text;
        Width = markup.Length;
        Height = markup.Lines;
        return this;
    }
}