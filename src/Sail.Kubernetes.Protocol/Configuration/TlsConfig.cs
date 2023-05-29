namespace Sail.Kubernetes.Protocol.Configuration;

public class TlsConfig
{
    public List<string> HostNames { get; init; }
    public string Cert { get; init; }
    public string Key { get; init; }
}