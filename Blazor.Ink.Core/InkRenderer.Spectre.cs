using Blazor.Ink.Core.Components;
using Blazor.Ink.Core.Layouts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace Blazor.Ink.Core;

public partial class InkRenderer
{
    private record struct RenderContext(int Position, ILayoutNode? Node) {
        public static RenderContext Create(int position) => new RenderContext(position, null);
    }
    private RenderContext BuildSpectreRenderable(int componentId)
    {
        Dispatcher.AssertAccess();
        var frames = base.GetCurrentRenderTreeFrames(componentId);
        return RenderFrames(componentId, frames, 0, frames.Count);
    }

    private RenderContext RenderFrames(int componentId, ArrayRange<RenderTreeFrame> frames, int position, int maxElements)
    {
        List<IRenderable> renderables = new();
        var endPosition = position + maxElements;
        while (position < endPosition)
        {
            var (_next, renderable) = RenderCore(componentId, frames, position);
            if (position == _next)
            {
                throw new InvalidOperationException("We don't consume any input.");
            }
            if (renderable is not null)
            {
                renderables.Add(renderable);
            }
            position = _next;
        }

        return new RenderContext(position, renderables.Count > 0 ? new Rows(renderables) : null);
    }

    private RenderContext RenderCore(int componentId, ArrayRange<RenderTreeFrame> frames, int position)
    {
        ref var frame = ref frames.Array[position];
        return frame.FrameType switch
        {
            RenderTreeFrameType.Text => new RenderContext(++position, new Markup(frame.TextContent)),
            RenderTreeFrameType.Component => ApplySpectreComponent(RenderChildComponent(ref frame), frame.Component),
            _ => new RenderContext(position, null)
        };
    }

    private void ComsumeComponentFrames(ArrayRange<RenderTreeFrame> frames, ref int position)
    {
        var componentFrame = frames.Array[position];
        if (componentFrame.FrameType != RenderTreeFrameType.Component)
        {
            throw new InvalidOperationException("Expected a component frame.");
        }
        
        var nextFrame = frames.Array[position + 1];
        while (nextFrame.FrameType is RenderTreeFrameType.Attribute)
        {
            // Gather attributes until we reach the next frame type
            var attributeFrame = frames.Array[position + 1];
            attributeFrame.
        }
    }

    private RenderContext ApplySpectreComponent(RenderContext ctx, IComponent component)
    {
        var renderable = component switch
        {
            Box box => new Panel(ctx.Renderable ?? new Markup(""))
            {
                Padding = new Padding(box.Padding, box.Padding),
                // Spectre.ConsoleのPanelはMargin未対応なので省略
            },
            _ => ctx.Renderable
        };
        return new RenderContext(ctx.Position, renderable);
    }

    private RenderContext RenderChildComponent(ref RenderTreeFrame componentFrame)
    {
        return BuildSpectreRenderable(componentFrame.ComponentId);
    }
}