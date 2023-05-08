using Sail.Logging;

namespace Microsoft.AspNetCore.Builder;


public static class RequestMetricCollectionExtensions
{
    public static IApplicationBuilder UseRequestMetricCollection(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestMetricCollectionMiddleware>();
    }
}
