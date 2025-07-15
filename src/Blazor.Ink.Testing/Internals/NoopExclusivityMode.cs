using Spectre.Console;

namespace Blazor.Ink.Testing.Internals;

internal sealed class NoopExclusivityMode : IExclusivityMode
{
    public T Run<T>(Func<T> func)
    {
        return func();
    }

    public async Task<T> RunAsync<T>(Func<Task<T>> func)
    {
        return await func().ConfigureAwait(false);
    }
}