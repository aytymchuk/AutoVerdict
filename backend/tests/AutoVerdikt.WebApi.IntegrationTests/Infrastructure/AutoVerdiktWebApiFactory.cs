using AutoVerdikt.Application.AI.Extraction;
using AutoVerdikt.Application.Email;
using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Infrastructure.AI.Extraction;
using AutoVerdikt.Store.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.MongoDb;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public class AutoVerdiktWebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:8.0")
        .Build();

    public SpySendGridService SendGridSpy { get; } = new();

    public FakeExtractionService ExtractionFake { get; } = new();

    public bool WhitelistEnabled { get; set; } = true;

    public async Task InitializeAsync() => await _mongoContainer.StartAsync();

    public new async Task DisposeAsync()
    {
        await _mongoContainer.DisposeAsync();
        await base.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"{MongoDbOptions.SectionName}:ConnectionString"] = _mongoContainer.GetConnectionString(),
                [$"{MongoDbOptions.SectionName}:DatabaseName"] = $"autoverdikt_it_{Guid.NewGuid():N}",
                [$"{FeatureFlagOptions.SectionName}:WhitelistEnabled"] = WhitelistEnabled.ToString().ToLowerInvariant(),
                [$"{OpenRouterOptions.SectionName}:ApiKey"] = "test-key",
                [$"{OpenRouterOptions.SectionName}:BaseUrl"] = "https://openrouter.ai/api/v1",
                [$"{OpenRouterOptions.SectionName}:ExtractionModel"] = "google/gemini-2.5-flash",
                [$"{OpenRouterOptions.SectionName}:SiteUrl"] = "https://autoverdikt.com",
                [$"{OpenRouterOptions.SectionName}:SiteName"] = "AutoVerdikt",
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<ISendGridService>();
            services.AddSingleton<ISendGridService>(SendGridSpy);

            services.RemoveAll<IExtractionService>();
            services.AddSingleton<IExtractionService>(ExtractionFake);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                TestAuthHandler.SchemeName,
                _ => { });

            services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build())
                .AddPolicy("Admin", policy => policy.RequireRole("admin", "org:admin"));
        });
    }
}
