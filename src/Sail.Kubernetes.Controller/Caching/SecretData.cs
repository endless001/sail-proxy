using k8s.Models;

namespace Sail.Kubernetes.Controller.Caching;

public class SecretData
{
    public SecretData(V1Secret secret)
    {
        if (secret is null)
        {
            throw new ArgumentNullException(nameof(secret));
        }

        Data = secret.Data;
        Metadata = secret.Metadata;
    }

    public IDictionary<string, byte[]> Data { get; }
    public V1ObjectMeta Metadata { get; }
}