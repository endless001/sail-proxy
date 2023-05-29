namespace Sail.Kubernetes.Protocol.Configuration;

public class PluginConfig
{
    public JwtBearerConfig JwtBearer { get; init; }
    public CorsConfig Cors { get; init; }
    public RateLimiterConfig RateLimiter { get; init; }
}