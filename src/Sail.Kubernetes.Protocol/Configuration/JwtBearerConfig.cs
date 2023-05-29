namespace Sail.Kubernetes.Protocol.Configuration;

public class JwtBearerConfig
{
    public string Name { get; init; }
    public string Secret { get; init; }
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string OpenIdConfiguration { get; init; }
}