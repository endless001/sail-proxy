using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using Sail.Kubernetes.Protocol.Configuration;
using Sail.Kubernetes.Protocol.Options;

namespace Sail.Kubernetes.Protocol.Certificates;

public class ServerCertificateSelector : IServerCertificateSelector
{
    private List<TlsConfig> _tls = new();
    private readonly CertificateOptions _options;

    public ServerCertificateSelector(IOptions<CertificateOptions> options)
    {
        _options = options.Value;
    }

    public X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName)
    {
        var tls = _tls.FirstOrDefault(x => x.HostNames.Any(h => h == domainName));

        if (tls is null)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return X509Certificate2.CreateFromPemFile(_options.DefaultPath, _options.DefaultKeyPath);
            {
                using var convertedCertificate =
                    X509Certificate2.CreateFromPemFile(_options.DefaultPath, _options.DefaultKeyPath);
                return new X509Certificate2(convertedCertificate.Export(X509ContentType.Pkcs12));
            }
        }

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return X509Certificate2.CreateFromPem(tls.Cert, tls.Key);
        {
            using var convertedCertificate = X509Certificate2.CreateFromPem(tls.Cert, tls.Key);
            return new X509Certificate2(convertedCertificate.Export(X509ContentType.Pkcs12));
        }
    }

    public Task ReloadAsync(List<TlsConfig> tls)
    {
        _tls = tls;
        return Task.CompletedTask;
    }
}