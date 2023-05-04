using Prometheus;
using Yarp.Telemetry.Consumption;

namespace Sail.Metrics.Prometheus;

public class PrometheusOutboundHttpMetrics : IMetricsConsumer<HttpMetrics>
{
    private static readonly double CUBE_ROOT_10 = Math.Pow(10, (1.0 / 3));


    private static readonly Counter _outboundRequestsStarted = global::Prometheus.Metrics.CreateCounter(
        "sail_outbound_http_requests_started",
        "Number of outbound requests initiated by the proxy"
    );

    private static readonly Counter _outboundRequestsFailed = global::Prometheus.Metrics.CreateCounter(
        "sail_outbound_http_requests_failed",
        "Number of outbound requests failed"
    );

    private static readonly Gauge _outboundCurrentRequests = global::Prometheus.Metrics.CreateGauge(
        "sail_outbound_http_current_requests",
        "Number of active outbound requests that have started but not yet completed or failed"
    );

    private static readonly Gauge _outboundCurrentHttp11Connections = global::Prometheus.Metrics.CreateGauge(
        "sail_outbound_http11_connections",
        "Number of currently open HTTP 1.1 connections"
    );

    private static readonly Gauge _outboundCurrentHttp20Connections = global::Prometheus.Metrics.CreateGauge(
        "sail_outbound_http20_connections",
        "Number of active proxy requests that have started but not yet completed or failed"
    );

    private static readonly Histogram _outboundHttp11RequestQueueDuration = global::Prometheus.Metrics.CreateHistogram(
        "sail_outbound_http11_request_queue_duration",
        "Average time spent on queue for HTTP 1.1 requests that hit the MaxConnectionsPerServer limit in the last metrics interval",
        new HistogramConfiguration
        {
            Buckets = Histogram.ExponentialBuckets(10, CUBE_ROOT_10, 10)
        });

    private static readonly Histogram _outboundHttp20RequestQueueDuration = global::Prometheus.Metrics.CreateHistogram(
        "sail_outbound_http20_request_queue_duration",
        "Average time spent on queue for HTTP 2.0 requests that hit the MAX_CONCURRENT_STREAMS limit on the connection in the last metrics interval",
        new HistogramConfiguration
        {
            Buckets = Histogram.ExponentialBuckets(10, CUBE_ROOT_10, 10)
        });

    public void OnMetrics(HttpMetrics previous, HttpMetrics current)
    {
        _outboundRequestsStarted.IncTo(current.RequestsStarted);
        _outboundRequestsFailed.IncTo(current.RequestsFailed);
        _outboundCurrentRequests.Set(current.CurrentRequests);
        _outboundCurrentHttp11Connections.Set(current.CurrentHttp11Connections);
        _outboundCurrentHttp20Connections.Set(current.CurrentHttp20Connections);
        _outboundHttp11RequestQueueDuration.Observe(current.Http11RequestsQueueDuration.TotalMilliseconds);
        _outboundHttp20RequestQueueDuration.Observe(current.Http20RequestsQueueDuration.TotalMilliseconds);
    }
}