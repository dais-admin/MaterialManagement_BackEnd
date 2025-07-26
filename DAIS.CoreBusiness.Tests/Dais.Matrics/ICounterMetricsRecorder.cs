using App.Metrics;

namespace Dais.Metrics
{
    public interface ICounterMetricsRecorder
    {
        void Publish(string metricName, Unit measurementUnit, long amount = 1, IReadOnlyDictionary<string, string>? tags = null, string? context = null);
    }
}
