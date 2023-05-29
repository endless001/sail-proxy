using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Sail.Kubernetes.Protocol.Configuration;

namespace Sail.Kubernetes.Protocol.Certificates;

public interface IServerCertificateSelector
{
    X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName);
    Task ReloadAsync(List<TlsConfig> tls);
}