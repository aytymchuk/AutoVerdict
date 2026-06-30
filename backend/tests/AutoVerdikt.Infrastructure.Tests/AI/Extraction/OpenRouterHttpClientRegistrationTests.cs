using AutoVerdikt.Infrastructure;
using AutoVerdikt.Infrastructure.AI.Extraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace AutoVerdikt.Infrastructure.Tests.AI.Extraction;

public sealed class OpenRouterHttpClientRegistrationTests
{
    [Fact]
    public void CreateClient_does_not_throw_when_OpenRouterHeadersHandler_is_registered()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{OpenRouterOptions.SectionName}:BaseUrl"] = "https://openrouter.ai/api/v1",
                [$"{OpenRouterOptions.SectionName}:ExtractionModel"] = "google/gemini-2.5-flash",
            })
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        var factory = provider.GetRequiredService<IHttpClientFactory>();

        Should.NotThrow(() => factory.CreateClient(OpenRouterOptions.SectionName));
    }
}
