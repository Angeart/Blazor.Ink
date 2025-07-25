namespace Blazor.Ink.Core.Layout.Value;

public record struct AspectRatio(float Value)
{
    public static implicit operator AspectRatio(float value) => new(value);
    public static implicit operator float(AspectRatio aspectRatio) => aspectRatio.Value;
    public static implicit operator AspectRatio((float width, float height) ratio)
    {
        if (ratio.height == 0) return new AspectRatio(0);
        return new AspectRatio(ratio.width / ratio.height);
    }
    
    public static AspectRatio From(float width, float height)
    {
        return (width, height);
    }
}