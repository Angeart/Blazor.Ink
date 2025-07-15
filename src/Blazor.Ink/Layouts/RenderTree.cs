namespace Blazor.Ink.Layouts;

public class RenderTree
{
    private readonly List<RenderTree> _children;
    private readonly Func<Size> _renderAction;

    public RenderTree(Func<Size> renderAction, List<RenderTree>? children = null)
    {
        _renderAction = renderAction;
        _children = children ?? new List<RenderTree>();
    }

    public Size Render()
    {
        var size = _renderAction.Invoke();
        foreach (var child in _children)
        {
            var contentSize = child.Render();
            size = Size.Max(size, contentSize);
        }

        return size;
    }
}