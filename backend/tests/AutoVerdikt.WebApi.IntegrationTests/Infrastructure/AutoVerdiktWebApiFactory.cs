using AutoVerdikt.Store.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;

namespace AutoVerdikt.WebApi.IntegrationTests.Infrastructure;

public sealed class AutoVerdiktWebApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:8.0")
        .Build();

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
                ["Clerk:Authority"] = "https://test.clerk.accounts.dev",
                ["Clerk:AuthorizedParty"] = "http://localhost",
                [$"{MongoDbOptions.SectionName}:ConnectionString"] = _mongoContainer.GetConnectionString(),
                [$"{MongoDbOptions.SectionName}:DatabaseName"] = $"autoverdikt_it_{Guid.NewGuid():N}",
            });
        });

        builder.ConfigureTestServices(services =>
        {
            services.PostConfigure<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
            });

            services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.SchemeName,
                    _ => { });
        });
    }
}
