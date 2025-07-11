using System.Runtime.CompilerServices;
using Blazor.Ink.Components;
using Blazor.Ink.Layouts;
using Microsoft.AspNetCore.Components.RenderTree;
using Spectre.Console;
using Spectre.Console.Rendering;
using Text = Blazor.Ink.Components.Text;

namespace Blazor.Ink;

public partial class InkRenderer
{
    private RenderContext BuildSpectreRenderable(int componentId, ref RenderContext ctx)
    {
        Dispatcher.AssertAccess();
        var frames = GetCurrentRenderTreeFrames(componentId);
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
            if (position == next) throw new InvalidOperationException("We don't consume any input.");

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
            RenderTreeFrameType.Element => UnsupportedFrameType(ref frame, ref ctx),
            RenderTreeFrameType.Markup => UnsupportedFrameType(ref frame, ref ctx),
            _ => NoopFrameType(ref frame, ref ctx)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RenderContext NoopFrameType(ref RenderTreeFrame frame, ref RenderContext ctx)
    {
        return ctx with { Position = ++ctx.Position };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RenderContext UnsupportedFrameType(ref RenderTreeFrame frame, ref RenderContext ctx)
    {
        _ansiConsole.WriteException(new InvalidOperationException(
            $"Unsupported frame type: {frame.FrameType} at position {ctx.Position} in component {frame.ComponentId}"));
        return ctx with { Position = ++ctx.Position };
    }

    private RenderContext RenderComponent(ref RenderTreeFrame frame, ref RenderContext ctx)
    {
        IInkNode? node = frame.Component switch
        {
            Box => new BoxNode(_ansiConsole),
            Text => new TextNode(_ansiConsole),
            _ => null
        };

        node?.ApplyComponent(frame.Component);
        ctx.Node?.AppendChild(node);

        var newCtx = new RenderContext(0, node);
        RenderChildComponent(ref frame, ref newCtx);
        return ctx with { Position = ++ctx.Position };
    }

    private RenderContext RenderChildComponent(ref RenderTreeFrame componentFrame, ref RenderContext ctx)
    {
        return BuildSpectreRenderable(componentFrame.ComponentId, ref ctx);
    }

    private record struct RenderContext(int Position, IInkNode? Node)
    {
        public static RenderContext Create(int position)
        {
            return new RenderContext(position, null);
        }
    }
}