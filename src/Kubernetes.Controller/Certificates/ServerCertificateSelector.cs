using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.Kubernetes;

namespace Sail.Kubernetes.Controller.Certificates;

internal class ServerCertificateSelector : IServerCertificateSelector
{
    private X509Certificate2 _defaultCertificate;

    public void AddCertificate(NamespacedName certificateName, X509Certificate2 certificate)
    {
        _defaultCertificate = certificate;
    }

    public X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName)
    {
        return _defaultCertificate;
    }

    public void RemoveCertificate(NamespacedName certificateName)
    {
        _defaultCertificate = null;
    }
}