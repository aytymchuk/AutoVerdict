using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Research;
using AutoVerdikt.Application.Whitelist;
using Microsoft.Extensions.Caching.Hybrid;

namespace AutoVerdikt.WebApi;

internal static class ApplicationServiceExtensions
{
    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromSeconds(60),
                Expiration = TimeSpan.FromSeconds(60),
            };
        });
        services.AddOptions<FeatureFlagOptions>()
            .BindConfiguration(FeatureFlagOptions.SectionName);

        services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
        services.AddScoped<IWhitelistService, WhitelistService>();
        services.AddResearchServices();

        return services;
    }
}
