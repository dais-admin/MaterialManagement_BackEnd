using App.Metrics;
using App.Metrics.Formatters.Ascii;
using App.Metrics.Formatters.Json;
using Dais.Metrics.Counter;
using Dais.Metrics.Gauges;
using Dais.Metrics.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;

namespace Dais.Metrics.Extensions
{
    public static class MetricesServiceCollectionExtensions
    {
        public static IHostBuilder UseDiasMetrics(this IHostBuilder hostBuilder,
            Action<MetricsOptions>? configureMetricsOptions)
        {
            hostBuilder.ConfigureServices((context, services) =>
            {
                services.AddMetricsConfigured(context.Configuration, configureMetricsOptions);
            });
            return hostBuilder;
        }

        private static void AddMetricsConfigured(this IServiceCollection services,
            IConfiguration configuration,
            Action<MetricsOptions>? configureMetricsOptions)
        {
            ArgumentNullException.ThrowIfNull(services);

            ArgumentNullException.ThrowIfNull(configuration);

            var appMetricsSection = configuration.GetSection(AppMetricsOptions.SectionName);

            services.Configure<AppMetricsOptions>(appMetricsSection)
                    .AddMetrics(builder =>
            {
                AppMetricsOptions metricsOptions = appMetricsSection.Get<AppMetricsOptions>();
                builder.Configuration.Configure(options =>
                {
                    options.AddAppTag();
                    options.AddEnvTag();
                    options.AddServerTag();
                    options.Enabled = true;
                    options.ReportingEnabled = metricsOptions!.ReportingEnabled;
                    configureMetricsOptions!.Invoke(options);
                });
            }).AddMetricsEndpoints(options =>
            {
                options.MetricsEndpointEnabled = true;
                options.MetricsTextEndpointEnabled = true;
                options.MetricsEndpointOutputFormatter = new MetricsJsonOutputFormatter();
                options.MetricsTextEndpointOutputFormatter = new MetricsTextOutputFormatter();
                options.EnvironmentInfoEndpointEnabled = true;
            })
            .AddSingleton<IGaugeMetricsRecorder, GaugeMetricsRecorder>()
            .AddSingleton<ICounterMetricsRecorder, CounterMetricsRecorder>();
        }



        public static void AddContext(this MetricValueOptionsBase? options, string? context)
        {
            if (options is null || context is null)
            {
                return;
            }
            options.Context = context;
        }
    }
}
