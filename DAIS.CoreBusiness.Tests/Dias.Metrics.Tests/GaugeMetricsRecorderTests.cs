using App.Metrics;
using App.Metrics.Gauge;
using Dais.Metrics.Gauges;
using Dais.Metrics.Options;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Diagnostics;
using Xunit;

namespace Dias.Metrics.Tests
{
    public class GaugeMetricsRecorderTests
    {
        public readonly IMetrics _metrics;
        private readonly GaugeMetricsRecorder _metricsRecorder;
        public static string MetricsName = "TestGaugeMetric";
        public GaugeMetricsRecorderTests()
        {

            _metrics = Substitute.For<IMetrics>();
            var appMetricsOptions = new AppMetricsOptions();

            var options = Substitute.For<IOptions<AppMetricsOptions>>();
            options.Value.Returns(appMetricsOptions);

            _metricsRecorder = new GaugeMetricsRecorder(_metrics, options);
        }

        [Theory]
        [ClassData(typeof(MetricsUnitName))]
        public void Publish_ShouldSetValueWithoutTags_WhenTagsAreNotProvided(string unitName)
        {
            Unit unit = Utils.GetUnit(unitName);
            double amount = Process.GetCurrentProcess().WorkingSet64;

            _metricsRecorder.Publish(MetricsName, unit, amount);

            _metrics.Measure.Gauge.Received(1)
                .SetValue(Arg.Is<GaugeOptions>(opt => opt.Name == MetricsName && opt.MeasurementUnit == unit), 
                amount);
        }

    }
}
