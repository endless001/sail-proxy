using OpenTelemetry.Metrics;
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
        services.AddKubernetesPlugin().AddReverseProxy().LoadFromMessages();

        services.AddOpenTelemetry()
            .WithTracing(builder => { builder.AddAspNetCoreInstrumentation().AddConsoleExporter(); })
            .WithMetrics(builder =>
            {
                builder
                    .AddMeter("Sail")
                    .AddAspNetCoreInstrumentation();

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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapReverseProxy();
        });
    }
}