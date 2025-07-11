namespace Blazor.Ink.Test.Internal;

public interface IInkStepDispatcher : IDisposable
{
    Task MoveNext(bool waitForEnqueueTask = true);
}