using Spectre.Console.Testing;

namespace Blazor.Ink.Test.Internal;

public class TestContext(TestConsole console, IInkStepDispatcher dispatcher, Action disposeAction) : IDisposable
{
    public TestConsole Console { get; } = console;
    public IInkStepDispatcher Dispatcher { get; } = dispatcher;
    private Action DisposeAction { get; } = disposeAction;

    public void Dispose()
    {
        Console.Dispose();
        Dispatcher.Dispose();
        DisposeAction?.Invoke();
    }
}