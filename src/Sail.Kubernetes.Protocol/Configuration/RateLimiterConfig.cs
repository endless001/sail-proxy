namespace Sail.Kubernetes.Protocol.Configuration;

public class RateLimiterConfig
{
    public string Name { get; init; }
    public int PermitLimit { get; init; }
    public int Window { get; init; }
    public int QueueLimit { get; init; }
}