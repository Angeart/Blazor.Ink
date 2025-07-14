using System.Collections.Concurrent;

namespace Blazor.Ink.Testing.Internals;

internal class SingleThreadSynchronizationContext : SynchronizationContext
{
    private readonly BlockingCollection<(SendOrPostCallback Callback, object? State)> _queue = new();
    private readonly Thread _workerThread;

    public SingleThreadSynchronizationContext()
    {
        _workerThread = new Thread(WorkLoop)
        {
            IsBackground = true
        };
        _workerThread.Start();
    }
    
    private SingleThreadSynchronizationContext(BlockingCollection<(SendOrPostCallback Callback, object? State)> queue, Thread workerThread)
    {
        _queue = queue;
        _workerThread = workerThread;
    }

    public override void Post(SendOrPostCallback d, object? state)
    {
        _queue.Add((d, state));
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        if (Thread.CurrentThread == _workerThread)
        {
            d(state);
        }
        else
        {
            var waitHandle = new ManualResetEvent(false);
            _queue.Add((s =>
            {
                d(s);
                waitHandle.Set();
            }, state));
            waitHandle.WaitOne();
        }
    }

    private void WorkLoop()
    {
        foreach (var (callback, state) in _queue.GetConsumingEnumerable())
        {
            try
            {
                callback(state);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception: {ex}");
            }
        }
    }

    public override SynchronizationContext CreateCopy()
    {
        return new SingleThreadSynchronizationContext(_queue, _workerThread);
    }

    public void Dispose()
    {
        _queue.CompleteAdding();
        _workerThread.Join();
    }
}