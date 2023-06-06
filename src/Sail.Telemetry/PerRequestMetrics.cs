using System.Diagnostics.Metrics;

namespace Sail.OpenTelemetry;

public class PerRequestMetrics : IDisposable
{
    private readonly Meter _meter;

    public PerRequestMetrics()
    {
        
    }
    
    public void Dispose()
    {
        _meter.Dispose();
    }
}