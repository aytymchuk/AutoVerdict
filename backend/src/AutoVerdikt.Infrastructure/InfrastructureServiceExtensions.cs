using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using System.Text.Json.Schema;
using System.Text.Json.Serialization.Metadata;
using AutoVerdikt.Application.AI.Extraction;
using AutoVerdikt.Application.Email;
using AutoVerdikt.Application.Research.Create.StartText;
using AutoVerdikt.Infrastructure.AI.Extraction;
using AutoVerdikt.Infrastructure.Email;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI;

namespace AutoVerdikt.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SendGridOptions>()
            .Bind(configuration.GetSection(SendGridOptions.SectionName));

        services.AddSingleton<ISendGridService, SendGridService>();

        services.AddOptions<OpenRouterOptions>()
            .Bind(configuration.GetSection(OpenRouterOptions.SectionName))
            .Validate(o => !string.IsNullOrWhiteSpace(o.BaseUrl), "OpenRouter:BaseUrl is required.")
            .Validate(o => !string.IsNullOrWhiteSpace(o.ExtractionModel), "OpenRouter:ExtractionModel is required.")
            .ValidateOnStart();

        services.AddTransient<OpenRouterHeadersHandler>();

        services.AddHttpClient(OpenRouterOptions.HttpClientName)
            .AddHttpMessageHandler<OpenRouterHeadersHandler>();

        services.AddSingleton<IChatClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<OpenRouterOptions>>().Value;
            var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient(OpenRouterOptions.HttpClientName);
            var loggerFactory = sp.GetRequiredService<ILoggerFactory>();

            var openAiClient = new OpenAIClient(
                new ApiKeyCredential(options.ApiKey),
                new OpenAIClientOptions
                {
                    Endpoint = new Uri(options.BaseUrl),
                    Transport = new HttpClientPipelineTransport(httpClient)
                });

            return openAiClient
                .GetChatClient(options.ExtractionModel)
                .AsIChatClient()
                .AsBuilder()
                .UseLogging(loggerFactory)
                .Build();
        });

        services.AddScoped<IExtractionService, ExtractionService>();
        services.AddHostedService<ExtractionSchemaValidationHostedService>();

        return services;
    }

    private sealed class ExtractionSchemaValidationHostedService(ILogger<ExtractionSchemaValidationHostedService> logger)
        : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var schemaOptions = new JsonSerializerOptions
            {
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
            };

            var schema = JsonSchemaExporter.GetJsonSchemaAsNode(
                schemaOptions,
                typeof(CarListingFacts),
                new JsonSchemaExporterOptions { TreatNullObliviousAsNonNullable = false });

            logger.LogDebug("Extraction schema for {Type}: {Schema}", nameof(CarListingFacts), schema);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
