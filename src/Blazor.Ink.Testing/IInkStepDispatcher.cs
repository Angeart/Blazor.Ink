namespace Blazor.Ink.Testing;

public interface IInkStepDispatcher : IDisposable
{
    Task MoveNext(bool waitForEnqueueTask = true);
}