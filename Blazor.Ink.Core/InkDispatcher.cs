using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Ink.Core;

/// <summary>
/// Ink用のカスタムDispatcher（Blazor公式Dispatcher互換）
/// </summary>
public class InkDispatcher : Dispatcher
{
  private readonly BlockingCollection<Action> _queue = new();
  private readonly Thread _dispatcherThread;
  private int _dispatcherThreadId;
  public InkDispatcher()
  {
    _dispatcherThread = new Thread(RunLoop) { IsBackground = false };
    _dispatcherThread.Start();
  }
  private void RunLoop()
  {
    _dispatcherThreadId = Thread.CurrentThread.ManagedThreadId;
    SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    foreach (var action in _queue.GetConsumingEnumerable())
    {
      action();
    }
  }
  public override bool CheckAccess() => Thread.CurrentThread.ManagedThreadId == _dispatcherThreadId;

  public override Task InvokeAsync(Action workItem)
  {
    var tcs = new TaskCompletionSource<object?>();
    _queue.Add(() => { try { workItem(); tcs.SetResult(null); } catch (Exception ex) { tcs.SetException(ex); } });
    return tcs.Task;
  }
  public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
  {
    var tcs = new TaskCompletionSource<TResult>();
    _queue.Add(() => { try { tcs.SetResult(workItem()); } catch (Exception ex) { tcs.SetException(ex); } });
    return tcs.Task;
  }
  public override Task InvokeAsync(Func<Task> workItem)
  {
    var tcs = new TaskCompletionSource<object?>();
    _queue.Add(async () => { try { await workItem(); tcs.SetResult(null); } catch (Exception ex) { tcs.SetException(ex); } });
    return tcs.Task;
  }
  public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
  {
    var tcs = new TaskCompletionSource<TResult>();
    _queue.Add(async () => { try { tcs.SetResult(await workItem()); } catch (Exception ex) { tcs.SetException(ex); } });
    return tcs.Task;
  }
  public void Stop() => _queue.CompleteAdding();
}
