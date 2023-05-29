using Sail.Kubernetes.Controller.Caching;

namespace Sail.Kubernetes.Controller.Converters;

internal sealed class SailIngressContext
{
    public SailIngressContext(IngressData ingress, List<ServiceData> services, List<Endpoints> endpoints,
        List<PluginData> plugins, List<SecretData> secrets)
    {
        Ingress = ingress;
        Services = services;
        Endpoints = endpoints;
        Plugins = plugins;
        Secrets = secrets;
    }

    public SailIngressOptions Options { get; } = new();
    public IngressData Ingress { get; }
    public List<PluginData> Plugins { get; }
    public List<ServiceData> Services { get; }
    public List<Endpoints> Endpoints { get; }
    public List<SecretData> Secrets { get; }
}