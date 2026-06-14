using AutoVerdikt.Application.FeatureFlags;
using AutoVerdikt.Application.Whitelist;
using Microsoft.Extensions.DependencyInjection;

namespace AutoVerdikt.WebApi;

internal static class ApplicationServiceExtensions
{
    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddOptions<FeatureFlagOptions>()
            .BindConfiguration(FeatureFlagOptions.SectionName);

        services.AddSingleton<IFeatureFlagService, FeatureFlagService>();
        services.AddScoped<IWhitelistService, WhitelistService>();

        return services;
    }
}
