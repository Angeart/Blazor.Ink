using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Ink;

/// <summary>
/// Custom Dispatcher for Ink (compatible with Blazor's official Dispatcher).
/// </summary>
public class InkDispatcher : Dispatcher, IDisposable
{
  private readonly BlockingCollection<Action> _queue = new();
  private int _dispatcherThreadId;
  private readonly CancellationTokenSource _cts = new();
  public InkDispatcher()
  {
    var dispatcherThread = new Thread(RunLoop) { IsBackground = false };
    dispatcherThread.Start(_cts.Token);
  }
  private void RunLoop(object? tokenObj)
  {
    var maybeToken = (CancellationToken?)tokenObj;
    if (maybeToken is null)
    {
      throw new ArgumentNullException(nameof(tokenObj), "CancellationToken must be provided.");
    }
    var token = maybeToken.Value;
    _dispatcherThreadId = Thread.CurrentThread.ManagedThreadId;
    SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
    foreach (var action in _queue.GetConsumingEnumerable())
    {
      action();
      if (_queue.IsCompleted || token.IsCancellationRequested)
      {
        break;
      }
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
    _queue.Add(async void () => { try { await workItem(); tcs.SetResult(null); } catch (Exception ex) { tcs.SetException(ex); } });
    return tcs.Task;
  }
  public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
  {
    var tcs = new TaskCompletionSource<TResult>();
    _queue.Add(async void () => { try { tcs.SetResult(await workItem()); } catch (Exception ex) { tcs.SetException(ex); } });
    return tcs.Task;
  }
  public void Stop() => _queue.CompleteAdding();

  public void Dispose()
  {
    _queue.Dispose();
    _cts.Dispose();
  }
}
