namespace Blazor.Ink.Core.Layout.Value;

public readonly record struct Size(int Width, int Height)
{
    public static Size Zero => new(0, 0);
    public static Size Max(in Size a, in Size b)
    {
        return new Size(Math.Max(a.Width, b.Width), Math.Max(a.Height, b.Height));
    }
}