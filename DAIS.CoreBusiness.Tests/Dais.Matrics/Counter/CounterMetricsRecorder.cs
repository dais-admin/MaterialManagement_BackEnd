using App.Metrics;
using App.Metrics.Counter;
using Dais.Metrics.Extensions;
using Dais.Metrics.Options;
using Microsoft.Extensions.Options;

namespace Dais.Metrics.Counter
{
    internal class CounterMetricsRecorder : ICounterMetricsRecorder
    {
        private readonly IMetrics _metrics;
        private readonly IOptions<AppMetricsOptions> _options;

        public CounterMetricsRecorder(IMetrics metrics, IOptions<AppMetricsOptions> options)
        {
            _metrics = metrics;
            _options = options;
        }


        public void Publish(string metricName, Unit measurementUnit, long amount = 1, IReadOnlyDictionary<string, string>? tags = null, string? context = null)
        {
            var counterOptions = new CounterOptions
            {
                Name = metricName,
                MeasurementUnit = measurementUnit,
                ResetOnReporting = _options.Value.ResetOnReporting
            };

            counterOptions.AddContext(context);
            if (amount == 0)
            {
                return;
            }

            if (tags == null)
            {
                Action<CounterOptions, long> counterOperation = amount > 0 ?
                    _metrics.Measure.Counter.Increment : _metrics.Measure.Counter.Decrement;
            }

            MetricTags metricTags = new(keys: tags.Keys.ToArray(), values: tags.Values.ToArray());
            Action<CounterOptions, MetricTags> counterOperationWithTags = amount > 0 ?
                _metrics.Measure.Counter.Increment : _metrics.Measure.Counter.Decrement;
        }
    }
}
