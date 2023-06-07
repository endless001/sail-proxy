using OpenTelemetry.Logs;
using Sail.Proxy;

public static class Program
{
    public static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHost(_ => { })
            .ConfigureLogging(builder => { builder.AddOpenTelemetry(options => { options.AddConsoleExporter(); }); })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseKubernetesReverseProxyCertificateSelector();
                webBuilder.UseStartup<Startup>();
            }).Build().Run();
    }
}