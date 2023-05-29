namespace Sail.Kubernetes.Protocol.Configuration;

public class CorsConfig
{
    public string Name { get; init; }
    public List<string> AllowOrigins { get; init; } 
    public List<string> AllowMethods { get; init; } 
    public List<string> AllowHeaders { get; init; }
}