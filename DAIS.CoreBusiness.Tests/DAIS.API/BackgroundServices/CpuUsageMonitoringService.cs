
using App.Metrics;
using Dais.Metrics;
using System.Diagnostics;

namespace DAIS.API.BackgroundServices
{
    public class CpuUsageMonitoringService : BackgroundService
    {
        private readonly IGaugeMetricsRecorder _metricsRecorder;

        public CpuUsageMonitoringService(IGaugeMetricsRecorder metricsRecorder)
        {
            _metricsRecorder = metricsRecorder;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _metricsRecorder.Publish("CpuUsage", measurementUnit: Unit.Percent, Process.GetCurrentProcess().WorkingSet64);
                }
                catch (OperationCanceledException)
                {
                }

                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            }

        }
    }
}
