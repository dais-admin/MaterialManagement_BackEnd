using App.Metrics;

namespace Dais.Metrics
{
    public interface IGaugeMetricsRecorder
    {
        void Publish(string metricsName, Unit measurementUnit, double amount = 1, IReadOnlyDictionary<string, string>? tagDictionary = null, string? context = null);
    }
}
