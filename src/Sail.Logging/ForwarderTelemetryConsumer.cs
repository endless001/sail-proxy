using Yarp.ReverseProxy.Forwarder;
using Yarp.Telemetry.Consumption;

namespace Sail.Logging;

public sealed class ForwarderTelemetryConsumer : IForwarderTelemetryConsumer
{
    public void OnForwarderStart(DateTime timestamp, string destinationPrefix)
    {
        var metrics = RequestMetrics.Current;
        metrics.ProxyStartOffset = metrics.CalcOffset(timestamp);
    }

    public void OnForwarderStop(DateTime timestamp, int statusCode)
    {
        var metrics = RequestMetrics.Current;
        metrics.ProxyStopOffset = metrics.CalcOffset(timestamp);
        metrics.StatusCode = statusCode;
    }

    public void OnForwarderFailed(DateTime timestamp, ForwarderError error)
    {
        var metrics = RequestMetrics.Current;
        metrics.Error = error;
    }

    public void OnContentTransferred(DateTime timestamp, bool isRequest, long contentLength, long iops,
        TimeSpan readTime, TimeSpan writeTime, TimeSpan firstReadTime)
    {
        var metrics = RequestMetrics.Current;

        if (isRequest)
        {
            metrics.RequestBodyLength = contentLength;
            metrics.RequestContentIops = iops;
            return;
        }

        metrics.HttpResponseContentStopOffset = metrics.CalcOffset(timestamp);
        metrics.ResponseBodyLength = contentLength;
        metrics.ResponseContentIops = iops;

    }

    public void OnForwarderInvoke(DateTime timestamp, string clusterId, string routeId, string destinationId)
    {
        var metrics = RequestMetrics.Current;
        metrics.RouteInvokeOffset = metrics.CalcOffset(timestamp);
        metrics.RouteId = routeId;
        metrics.ClusterId = clusterId;
        metrics.DestinationId = destinationId;
    }
}