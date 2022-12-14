using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.Kubernetes;

namespace Sail.Kubernetes.Controller.Certificates;

public interface IServerCertificateSelector
{
    /// <summary>
    /// Retrieve a certificate using the provided domain name.
    /// </summary>
    /// <param name="connectionContext">The connection context.</param>
    /// <param name="domainName">The domain name.</param>
    /// <returns>Either returns the specific certificate for the domain name, a wildcard certificates, or no certificate.</returns>
    X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName);

    /// <summary>
    /// Adds a certificate to the selector.
    /// </summary>
    /// <param name="certificateName">An identifier for the certificate that can be used to remove it.</param>
    /// <param name="certificate">The server certificate.</param>
    void AddCertificate(NamespacedName certificateName, X509Certificate2 certificate);

    /// <summary>
    /// Removes a certificate from the selector.
    /// </summary>
    /// <param name="certificateName">An identifier for the certificate that can be used to remove it.</param>
    void RemoveCertificate(NamespacedName certificateName);
}