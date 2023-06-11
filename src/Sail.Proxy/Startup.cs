using System.Diagnostics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Sail.Kubernetes.Protocol;
using Sail.Kubernetes.Protocol.Certificates;
using Sail.Kubernetes.Protocol.Options;

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
        services.Configure<CertificateOptions>(Configuration.GetSection("Certificate"));
        services.AddSingleton<IServerCertificateSelector, ServerCertificateSelector>();

        services.AddKubernetesPlugin()
            .AddReverseProxy()
            .ConfigureHttpClient((_, handler) =>
            {
                handler.ActivityHeadersPropagator = DistributedContextPropagator.CreatePassThroughPropagator();
            }).LoadFromMessages();

        services.AddOpenTelemetry()
            .ConfigureResource(builder =>
            {
                builder.AddService(Configuration.GetValue<string>("ServiceName"));
            })
            .WithTracing(builder =>
            {
                builder.AddSource(Configuration.GetValue<string>("ServiceName"))
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddJaegerExporter();

                builder.ConfigureServices(services =>
                {
                    services.Configure<JaegerExporterOptions>(Configuration.GetSection("Jaeger"));
                });
            })
            .WithMetrics(builder =>
            {
                builder
                    .AddMeter(Configuration.GetValue<string>("ServiceName"))
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation();

                builder.AddPrometheusExporter();
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseOpenTelemetryPrometheusScrapingEndpoint();
        app.UseEndpoints(endpoints => { endpoints.MapReverseProxy(); });
    }
}