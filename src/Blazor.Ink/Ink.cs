namespace Blazor.Ink;

public static class Ink
{
    public static InkApplicationBuilder CreateBuilder(string[] args)
    {
        return new InkApplicationBuilder(args);
    }
}