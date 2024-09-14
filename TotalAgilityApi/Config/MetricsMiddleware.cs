using Prometheus;
using System.Diagnostics;

namespace TotalAgilityApi.Config
{
    public class MetricsMiddleware
    {
        //private static readonly Counter EventCounter = Metrics.CreateCounter("http_events_total", "Total number of HTTP events", new[] { "event_type", "status_code" });
        //private static readonly Histogram EventDuration = Metrics.CreateHistogram("http_event_duration_seconds", "Histogram of HTTP event durations", new[] { "event_type", "status_code" });

        private static readonly Counter EventCounter = Metrics.CreateCounter("http_events_total", "Total number of HTTP events", ["event_type", "status_code"]);
        private static readonly Histogram EventDuration = Metrics.CreateHistogram("http_event_duration_seconds", "Histogram of HTTP event durations", ["event_type", "status_code"]);

        private readonly RequestDelegate _next;

        public MetricsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var eventType = context.Request.Path.Value.TrimStart('/').Split('/')[0];
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                EventCounter.Labels(eventType, context.Response.StatusCode.ToString()).Inc();
                EventDuration.Labels(eventType, context.Response.StatusCode.ToString()).Observe(stopwatch.Elapsed.TotalSeconds);
            }
        }

    }
}
