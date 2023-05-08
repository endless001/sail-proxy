using System.Text.Json;
using Yarp.ReverseProxy.Forwarder;

namespace Sail.Logging;

public class RequestMetrics
{
    private static readonly AsyncLocal<RequestMetrics> _local = new();
    private static readonly JsonSerializerOptions _jsonOptions = new() { WriteIndented = true };

    public static RequestMetrics Current => _local.Value ??= new RequestMetrics();
    public DateTime StartTime { get; set; }
    public float RouteInvokeOffset { get; set; }
    public float ProxyStartOffset { get; set; }
    public float HttpRequestStartOffset { get; set; }
    public float HttpConnectionEstablishedOffset { get; set; }
    public float HttpRequestLeftQueueOffset { get; set; }
    public float HttpRequestHeadersStartOffset { get; set; }
    public float HttpRequestHeadersStopOffset { get; set; }
    public float HttpRequestContentStartOffset { get; set; }
    public float HttpRequestContentStopOffset { get; set; }
    public float HttpResponseHeadersStartOffset { get; set; }
    public float HttpResponseHeadersStopOffset { get; set; }
    public float HttpResponseContentStopOffset { get; set; }
    public float HttpRequestStopOffset { get; set; }
    public float ProxyStopOffset { get; set; }
    public ForwarderError Error { get; set; }
    public long RequestBodyLength { get; set; }
    public long ResponseBodyLength { get; set; }
    public long RequestContentIops { get; set; }
    public long ResponseContentIops { get; set; }
    public string DestinationId { get; set; }
    public string ClusterId { get; set; }
    public string RouteId { get; set; }
    public int StatusCode { get; set; }

    public float CalcOffset(DateTime timestamp)
    {
        return (float)(timestamp - StartTime).TotalMilliseconds;
    }
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, _jsonOptions);
    }
}