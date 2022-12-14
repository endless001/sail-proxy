using System.Security.Cryptography.X509Certificates;
using k8s.Models;
using Microsoft.Kubernetes;

namespace Sail.Kubernetes.Controller.Certificates;

public interface ICertificateHelper
{
    X509Certificate2 ConvertCertificate(NamespacedName namespacedName, V1Secret secret);
}