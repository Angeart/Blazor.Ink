using System.Collections.Concurrent;
using System.Threading.Channels;
using Blazor.Ink.Testing.Helper;
using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Testing.Internals;

internal class InkTestingDispatcher : Dispatcher, IInkStepDispatcher
{
    private readonly BlockingCollection<Func<Task>> _queue = new();
    private readonly Channel<int> _stepChannel = Channel.CreateUnbounded<int>();
    private readonly Channel<int> _finishCurrentStepChannel = Channel.CreateUnbounded<int>();
    private readonly CancellationTokenSource _cts = new();
    private readonly SingleThreadSynchronizationContext _context = new();

    public InkTestingDispatcher()
    {
        var dispatcherThread = new Thread(RunLoop)
        {
            IsBackground = false,
            Name = "InkTestingDispatcher"
        };
        dispatcherThread.Start(_cts.Token);
    }

    public async Task MoveNext(bool waitForEnqueueTask = true)
    {
        if (waitForEnqueueTask)
        {
            await TaskHelper.WaitUntil(() => _queue.Count > 0, _cts.Token);
        }
        await _stepChannel.Writer.WriteAsync(0);
        await _finishCurrentStepChannel.Reader.ReadAsync();
    }

    private async void RunLoop(object? tokenObj)
    {
        var maybeToken = (CancellationToken?)tokenObj;
        if (maybeToken is null)
        {
            throw new ArgumentNullException(nameof(tokenObj), "CancellationToken must be provided.");
        }
        var token = maybeToken.Value;
        while (true)
        {
            await _stepChannel.Reader.ReadAsync(token);
            SynchronizationContext.SetSynchronizationContext(_context);
            var actions = new List<Func<Task>>();
            while (_queue.TryTake(out var action))
            {
                actions.Add(action);
            }

            foreach (var a in actions)
            {
                await a();
            }
            
            await _finishCurrentStepChannel.Writer.WriteAsync(0, token);
            
            token.ThrowIfCancellationRequested();
            if (_queue.IsCompleted)
            {
                break;
            }
        }
    }

    public override bool CheckAccess() => SynchronizationContext.Current == _context;

    public override Task InvokeAsync(Action workItem)
    {
        var tcs = new TaskCompletionSource<object?>();
        _queue.Add(() =>
        {
            try
            {
                workItem();
                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
            
            return tcs.Task;
        });
        return tcs.Task;
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
    {
        var tcs = new TaskCompletionSource<TResult>();
        _queue.Add(() =>
        {
            try
            {
                tcs.SetResult(workItem());
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }

            return tcs.Task;
        });
        return tcs.Task;
    }

    public override Task InvokeAsync(Func<Task> workItem)
    {
        var tcs = new TaskCompletionSource<object?>();
        _queue.Add(async Task () =>
        {
            try
            {
                await workItem();
                tcs.SetResult(null);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }

    public override Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
    {
        var tcs = new TaskCompletionSource<TResult>();
        _queue.Add(async Task () =>
        {
            try
            {
                tcs.SetResult(await workItem());
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });
        return tcs.Task;
    }

    public void Stop() => _queue.CompleteAdding();

    public void Dispose()
    {
        _queue.CompleteAdding();
        _queue.Dispose();
        _cts.Dispose();
    }
}