using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace AutoVerdikt.WebApi.Observability;

internal static class OpenTelemetryExtensions
{
    private const string ServiceName = "autoverdict-api";

    internal static WebApplicationBuilder AddObservability(this WebApplicationBuilder builder)
    {
        var otlpEndpoint = builder.Configuration["OpenTelemetry:Otlp:Endpoint"]
            ?? "http://localhost:5341/ingest/otlp";

        var isTesting = builder.Environment.IsEnvironment("Testing");

        builder.Services
            .AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(ServiceName))
            .WithTracing(t =>
            {
                t.AddAspNetCoreInstrumentation();
                t.AddHttpClientInstrumentation();

                if (!isTesting)
                {
                    t.AddOtlpExporter(o =>
                    {
                        o.Endpoint = new Uri(otlpEndpoint);
                        o.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });
                }

                if (builder.Environment.IsDevelopment())
                    t.AddConsoleExporter();
            });

        builder.Logging
            .ClearProviders()
            .AddOpenTelemetry(o =>
            {
                o.IncludeFormattedMessage = true;
                o.IncludeScopes = true;
                o.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName));

                if (!isTesting)
                {
                    o.AddOtlpExporter(otlp =>
                    {
                        otlp.Endpoint = new Uri(otlpEndpoint);
                        otlp.Protocol = OtlpExportProtocol.HttpProtobuf;
                    });
                }

                if (builder.Environment.IsDevelopment())
                    o.AddConsoleExporter();
            });

        return builder;
    }
}
