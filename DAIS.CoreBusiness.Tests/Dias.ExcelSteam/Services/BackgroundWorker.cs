using Dias.ExcelSteam.Queue;
using Microsoft.Extensions.Hosting;

namespace Dias.ExcelSteam.Services
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _queue;

        public BackgroundWorker(IBackgroundTaskQueue queue)
        {
            _queue = queue;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _queue.ExecuteAsync(stoppingToken);
            }            
        }
    }
}
