using System.Text;
using k8s.Models;
using Sail.Kubernetes.Controller.Models;

namespace Sail.Kubernetes.Controller.Utilities;

internal static class CertificateHelper
{
    private const string TlsCertKey = "tls.crt";
    private const string TlsPrivateKeyKey = "tls.key";

    public static Certificate ConvertCertificate(IDictionary<string, byte[]> data)
    {
        try
        {
            var cert = data[TlsCertKey];
            var privateKey = data[TlsPrivateKeyKey];
            
            if (cert is null || cert.Length == 0 || privateKey is null || privateKey.Length == 0)
            {
                return null;
            }

            var certString = EnsurePemFormat(cert, "CERTIFICATE");
            var privateString = EnsurePemFormat(privateKey, "PRIVATE KEY");

            return new Certificate(certString, privateString);
        }
        catch (Exception)
        {
        }

        return null;
    }

    private static string EnsurePemFormat(byte[] data, string pemType)
    {
        var der = Encoding.ASCII.GetString(data);
        return !der.StartsWith("---", StringComparison.Ordinal)
            ? $"-----BEGIN {pemType}-----\n{der}\n-----END {pemType}-----"
            : der;
    }
}