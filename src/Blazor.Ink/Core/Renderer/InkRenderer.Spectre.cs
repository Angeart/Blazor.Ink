using System.Runtime.CompilerServices;
using Blazor.Ink.Core.Renderer.Abstraction;
using Microsoft.AspNetCore.Components.RenderTree;
using Spectre.Console;

namespace Blazor.Ink.Core.Renderer;

public partial class InkRenderer
{
    private RenderContext BuildSpectreRenderable(int componentId, ref RenderContext ctx)
    {
        Dispatcher.AssertAccess();
        var frames = GetCurrentRenderTreeFrames(componentId);
        return RenderFrames(frames, 0, frames.Count, ref ctx);
    }

    private RenderContext RenderFrames(ArrayRange<RenderTreeFrame> frames, int position,
        int maxElements, ref RenderContext ctx)
    {
        var endPosition = position + maxElements;
        while (position < endPosition)
        {
            var (next, _) = RenderCore(frames, position, ref ctx);
            if (position == next) throw new InvalidOperationException("We don't consume any input.");

            position = next;
        }

        return ctx with { Position = position };
    }

    private RenderContext RenderCore(ArrayRange<RenderTreeFrame> frames, int position,
        ref RenderContext ctx)
    {
        ref var frame = ref frames.Array[position];
        return frame.FrameType switch
        {
            RenderTreeFrameType.Text => new RenderContext(++position, ctx.RenderTree.AddText(frame.TextContent)),
            // NOTE: Ink does not support markup, directly add as a text content instead.
            RenderTreeFrameType.Markup => new RenderContext(++position, ctx.RenderTree.AddText(frame.MarkupContent)),
            RenderTreeFrameType.Component => RenderComponent(ref frame, ref ctx),
            RenderTreeFrameType.Element => UnsupportedFrameType(ref frame, ref ctx),
            _ => NoopFrameType(ref ctx)
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private RenderContext NoopFrameType(ref RenderContext ctx)
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
        if (frame.Component is not IRenderableComponent renderable)
        {
            return RenderChildComponent(ref frame, ref ctx);
        }
        
        var newCtx = new RenderContext(0, new RenderTree(renderable));
        RenderChildComponent(ref frame, ref newCtx);
        ctx.RenderTree.AddChild(newCtx.RenderTree);
        return ctx with { Position = ++ctx.Position };
    }

    private RenderContext RenderChildComponent(ref RenderTreeFrame componentFrame, ref RenderContext ctx)
    {
        return BuildSpectreRenderable(componentFrame.ComponentId, ref ctx);
    }

    private struct RenderContext(int position,in RenderTree renderTree) : IDisposable
    {
        public int Position = position;
        public RenderTree RenderTree = renderTree;
        public static RenderContext Empty => new(0, new RenderTree(null));
        
        public void Deconstruct(out int position, out RenderTree renderTree)
        {
            position = Position;
            renderTree = RenderTree;
        }

        public void Dispose()
        {
            RenderTree.Dispose();
        }
    }
}