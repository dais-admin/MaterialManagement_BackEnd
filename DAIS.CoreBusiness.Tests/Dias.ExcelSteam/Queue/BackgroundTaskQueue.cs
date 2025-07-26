using DAIS.DataAccess.Data;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Dias.ExcelSteam.Queue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();
        private readonly SemaphoreSlim _signal = new(0);

        public Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem)
        {
            ArgumentNullException.ThrowIfNull(workItem, nameof(workItem));
            _workItems.Enqueue(workItem);
            _signal.Release();

            return Task.CompletedTask;
        }


        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _signal.WaitAsync(stoppingToken);
                if (_workItems.TryDequeue(out var workItem))
                {
                    try
                    {

                        await workItem(stoppingToken);

                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }

            }
        }
    }
}
