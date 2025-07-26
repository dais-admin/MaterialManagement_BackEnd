namespace Dias.ExcelSteam.Queue
{
    public interface IBackgroundTaskQueue
    {
        Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
