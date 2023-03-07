namespace ToolboxWebApi.Services
{
    public interface IBackgroundTaskQueue
    {
        ValueTask<Func<CancellationToken, ValueTask>> DequeueAsync(CancellationToken cancellationToken);
        ValueTask QueueBackgroundWorkItemAsync(Func<CancellationToken, ValueTask> workItem);
    }
}