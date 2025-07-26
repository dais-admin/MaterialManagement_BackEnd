using App.Metrics;
using App.Metrics.Gauge;
using Dais.Metrics.Options;
using Microsoft.Extensions.Options;

namespace Dais.Metrics.Gauges
{
    public class GaugeMetricsRecorder : IGaugeMetricsRecorder
    {
        private readonly IMetrics _metrics;
        private readonly IOptions<AppMetricsOptions> _options;

        public GaugeMetricsRecorder(IMetrics metrics, IOptions<AppMetricsOptions> options)
        {
            _metrics = metrics;
            _options = options;
        }
        public void Publish(string metricsName, Unit measurementUnit, double amount = 1, IReadOnlyDictionary<string, string>? tagDictionary = null, string? context = null)
        {
            var gaugeOptions = new GaugeOptions
            {
                Context = context,
                Name = metricsName,
                MeasurementUnit = measurementUnit,
                ResetOnReporting = _options.Value.ResetOnReporting
            };

            if (tagDictionary is null)
            {
                _metrics.Measure.Gauge.SetValue(gaugeOptions, amount);
                return;
            }
            var tags = new MetricTags(tagDictionary!.Keys.ToArray(), tagDictionary.Values.ToArray());
            _metrics.Measure.Gauge.SetValue(gaugeOptions, tags, amount);

        }
    }
}
