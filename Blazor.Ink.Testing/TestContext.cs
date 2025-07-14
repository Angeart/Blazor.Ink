namespace Blazor.Ink.Testing;

public class TestContext(CursorEmulatedTestConsole console, IInkStepDispatcher dispatcher, Action disposeAction) : IDisposable
{
    public CursorEmulatedTestConsole Console { get; } = console;
    public IInkStepDispatcher Dispatcher { get; } = dispatcher;
    private Action DisposeAction { get; } = disposeAction;

    public void Dispose()
    {
        Console.Dispose();
        Dispatcher.Dispose();
        DisposeAction?.Invoke();
    }
}