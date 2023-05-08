using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Sail.Logging;

public class RequestMetricCollectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestMetricCollectionMiddleware> _logger;

    public RequestMetricCollectionMiddleware(RequestDelegate next, ILogger<RequestMetricCollectionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var metrics = RequestMetrics.Current;
        metrics.StartTime = DateTime.UtcNow;

        await _next(context);

        _logger.LogInformation($"RequestMetrics:  {metrics.ToJson()}");
    }
}