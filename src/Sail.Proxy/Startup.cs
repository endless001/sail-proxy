﻿using Prometheus;
using Sail.Kubernetes.Protocol;
using Sail.Metrics.Prometheus;

namespace Sail.Proxy;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAuthentication();
        services.AddCors();
        services.Configure<ReceiverOptions>(Configuration.Bind);
        services.AddHostedService<Receiver>();
        services.AddAllPrometheusMetrics();
        services.AddKubernetesPlugin().AddReverseProxy().LoadFromMessages();

        services.AddRequestMetricCollection();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRequestMetricCollection();
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy();
            endpoints.MapMetrics();
        });
    }
}