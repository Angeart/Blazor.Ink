using Blazor.Ink.Core.Components;
using Blazor.Ink.Core.Layouts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Spectre.Console;
using Spectre.Console.Rendering;
using Text = Blazor.Ink.Core.Components.Text;

namespace Blazor.Ink.Core;

public partial class InkRenderer
{
    private record struct RenderContext(int Position, IInkNode? Node)
    {
        public static RenderContext Create(int position) => new RenderContext(position, null);
    }

    private RenderContext BuildSpectreRenderable(int componentId, ref RenderContext ctx)
    {
        Dispatcher.AssertAccess();
        var frames = base.GetCurrentRenderTreeFrames(componentId);
        return RenderFrames(componentId, frames, 0, frames.Count, ref ctx);
    }

    private RenderContext RenderFrames(int componentId, ArrayRange<RenderTreeFrame> frames, int position,
        int maxElements, ref RenderContext ctx)
    {
        List<IRenderable> renderables = new();
        var endPosition = position + maxElements;
        while (position < endPosition)
        {
            var (next, _) = RenderCore(componentId, frames, position, ref ctx);
            if (position == next)
            {
                throw new InvalidOperationException("We don't consume any input.");
            }

            position = next;
        }

        return ctx with { Position = position };
    }

    private RenderContext RenderCore(int componentId, ArrayRange<RenderTreeFrame> frames, int position,
        ref RenderContext ctx)
    {
        ref var frame = ref frames.Array[position];
        return frame.FrameType switch
        {
            RenderTreeFrameType.Text => new RenderContext(++position, ctx.Node?.ApplyText(frame.TextContent)),
            RenderTreeFrameType.Component => RenderComponent(ref frame, ref ctx),
            _ => new RenderContext(++position, null)
        };
    }

    private RenderContext RenderComponent(ref RenderTreeFrame frame, ref RenderContext ctx)
    {
        IInkNode? node = frame.Component switch
        {
            Box => new BoxNode(),
            Text => new TextNode(),
            _ => null
        };

        node?.ApplyComponent(frame.Component);
        ctx.Node?.AppendChild(node);

        var nextCtx = new RenderContext(++ctx.Position, Node: node);
        return RenderChildComponent(ref frame, ref nextCtx);
    }

    private RenderContext RenderChildComponent(ref RenderTreeFrame componentFrame, ref RenderContext ctx)
    {
        return BuildSpectreRenderable(componentFrame.ComponentId, ref ctx);
    }
}