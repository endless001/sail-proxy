using Sail.Logging;

namespace Microsoft.Extensions.DependencyInjection;

public static class RequestMetricCollectionServiceExtensions
{
    public static IServiceCollection AddRequestMetricCollection(this IServiceCollection services)
    {
        services.AddTelemetryConsumer<ForwarderTelemetryConsumer>();
        services.AddTelemetryConsumer<HttpClientTelemetryConsumer>();
        return services;
    }
}