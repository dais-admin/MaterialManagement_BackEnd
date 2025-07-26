using App.Metrics.Gauge;

namespace Dais.Metrics.Options
{
    public class AppMetricsOptions
    {
        public const string SectionName = "AppMetrics";
        
        public bool ReportingEnabled { get; set; } = true;
        public bool ResetOnReporting { get; set; } = true;
        public GaugeOptions Gauges { get; set; } = new();
        public Dictionary<string, string> GlobalTags { get; } = new();
    }
}
